@echo off
setlocal enabledelayedexpansion
cd /d "%~dp0%"

:: BUILD TYPE: STABLE

:: ANYTHING REFERRED TO AS "CUSTOM" AND "custom" ARE PLACEHOLDERS FOR FUTURE FORMAT SUPPORT.
:: YOU CAN REPLACE THEM WITH ANY PROPRIETARY AUDIO FORMAT TO TEST COMPATIBILITY.
:: KEEP IN MIND CASE-SENSITIVITY! REPLACE "CUSTOM" AND "custom" ACCORDINGLY.
:: IF YOU'RE CONTRIBUTING TO FORMAT SUPPORT: PLEASE RE-ADD THE CUSTOM PARTS UNDER/AFTER THE NEW FORMAT.

:: dependency & license sorting
set "depDir=%~dp0NA2FLAC"
set "licenseDir=%depDir%\licenses"
if not exist "%depDir%" mkdir "%depDir%"
if not exist "%licenseDir%" mkdir "%licenseDir%"

for %%F in ("vgmstream-cli.exe" "ffmpeg.exe" "ffprobe.exe" "NA2FLAC_Installer.exe" "avcodec-vgmstream-59.dll" "avformat-vgmstream-59.dll" "avutil-vgmstream-57.dll" "libatrac9.dll" "libcelt-0061.dll" "libcelt-0110.dll" "libg719_decode.dll" "libmpg123-0.dll" "libspeex-1.dll" "libvorbis.dll" "swresample-vgmstream-4.dll" "NA2FLAC_1.2.1.md" "CODE_OF_CONDUCT.md" "CONTRIBUTING.md" "README.md") do move "%%F" "%depDir%" >nul 2>&1
for %%L in ("BatToExePortable_LICENSE.md" "FFMPEG_COPYING.GPLv3.md" "FFMPEG_LICENSE.md" "VGMSTREAM_COPYING.md" "LICENSE.txt" "NSIS_COPYING.md") do move "%%L" "%licenseDir%" >nul 2>&1

set "VGM=%depDir%\vgmstream-cli.exe"
set "FFM=%depDir%\ffmpeg.exe"
set "FFP=%depDir%\ffprobe.exe"

echo =============================================
echo    Nintendo Audio to FLAC Converter v1.2.1
echo =============================================

timeout /t 1 /nobreak >nul

:: -------------------- Dependency check & Sorting licenses --------------------
set "missingDeps="

:: List all required files for vgmstream + ffmpeg + ffprobe
set requiredFiles="%depDir%\vgmstream-cli.exe" "%depDir%\ffmpeg.exe" "%depDir%\ffprobe.exe" "%depDir%\avcodec-vgmstream-59.dll" "%depDir%\avformat-vgmstream-59.dll" "%depDir%\avutil-vgmstream-57.dll" "%depDir%\libatrac9.dll" "%depDir%\libcelt-0061.dll" "%depDir%\libcelt-0110.dll" "%depDir%\libg719_decode.dll" "%depDir%\libmpg123-0.dll" "%depDir%\libspeex-1.dll" "%depDir%\libvorbis.dll" "%depDir%\swresample-vgmstream-4.dll"

for %%f in (%requiredFiles%) do (
    if not exist "%%f" set "missingDeps=!missingDeps!%%f, "
)

:: Trim trailing comma and space
if defined missingDeps set "missingDeps=!missingDeps:~0,-2!"

if defined missingDeps (
    echo Warning: The following required files are missing: !missingDeps!
    echo Please make sure all files extracted by the executable are inside the NA2FLAC folder.
    pause
    exit
)

:: -------------------- Scan prompt --------------------
:ask_scan
set /p choice=Start scan? (y/n): 
if /i "%choice%"=="y" (
    timeout /t 1 /nobreak >nul
) else if /i "%choice%"=="n" (
    echo Process cancelled by user. Closing in 5 seconds.
    timeout /t 5 /nobreak >nul
    exit
) else (
    echo Please type y or n.
    goto ask_scan
)

:: -------------------- Scan for files --------------------

echo Checking for files...
timeout /t 2 /nobreak >nul

set countAST=0
set countBRSTM=0
set countBCSTM=0
set countBFSTM=0
set countBFWAV=0
set countBWAV=0
set countSWAV=0
set countSTRM=0
set countLOPUS=0
set countIDSP=0
set countHPS=0
set countDSP=0
set countADX=0
set countMPT=0
set countOGG=0
set countCUSTOM=0

for /r %%f in (*.ast) do set /a countAST+=1
for /r %%f in (*.brstm) do set /a countBRSTM+=1
for /r %%f in (*.bcstm) do set /a countBCSTM+=1
for /r %%f in (*.bfstm) do set /a countBFSTM+=1
for /r %%f in (*.bfwav) do set /a countBFWAV+=1
for /r %%f in (*.bwav) do set /a countBWAV+=1
for /r %%f in (*.swav) do set /a countSWAV+=1
for /r %%f in (*.strm) do set /a countSTRM+=1
for /r %%f in (*.lopus) do set /a countLOPUS+=1
for /r %%f in (*.idsp) do set /a countIDSP+=1
for /r %%f in (*.hps) do set /a countHPS+=1
for /r %%f in (*.dsp) do set /a countDSP+=1
for /r %%f in (*.adx) do set /a countADX+=1
for /r %%f in (*.mp3) do set /a countMPT+=1
for /r %%f in (*.ogg) do set /a countOGG+=1
for /r %%f in (*.custom) do set /a countCUSTOM+=1
set /a totalFiles=countAST + countBRSTM + countBCSTM + countBFSTM + countBFWAV + countBWAV + countSWAV + countSTRM + countLOPUS + countIDSP + countHPS + countDSP + countADX + countMPT + countOGG + countCUSTOM

if %countAST%==0 if %countBRSTM%==0 if %countBCSTM%==0 if %countBFSTM%==0 if %countBFWAV%==0 if %countBWAV%==0 if %countSWAV%==0 if %countSTRM%==0 if %countLOPUS%==0 if %countIDSP%==0 if %countHPS%==0 if %countDSP%==0 if %countADX%==0 if %countMPT%==0 if %countOGG%==0 if %countCUSTOM%==0 (
    echo No supported files found.
    echo Please move the executable into the folder containing your audio files or subfolders with files and try again.
    echo Supported formats: AST, BRSTM, BCSTM, BFSTM, BFWAV, BWAV, SWAV, STRM, LOPUS, IDSP, HPS, DSP, ADX, MP3, OGG
    pause
    exit
)
  
if %countAST% gtr 0 echo %countAST% AST files found^^!
if %countBRSTM% gtr 0 echo %countBRSTM% BRSTM files found^^!
if %countBCSTM% gtr 0 echo %countBCSTM% BCSTM files found^^!
if %countBFSTM% gtr 0 echo %countBFSTM% BFSTM files found^^!
if %countBFWAV% gtr 0 echo %countBFWAV% BFWAV files found^^!
if %countBWAV% gtr 0 echo %countBWAV% BWAV files found^^!
if %countSWAV% gtr 0 echo %countSWAV% SWAV files found^^!
if %countSTRM% gtr 0 echo %countSTRM% STRM files found^^!
if %countLOPUS% gtr 0 echo %countLOPUS% LOPUS files found^^!
if %countIDSP% gtr 0 echo %countIDSP% IDSP files found^^!
if %countHPS% gtr 0 echo %countHPS% HPS files found^^!
if %countDSP% gtr 0 echo %countDSP% DSP files found^^!
if %countADX% gtr 0 echo %countADX% ADX files found^^!
if %countMPT% gtr 0 echo %countMPT% MP3 files found^^!
if %countOGG% gtr 0 echo %countOGG% OGG files found^^!
if %countCUSTOM% gtr 0 echo %countCUSTOM% CUSTOM files found^^!
echo ^(%totalFiles% files total^)

timeout /t 1 /nobreak >nul

:: -------------------- Conversion prompt --------------------
:ask_convert
set /p choice=Convert to FLAC? (y/n): 
if /i "%choice%"=="y" (
    echo Starting in 5 seconds...
    timeout /t 5 /nobreak >nul
) else if /i "%choice%"=="n" (
    echo Process cancelled by user. Closing in 5 seconds.
    timeout /t 5 /nobreak >nul
    exit
) else (
    echo Please type y or n.
    goto ask_convert
)

:: -------------------- Conversion with mirrored folder structure --------------------
set "sourceRoot=%cd%"
set "targetRoot=%cd%\converted"

if not exist "%targetRoot%" mkdir "%targetRoot%"

set /a converted=0
set /a failed=0
set /a wavKept=0

:: Start timer
set "startTime=%time%"
set "st=%startTime: =%"
for /f "tokens=1-3 delims=:." %%a in ("%st%") do (
    set "sh=%%a"
    set "sm=%%b"
    set "ss=%%c"
)
for %%x in (sh sm ss) do call set "%%x=%%%%x%%%"
for %%x in (sh sm ss) do if "!%%x!"=="" set "%%x=0"
set /a startSec=sh*3600 + sm*60 + ss

:: -------------------- Recursive conversion loop --------------------
for /r "%sourceRoot%" %%F in (*.ast *.brstm *.bcstm *.bfstm *.bfwav *.bwav *.swav *.strm *.lopus *.idsp *.hps *.dsp *.adx *.mp3 *.ogg *.custom) do (
    set "file=%%~nF"
    set "relPath=%%~dpF"
    set "relPath=!relPath:%sourceRoot%=!"
    
    if not exist "%targetRoot%!relPath!" mkdir "%targetRoot%!relPath!"
    
    echo Processing %%F...
    
    :: Step 1: Decode to WAV
    "%VGM%" "%%F" -o "%targetRoot%!relPath!!file!.wav"

    :: Step 2: Merge stereo _l/_r if exists
    set "merged=0"
    if "!file:~-2!"=="_l" (
        set "right=!file:_l=_r!"
        if exist "%targetRoot%!relPath!!right!.wav" (
            set "base=!file:_l=!"
            "%FFM%" -y -i "%targetRoot%!relPath!!file!.wav" -i "%targetRoot%!relPath!!right!.wav" -filter_complex "[0:a][1:a]amerge=inputs=2[a]" -map "[a]" -c:a flac "%targetRoot%!relPath!!base!.flac" >nul 2>&1
            if exist "%targetRoot%!relPath!!base!.flac" (
                del "%targetRoot%!relPath!!file!.wav" "%targetRoot%!relPath!!right!.wav"
                set /a converted+=1
                set "merged=1"
            ) else (
                set /a wavKept+=1
            )
        )
    )

    if !merged! == 0 (
        :: Step 3: Detect number of channels
        set "channels=0"
        for /f "usebackq tokens=* delims=" %%c in (`"%FFP%" -v error -select_streams a:0 -show_entries stream^=channels -of default^=noprint_wrappers^=1:nokey^=1 "%targetRoot%!relPath!!file!.wav" 2^>nul`) do (
            set "channels=%%c"
        )
        if "!channels!"=="" set "channels=0"

        :: Step 4: Convert if <= 8 channels
        if !channels! leq 8 (
            "%FFM%" -y -i "%targetRoot%!relPath!!file!.wav" -c:a flac "%targetRoot%!relPath!!file!.flac" >nul 2>&1
            if exist "%targetRoot%!relPath!!file!.flac" (
                del "%targetRoot%!relPath!!file!.wav"
                set /a converted+=1
            ) else (
                echo Conversion failed for "%targetRoot%!relPath!!file!.wav"
                set /a failed+=1
            )
        ) else (
            echo Keeping "%targetRoot%!relPath!!file!.wav" because it has !channels! channels
            set /a wavKept+=1
        )
    )
)

:: -------------------- End timer --------------------
set "endTime=%time%"
set "et=%endTime: =%"
for /f "tokens=1-3 delims=:." %%a in ("%et%") do (
    set "eh=%%a"
    set "em=%%b"
    set "es=%%c"
)
for %%x in (eh em es) do call set "%%x=%%%%x%%%"
for %%x in (eh em es) do if "!%%x!"=="" set "%%x=0"
set /a endSec=eh*3600 + em*60 + es
if %endSec% lss %startSec% set /a endSec+=86400
set /a totalSec=endSec-startSec
set /a mins=totalSec/60
set /a secs=totalSec%%60

:: -------------------- Summary & End --------------------
set "displayFailed=!failed!"
set "dspWavKept=!wavKept!"
if !failed! lss 1 set "displayFailed=None"
if !wavKept! lss 1 set "dspWavKept=None"

echo.
echo =======================================
echo Conversion Summary
echo =======================================
echo Files converted (FLAC): %converted%
echo Files kept as WAV (too many channels or merge failed): %dspWavKept%
echo Failed to convert: %displayFailed%
echo Total time: %mins%m %secs%s
echo =======================================
echo.

pause