# NA2FLAC

**Nintendo Audio to FLAC Converter**

NA2FLAC is a lightweight batch-based audio conversion tool that scans and converts Nintendo audio formats into high-quality FLAC files.  
It guides the user through each step with simple prompts (console) or a small GUI (WPF), giving you full control over scanning, conversion, and file organization.

---

## Builds

- **WPF build (v2.0)** — modern .NET 8 WPF UI with folder selection, output folder option, progress bar, and status text. Designed for easy, visual batch conversion and to work from any selected input/output folder.
- **Legacy build (v2.0 Legacy)** — console application ported to C#, matching the original v1.2.1 behaviour for users who prefer the simple console workflow.

---

## Features

- **Interactive scanning and conversion**
  - Console build prompts with simple yes/no answers.
  - WPF build provides an explicit Scan / Convert flow with progress updates, status text, and visual controls.

- **Supported formats**
  - `AST`, `BRSTM`, `BCSTM`, `BFSTM`, `BFWAV`, `BWAV`, `SWAV`, `STRM`, `LOPUS`, `IDSP`, `HPS`, `DSP`, `ADX`, `MP3`, and `OGG`

- **Intelligent channel handling**
  - Files split into `_l` (left) and `_r` (right) channels are automatically detected and merged into a single stereo track where appropriate.

- **Smart output format**
  - Files with up to **8 channels** are converted to **FLAC**.
  - Files with more than 8 channels are exported as **WAV** (some players may not support >8 channels).

- **Organized output with preserved folder tree**
  - Converted files are placed into a mirrored `converted` folder that preserves the original folder structure.
  - By default the WPF build lets you pick a separate output directory; if left empty it defaults to the input folder (it still creates `converted` there).

- **File-size estimation**
  - Scans report `original -> ~estimated after conversion` using per-format heuristics to give a reasonable ballpark for final FLAC size.

- **Per-file progress**
  - Both builds display a `(N/Total) Processing ...` counter while converting.

- **Automatic cleanup**
  - WAV intermediate files are deleted automatically when a FLAC is created successfully.
  - WAVs are kept if conversion fails or if the file has too many channels.

- **Tidied dependency layout**
  - App files live in the install folder; third-party binaries (ffmpeg, vgmstream, ffprobe and their native DLLs) are placed in a `dependencies` folder.
  - Licenses are grouped in `NA2FLAC/licenses` (or `dependencies/licenses` in installer layout) for clarity.

- **Installer**
  - NSIS-based installer available (Program Files variant creates desktop shortcut, Start Menu entry, and Uninstaller).
  - Installer places the app files, `dependencies`, and `licenses` in the chosen install location.

---

## How It Works (WPF)

1. Run the installer (or unzip the package) to your chosen folder (Program Files or other).
2. Run the WPF app (`NA2FLAC v2.0.exe`) from the install location.
3. Use *Browse* to choose your input folder (the folder with your audio files), and optionally set an output folder.
4. Click **Scan** — app will scan supported audio formats and show counts and a size estimate.
5. Click **Convert** — progress bar and status text update while the app converts files (vgmstream → WAV → ffmpeg → FLAC).
6. Converted files appear in the `converted` folder inside the selected output (or input) directory, preserving folder structure.

---

## How It Works (Legacy / Console)

1. Place the installer or legacy executable in the folder containing your audio files (or run it from any folder after installing).
2. Run `NA2FLAC_2.0_legacy.exe` (console).
3. Confirm the prompts to scan and convert.
4. Converted files are written into a `converted` folder inside the folder where you ran the exe (preserving structure).

---

## Requirements

- Windows 10 or later  
- **WPF build:** .NET 8 runtime (the WPF app is built for .NET 8).  
- No separate install needed for ffmpeg/ffprobe/vgmstream — they are bundled in `dependencies` by the installer.

---

## Licenses

- **NA2FLAC** — MIT License (see `NA2FLAC/licenses/LICENSE.txt`)  
- **FFmpeg / FFprobe** — GNU GPL v3 (see `NA2FLAC/licenses/FFMPEG_COPYING.GPLv3.md` and `FFMPEG_LICENSE.md`)  
  Visit https://ffmpeg.org  
- **VGMStream** — MIT License (see `NA2FLAC/licenses/VGMSTREAM_COPYING.md`)  
  Visit https://vgmstream.org  
- **BatToExe Portable** — MIT License (see `NA2FLAC/licenses/BatToExePortable_LICENSE.md`)  
  Visit https://github.com/Makazzz/BatToExePortable  
- **NSIS (Nullsoft Scriptable Install System)** — zlib/libpng License (see `NA2FLAC/licenses/NSIS_COPYING.md`)  
  Visit https://nsis.sourceforge.io

---

## Credits

- FFmpeg and FFprobe by the FFmpeg developers  
- VGMStream by the VGMStream team  
- BatToExe Portable by Makazzz (used in pre-v2.0 builds)  
- NSIS Installer System by the NSIS developers  
- NA2FLAC by n.jcbz

---

## Notes & Caveats

- Size estimations are heuristic — streamed formats (BFSTM, etc.) can be variable and may require tuning of multipliers for better accuracy.
- No persistent logs by default to keep things simple and minimal; optional logging can be added later.

---

## Contact / Contribute

If you want to help tune estimations, add new formats, or test installer variants, open an issue or a PR in the repository. Pull requests that add format support should include a short test file and expected behavior.
