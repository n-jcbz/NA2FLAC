# NA2FLAC

**Nintendo Audio to FLAC Converter**

NA2FLAC is a lightweight batch-based audio conversion tool that scans and converts Nintendo audio formats into high-quality FLAC files.  
It guides the user through each step with simple yes/no prompts, giving you full control over scanning, conversion, and file organization.

---

## Features

- **Interactive scanning and conversion**
  - The program asks for confirmation before scanning and converting, giving you full control.

- **Supported formats (as of v1.2.1)**
  - `AST`, `BRSTM`, `BCSTM`, `BFSTM`, `BFWAV`, `BWAV`, `SWAV`, `STRM`, `LOPUS`, `IDSP`, `HPS`, `DSP`, `ADX`, `MP3`, and `OGG`

- **Intelligent channel handling**
  - Files split into `_l` (left) and `_r` (right) channels are automatically detected and merged into a single stereo track.

- **Smart output format**
  - Files with up to **8 channels** are converted to **FLAC**
  - Files with more than 8 channels are exported as **WAV**      
    *Some players may not be able to play files with more than 8 channels!*

- **Organized output with full folder tree**
  - Converted files are placed into a mirrored `converted` folder, preserving the original folder structure.
  - Original files remain untouched.

- **Automatic organization**
  - A dedicated `NA2FLAC` folder is created during conversion to store all tools (`vgmstream-cli.exe`, `ffmpeg.exe`, `ffprobe.exe`, `DLLs`) and licenses.
  - Licenses are sorted into the `licenses` subfolder.

- **Simple installer setup**
  - Distributed as an **NSIS-based installer** for a clean and reliable installation process.
  - All required tools and licenses are placed directly into the folder where the installer is launched.

---

## How It Works

1. Place the **installer** (`NA2FLAC_1.2.1_Installer.exe`) in the folder that contains your audio files or subfolders.  
2. Run the installer — it will extract all necessary files into the same location.  
3. Run `NA2FLAC_1.2.1_x64.exe`.  
4. Follow the on-screen prompts (`y/n`) to scan and convert supported files.  
5. Find your converted FLAC (or WAV) files inside the new `converted` folder, organized just like your originals.  
6. Check the `NA2FLAC` folder for tools, DLLs, licenses, and the installer after conversion.

---

## Requirements

- Windows 10 or later  
- Comes prepackaged with **FFmpeg**, **FFprobe**, and **VGMStream** — no additional setup required.

---

## Licenses

- **NA2FLAC** — MIT License (see `NA2FLAC/licenses/LICENSE.txt`)  
- **FFmpeg / FFprobe** — GNU GPL v3 (see `NA2FLAC/licenses/FFMPEG_COPYING.GPLv3.md` and `FFMPEG_LICENSE.md`)  
  Visit [https://ffmpeg.org](https://ffmpeg.org/)
- **VGMStream** — MIT License (see `NA2FLAC/licenses/VGMSTREAM_COPYING.md`)  
  Visit [https://vgmstream.org](https://vgmstream.org/)
- **BatToExe Portable** — MIT License (see `NA2FLAC/licenses/BatToExePortable_LICENSE.md`)  
  Visit [https://github.com/Makazzz/BatToExePortable](https://github.com/Makazzz/BatToExePortable)
- **NSIS (Nullsoft Scriptable Install System)** — zlib/libpng License (see `NA2FLAC/licenses/NSIS_COPYING.md`)  
  Visit [https://nsis.sourceforge.io](https://nsis.sourceforge.io)

---

## Credits

- FFmpeg and FFprobe by the FFmpeg developers  
- VGMStream by the VGMStream team  
- BatToExe Portable by Makazzz  
- NSIS Installer System by the NSIS developers  
- NA2FLAC by n.jcbz
