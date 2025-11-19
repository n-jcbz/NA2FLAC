# Notes & Thoughts

- UI tweaks (wasn't planned, I just couldn't help myself)
  - Added **minimize** button to WPF.
  - Error messages in WPF now show in the bottom status textbox (no system popups except if output folder doesn't exist).
  - Temporarily disabled "Open output" button
  - Adjusted various margins
  - Added time spent converting in summary message
  - Added file types found + count, just like in the legacy build

- BRSTM is a mess — many filename variants change real output size.
  - Added a few special-case suffix rules to handle common patterns.
  - Still need more samples for edge cases (per-game variations are problematic).

- Current estimation rules (short summary)
  - Suffix-based BRSTM special cases:                                         
    - `*_32.brstm` → **4.7** (found this in Wii Party)
    - `*_only32.brstm` → **1** (keeps these small “only” files small in calculation and just so this is already taken care of)
    - `*32_n.brstm`, `*32_f.brstm` → **1** (Mario Kart Wii cases)
    - `*.ry.32.brstm` → **1.35** (found this in Wii Sports Resort)
    - `*.32.c4.brstm` → **1.3** (found this in Wii Sports)
    - plain `.brstm` → **5.3** (tested with Mario Party 8 — has standard naming, may suggest no variation was used)                                            
  - Other formats (defaults / tested where noted):
    - `.bfstm` → **4.5** (tested)
    - `.bcstm` → **4.5** (untested)
    - `.bwav` → **2.3** (tested)
    - `.bfwav`, `.dsp` → **1.5** (untested)
    - `.strm` → **0.56** (tested — can shrink)
    - `.ast` → **1.22** (tested)
    - `.adx` → **4.23** (tested)
    - `.ogg` / `.mp3` → **2.5** (tested)
    - `.custom` → **1.0**
    - fallback → **2.5**
   
- Had to update the Legacy installer since it had an unfinished version for some reason                                
  (it didn't have size estimation and file count during conversion)
- Fixed a small problem in the WPF build that would create a `licenses` folder in `\dependencies`

- Quick status / to-do
  - BRSTM still needs systematic per-game sampling — suffixes like `_32`, `_only32`, `_32_n`, `_32_f` probably behave differently across titles.
  - Keep `*_only32` and `*32_*` cases conservative (1 or ~1) until there's more data.
  - Consider adding an optional “calibrate from sample file” dev tool later (pick one file, run conversion, derive multiplier).
  - I'm gonna crash out fr, ong they do ts on purpose chat 🙏

- TL;DR: special-cases are in; BRSTM is the main headache; more sample files per game will tighten multipliers.

### Check [v2.0 release](https://github.com/n-jcbz/NA2FLAC/releases/tag/v2.0), updated versions replaced old builds
