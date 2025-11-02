# NA2FLAC

**Nintendo Audio to FLAC Converter**

NA2FLAC is a lightweight batch-based audio conversion tool that scans and converts Nintendo audio formats into high-quality FLAC files.
It guides the user through each step with simple yes/no prompts, giving you full control over scanning, conversion, and file organization.

---

## Features

- **Interactive scanning and conversion**
  - The program asks for confirmation before scanning and converting, giving you full control.

- **Currently supported formats (as of v1.2)**
  - `AST`, `BRSTM`, `BCSTM`, `BFSTM`, `BFWAV`, `BWAV`, `SWAV`, `STRM`, `LOPUS`, `IDSP`, `HPS`, `DSP`, `ADX`, `MP3`, and `OGG`

- **Intelligent channel handling**
  - Files split into `_l` (left) and `_r` (right) channels are automatically detected and merged into a single stereo track.

- **Smart output format**
  - Files with up to **8 channels** are converted to **FLAC**
  - Files with more than 8 channels are exported as **WAV**      
    *Some players may not be able to play files with more than 8 channels!*

- **Organized output with full folder tree**
  - Converted files are copied into a mirrored folder structure starting from the folder where `NA2FLAC_1.2_x64.exe` is launched.
  - Relative paths are preserved, so all subfolders and file organization remain intact.

- **Dedicated tools & license folders**
  - A dedicated `NA2FLAC` folder is created to store all tools (`vgmstream-cli.exe`, `ffmpeg.exe`, `ffprobe.exe`, `DLLs`) and a `licenses` subfolder.
  - Licenses are automatically moved into the `licenses` subfolder.
  - After conversion, the executable is also moved into the `NA2FLAC` folder to keep everything organized.

---

## How It Works

1. Place your supported audio files anywhere in the folder tree starting from where the program is launched
2. Run `NA2FLAC_1.2_x64.exe`
3. Wait for extraction to finish
4. Follow the on-screen prompts (`y/n`) to scan and convert files
5. Check the `converted` folder for your FLAC (or WAV) tracks organized in the same folder structure as your originals
6. After conversion, check the `NA2FLAC` folder for tools, DLLs, licenses, and the moved executable

---

## Requirements

- Windows 10 or later
- Includes prepackaged **FFmpeg**, **FFprobe**, and **VGMStream** — no additional setup required.

---

## Licenses

- **NA2FLAC** — MIT License (see `NA2FLAC/licenses/LICENSE.txt`)
- **FFmpeg / FFprobe** — GNU GPL v3 (see `NA2FLAC/licenses/FFMPEG_COPYING.GPLv3.md` and `FFMPEG_LICENSE.md`)      
  Visit [https://ffmpeg.org](https://ffmpeg.org/)
- **VGMStream** — MIT License (see `NA2FLAC/licenses/VGMSTREAM_COPYING.md`)        
  Visit [https://vgmstream.org](https://vgmstream.org/)
- **BatToExe Portable** — MIT License (see `NA2FLAC/licenses/BatToExePortable_LICENSE.md`)        
  Visit [https://github.com/Makazzz/BatToExePortable](https://github.com/Makazzz/BatToExePortable)

---

## Credits

- FFmpeg and FFprobe by the FFmpeg developers
- VGMStream by the VGMStream team
- Compiled using BatToExe Portable by Makazzz
- NA2FLAC by n.jcbz
