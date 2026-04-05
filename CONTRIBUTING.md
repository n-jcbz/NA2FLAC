# Contributing to NA2FLAC

Thanks for thinking about contributing — welcome!  
NA2FLAC is now a small .NET 8 project with two builds: a WPF UI and a legacy console build. This doc explains the current setup and how to help.

---

## Quick overview
- **WPF build (v2.0)** — `NA2FLAC_v2.0_WPF`  
  - Full .NET 8 WPF UI (frameless, rounded window).  
  - Uses `dependencies` folder for third-party binaries (`vgmstream-cli.exe`, `ffmpeg.exe`, `ffprobe.exe`) and `licenses` for license files.
- **Legacy build (v2.0 - legacy)** — console C# port (same conversion pipeline, old workflow).
- Installer scripts live in `devbuilds/*/installer_script.nsi`.
- If you need the repo tree for context, check the `devbuilds` and `bin` folders (dependencies are included in `devbuilds/*/dependencies`).

---

## Before you start
- Install the **.NET 8 SDK** and a suitable editor/IDE (Visual Studio 2022/2023 or VS Code with C# extensions).
- Windows 10 or later is required for the .NET 8 builds.
- Keep third-party binaries and their licenses: `vgmstream-cli`, `ffmpeg`, `ffprobe` + the VGMStream/FFmpeg DLLs and license files.

---

## Simple dev workflow
1. **Fork** the repo.  
2. `git clone <your-fork>`  
3. `git checkout -b feature/whatever-you-are-doing`  
4. Make changes (C# or XAML). You can change XAML freely — UI edits are welcome.  
5. Build & run:
   - Visual Studio: open the solution/project and run the WPF project.
   - CLI: `dotnet build` and `dotnet run --project path\to\NA2FLAC_v2.0_WPF.csproj`
6. Test with real files. Keep `dependencies` present (either copy from `devbuilds/v2.0/wpf/dependencies` or keep them in the project folder).
7. Commit & push your branch, open a PR with a clear description and required test steps.

---

## What to edit / hotspots
- **Size estimation:** `EstimateFlacMultiplier(...)` in `MainWindow.cs` (comment says `Check EstimateFlacMultiplier @ line 320`).  
  - If you want to adjust format multipliers or add specific suffix rules, do it there. Small tests per-file help a lot (we added several suffix-based BRSTM rules).
- **Conversion logic:** decoding/merging/conversion loops are in `Convert_Click` in the WPF project and in the legacy `Program` file for console build.
- **UI:** XAML files are the place to tweak layout, colors (`#47A097` is the app color), and title-bar controls (close/minimize implemented already).
- **Installers:** NSIS scripts are in `devbuilds/*/installer_script.nsi` — update if you change folder layout or add/remove files.

---

## Testing & QA
- Provide a short test-case in your PR (example: one sample file and expected multiplier/behavior). If sample files are too big to attach, provide a short list of filenames + sizes and the expected result.
- For multiplier tweaks: pick one representative file, run scan, run conversion, measure before/after and iterate. Consider adding a dev-only “calibrate” helper (pick a file, run conversion, compute multiplier).
- Keep edge cases in mind: `_l/_r` stereo merging, per-game suffix variants (e.g. `_32`, `_only32`, `_32_n`, `.ry.32.brstm`, `.32.c4.brstm`) — BRSTM is especially fiddly.

---

## PR checklist (short)
- [ ] Describe the change clearly.
- [ ] Include how to reproduce/test.
- [ ] Update `README.md` or notes if behaviour changes.
- [ ] If you modify dependencies, include license updates or mention where binaries are expected.
- [ ] Keep commits small & focused.

---

## Issues / bug reports
When opening an issue, include:
- OS + .NET SDK version.
- Build tested (WPF or Legacy).
- A minimal repro or sample filenames + sizes.
- Expected vs actual behaviour.
- Any logs or console output (UI: the bottom status textbox shows scan/conversion messages).

---

## Style & code notes
- Keep code readable and commented (we like clear names: `file`, `allFiles`, `depDir`, `EstimateFlacMultiplier`, etc.).
- XAML: keep UI changes modular (small, reversible tweaks) and leave `resources` the way they are unless you improve consistency.
- Tests: add notes in your PR about which game/formats you tested (BRSTM/AST/ADX/etc.).

---

## License & attribution
By contributing, you agree to license your work under the repo license. Respect third-party licenses — keep license files in `licenses`.

---

## Small dev notes / tips
- If you touch the size estimation, mention the datasets you used (e.g. “Wii Party — 581 BRSTM files”).
- The WPF status textbox `txtStatus` is used for user-facing messages instead of system popups — aim to keep messages concise and helpful.
- The app color is `#47A097` — use it for UI accents if you add controls.