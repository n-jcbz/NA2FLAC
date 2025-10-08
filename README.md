# NA2FLAC  
**Nintendo Audio to FLAC Converter**

NA2FLAC is a lightweight batch-based audio conversion tool that scans and converts Nintendo audio formats into high-quality FLAC files.  
It guides the user through each step with simple yes/no prompts, giving you full control over scanning, conversion, and file organization.

---

## Features
- **Interactive scanning and conversion**  
  - The program asks for confirmation before scanning, converting, and sorting, giving you full control.
- **Supported formats**  
  - `AST`, `BRSTM`, `BCSTM`, `BFSTM`, `BFWAV`, `BWAV`, `LOPUS`, `IDSP`, `HPS`, and `DSP`
- **Intelligent channel handling**  
  - AST files split into `_l` (left) and `_r` (right) channels are automatically detected and merged into a single stereo track.
- **Smart output format**  
  - Files with up to **8 channels** are converted to **FLAC**  
  - Files with more than 8 channels are exported as **WAV**
- **Organized output**  
  - Converted files are sorted into folders:  
    `FLAC_Tracks`, `AST_Tracks`, `BRSTM_Tracks`, `BCSTM_Tracks`, etc.  
    (`.wav` files are placed into the `FLAC_Tracks` folder.)

---

## How It Works
1. Place your supported audio files in the same directory as the program.  
2. Run **NA2FLAC.exe**.  
3. Follow the on-screen prompts (`y/n`) to scan, convert, and sort files.  
4. Check the generated folders for your converted tracks.

---

## Requirements
- Windows 10 or later  
- Includes prepackaged **FFmpeg**, **FFprobe**, and **VGMStream** — no additional setup required.

---

## Licenses
- **NA2FLAC** — Licensed under the MIT License (see `LICENSE`)
- **FFmpeg / FFprobe** — Licensed under the GNU General Public License v3 (GPLv3)  
  See `licenses/LICENSE_FFMPEG_GPLv3.txt` or visit [https://ffmpeg.org](https://ffmpeg.org/)
- **VGMStream** — Licensed under the MIT License (see `licenses/LICENSE_VGMSTREAM.txt`)  
  Visit [https://vgmstream.org](https://vgmstream.org/)

---

## Credits
- FFmpeg and FFprobe by the FFmpeg developers  
- VGMStream by the VGMStream team  
- NA2FLAC by n.jcbz
