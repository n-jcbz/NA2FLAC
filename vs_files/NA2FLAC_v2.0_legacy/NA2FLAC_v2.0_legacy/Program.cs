using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

// BUILD TYPE: LEGACY — STABLE — IN DEVELOPMENT — Check EstimateFlacMultipler @ line 271

// ANYTHING REFERRED TO AS "CUSTOM" AND "custom" ARE PLACEHOLDERS FOR FUTURE FORMAT SUPPORT.
// YOU CAN REPLACE THEM WITH ANY PROPRIETARY AUDIO FORMAT TO TEST COMPATIBILITY.
// KEEP IN MIND CASE-SENSITIVITY! REPLACE "CUSTOM" AND "custom" ACCORDINGLY.
// IF YOU'RE CONTRIBUTING TO FORMAT SUPPORT: PLEASE RE-ADD THE CUSTOM PARTS UNDER/AFTER THE NEW FORMAT.


class NA2FLAC
{
    static string baseDir = AppDomain.CurrentDomain.BaseDirectory;
    static string depDir = Path.Combine(baseDir, "NA2FLAC");
    static string licenseDir = Path.Combine(depDir, "licenses");

    static string vgm = Path.Combine(depDir, "vgmstream-cli.exe");
    static string ffmpeg = Path.Combine(depDir, "ffmpeg.exe");
    static string ffprobe = Path.Combine(depDir, "ffprobe.exe");

    static void Main()
    {
        Console.Title = "Nintendo Audio to FLAC Converter v2.0 (Legacy)";

        Directory.CreateDirectory(depDir);
        Directory.CreateDirectory(licenseDir);

        string[] docFiles = { "CODE_OF_CONDUCT.md", "CONTRIBUTING.md", "README.md", "NA2FLAC_2.0_legacy.txt" };
        foreach (var doc in docFiles)
        {
            string srcPath = Path.Combine(baseDir, doc);
            string destPath = Path.Combine(depDir, doc);
            if (File.Exists(srcPath) && !File.Exists(destPath))
                File.Move(srcPath, destPath);
        }

        string[] depFiles = {
            "vgmstream-cli.exe","ffmpeg.exe","ffprobe.exe",
            "avcodec-vgmstream-59.dll","avformat-vgmstream-59.dll","avutil-vgmstream-57.dll",
            "libatrac9.dll","libcelt-0061.dll","libcelt-0110.dll","libg719_decode.dll",
            "libmpg123-0.dll","libspeex-1.dll","libvorbis.dll","swresample-vgmstream-4.dll"
        };

        foreach (var file in depFiles)
        {
            string src = Path.Combine(baseDir, file);
            string dest = Path.Combine(depDir, file);
            if (File.Exists(src) && !File.Exists(dest)) File.Move(src, dest);
        }

        string[] licenseFiles = {
            "FFMPEG_COPYING.GPLv3.md","FFMPEG_LICENSE.md",
            "VGMSTREAM_COPYING.md","LICENSE.txt","NSIS_COPYING.md"
        };
        foreach (var file in licenseFiles)
        {
            string src = Path.Combine(baseDir, file);
            string dest = Path.Combine(licenseDir, file);
            if (File.Exists(src) && !File.Exists(dest)) File.Move(src, dest);
        }

        Console.WriteLine("\n===========================================");
        Console.WriteLine("   Nintendo Audio to FLAC Converter v2.0");
        Console.WriteLine("===========================================\n");

        var missingDeps = depFiles.Where(f => !File.Exists(Path.Combine(depDir, f))).ToArray();
        if (missingDeps.Any())
        {
            Console.WriteLine("Warning: The following required files are missing:");
            foreach (var f in missingDeps) Console.WriteLine(f);
            Console.WriteLine("Please make sure all files are inside the NA2FLAC folder.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        if (!PromptYesNo("Start scan? (y/n): ")) return;

        Console.WriteLine("Checking for files...");
        Thread.Sleep(2000);

        string[] supportedExts = { ".ast",".brstm",".bcstm",".bfstm",".bfwav",".bwav",".swav",".strm",
                                   ".lopus",".idsp",".hps",".dsp",".adx",".mp3",".ogg",".custom" };

        var allFiles = supportedExts.SelectMany(ext => Directory.GetFiles(baseDir, "*" + ext, SearchOption.AllDirectories)).ToArray();
        int totalFiles = allFiles.Length;

        if (totalFiles == 0)
        {
            Console.WriteLine("No supported files found.");
            Console.WriteLine("Please move the executable into the folder containing your audio files or subfolders.");
            Console.WriteLine("Supported formats: AST, BRSTM, BCSTM, BFSTM, BFWAV, BWAV, SWAV, STRM, LOPUS, IDSP, HPS, DSP, ADX, MP3, OGG");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        foreach (var ext in supportedExts)
        {
            int count = Directory.GetFiles(baseDir, "*" + ext, SearchOption.AllDirectories).Length;
            if (count > 0) Console.WriteLine($"{count} {ext.Trim('.').ToUpper()} files found!");
        }

        // -------------------- Size estimation --------------------
        long totalOriginalBytes = 0;
        double estimatedFlacBytes = 0;

        foreach (var file in allFiles)
        {
            long fileSize = new FileInfo(file).Length;
            totalOriginalBytes += fileSize;

            string ext = Path.GetExtension(file).ToLower();
            double multiplier = EstimateFlacMultiplier(ext);
            estimatedFlacBytes += fileSize * multiplier;
        }

        string originalSize = FormatSize(totalOriginalBytes);
        string estimatedSize = FormatSize((long)estimatedFlacBytes);

        Console.WriteLine($"({totalFiles} total, {originalSize} -> ~{estimatedSize} after conversion)");

        if (!PromptYesNo("Convert to FLAC? (y/n): ")) return;

        Console.WriteLine("Starting conversion in 5 seconds...");
        Thread.Sleep(5000);

        string targetRoot = Path.Combine(baseDir, "converted");
        Directory.CreateDirectory(targetRoot);

        int converted = 0, failed = 0, wavKept = 0;
        var startTime = DateTime.Now;

        for (int i = 0; i < allFiles.Length; i++)
        {
            var filePath = allFiles[i];
            string relPath = Path.GetRelativePath(baseDir, Path.GetDirectoryName(filePath));
            string destDir = Path.Combine(targetRoot, relPath);
            Directory.CreateDirectory(destDir);

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string wavPath = Path.Combine(destDir, fileName + ".wav");

            Console.WriteLine($"({i + 1}/{totalFiles}) Processing {filePath}...");

            RunProcess(vgm, $"\"{filePath}\" -o \"{wavPath}\"");

            bool merged = false;
            if (fileName.EndsWith("_l"))
            {
                string rightName = fileName.Substring(0, fileName.Length - 2) + "_r";
                string rightPath = Path.Combine(destDir, rightName + ".wav");
                if (File.Exists(rightPath))
                {
                    string baseName = fileName.Substring(0, fileName.Length - 2);
                    string flacPath = Path.Combine(destDir, baseName + ".flac");
                    RunProcess(ffmpeg, $"-y -i \"{wavPath}\" -i \"{rightPath}\" -filter_complex \"[0:a][1:a]amerge=inputs=2[a]\" -map \"[a]\" -c:a flac \"{flacPath}\"");
                    if (File.Exists(flacPath))
                    {
                        File.Delete(wavPath);
                        File.Delete(rightPath);
                        converted++;
                        merged = true;
                    }
                    else wavKept++;
                }
            }

            if (!merged)
            {
                int channels = 0;
                var ffprobeOutput = RunProcessCapture(ffprobe, $"-v error -select_streams a:0 -show_entries stream=channels -of default=noprint_wrappers=1:nokey=1 \"{wavPath}\"");
                if (int.TryParse(ffprobeOutput.Trim(), out int ch)) channels = ch;

                if (channels <= 8)
                {
                    string flacPath = Path.Combine(destDir, fileName + ".flac");
                    RunProcess(ffmpeg, $"-y -i \"{wavPath}\" -c:a flac \"{flacPath}\"");
                    if (File.Exists(flacPath))
                    {
                        File.Delete(wavPath);
                        converted++;
                    }
                    else
                    {
                        Console.WriteLine($"Conversion failed for {wavPath}");
                        failed++;
                    }
                }
                else
                {
                    Console.WriteLine($"Keeping {wavPath} because it has {channels} channels");
                    wavKept++;
                }
            }
        }

        var endTime = DateTime.Now;
        var totalTime = endTime - startTime;

        Console.WriteLine("\n=======================================");
        Console.WriteLine("Conversion Summary");
        Console.WriteLine("=======================================");
        Console.WriteLine($"Files converted (FLAC): {converted}");
        Console.WriteLine($"Files kept as WAV (too many channels or merge failed): {(wavKept > 0 ? wavKept.ToString() : "None")}");
        Console.WriteLine($"Failed to convert: {(failed > 0 ? failed.ToString() : "None")}");
        Console.WriteLine($"Total time: {totalTime.Minutes}m {totalTime.Seconds}s");
        Console.WriteLine("=======================================\n");

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static bool PromptYesNo(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (input == "y") return true;
            if (input == "n") return false;
            Console.WriteLine("Please type y or n.");
        }
    }

    static void RunProcess(string exe, string args)
    {
        var psi = new ProcessStartInfo(exe, args)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false
        };
        using var process = Process.Start(psi);
        process.WaitForExit();
    }

    static string RunProcessCapture(string exe, string args)
    {
        var psi = new ProcessStartInfo(exe, args)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var process = Process.Start(psi);
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }

    static string FormatSize(long bytes)
    {
        double size = bytes;
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        int unitIndex = 0;
        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }
        return $"{size:0.##} {units[unitIndex]}";
    }

    static double EstimateFlacMultiplier(string extension) // Variations can appear and mess up estimations
    {
        if (extension.EndsWith("_32.brstm")) return 4.7; // BRSTM variation, tested with Wii Party

        if (extension.EndsWith("_only32.brstm")) return 1; // Specific case for Mario Kart Wii? will stay at 1
        // Set to 1 since there's currently not enough testing data to decide any other value:
        if (extension.EndsWith("32_n.brstm")) return 1; // Specific case for Mario Kart Wii
        if (extension.EndsWith("32_f.brstm")) return 1; // Specific case for Mario Kart Wii

        if (extension.EndsWith(".ry.32.brstm")) return 1.35; // BRSTM variation, tested with Wii Sports Resort
        if (extension.EndsWith(".32.c4.brstm")) return 1.3; // BRSTM variation, tested with Wii Sports
        if (extension == ".brstm") return 5.3; // Tested with Mario Party 8 (seemingly standard BRSTM files since no "32" is in file names)
        return extension switch
        {   
            ".bcstm" => 4.5, // Not tested yet
            ".bfstm" => 4.5, // Tested — Accurate (MK8 Deluxe + 3D World/Bowser's Fury + Odyssey, 620 files total)
            ".bwav" => 2.3, // Tested — Accurate (ACNH, 860 files total)
            ".bfwav" => 1.5, // Not tested yet
            ".dsp" => 1.5, // Not tested yet
            ".hps" => 2.3, // Not tested yet
            ".strm" => 0.56, // Tested — Accurate (MySims on Wii, 158 files total) (The only format here that goes down in size, interesting...)
            ".swav" => 2.3, // Not tested yet
            ".lopus" => 5.0, // Not tested yet
            ".ast" => 1.22, // Tested — Accurate (Galaxy 1 + 2 on Wii, 170 files total)
            ".idsp" => 2.0, // Not tested yet
            ".adx" => 4.23, // Tested — Accurate (M&S OWG on Wii, 106 files total)
            ".ogg" or ".mp3" => 2.5, // Tested — Accurate (Minecraft on Switch + Random MP3 files)
            ".custom" => 1.0, 
            _ => 2.5 // Fallback for unknown formats
        };
    }
}