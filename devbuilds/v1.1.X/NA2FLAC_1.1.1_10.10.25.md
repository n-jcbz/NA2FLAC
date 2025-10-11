@echo off
setlocal enabledelayedexpansion
cd /d "%~dp0%"

:: BUILD TYPE: STABLE

:: ANYTHING REFERRED TO AS "CUSTOM" AND "custom" ARE PLACEHOLDERS FOR FUTURE FORMAT SUPPORT.
:: YOU CAN REPLACE THEM WITH ANY PROPRIETARY AUDIO FORMAT TO TEST COMPATIBILITY.
:: KEEP IN MIND CASE-SENSITIVITY! REPLACE "CUSTOM" AND "custom" ACCORDINGLY.
:: IF YOU'RE CONTRIBUTING TO FORMAT SUPPORT: PLEASE RE-ADD THE CUSTOM PARTS UNDER/AFTER THE NEW FORMAT.

:: tools
set VGM=vgmstream-cli.exe
set FFM=ffmpeg.exe
set FFP=ffprobe.exe

echo ============================================
echo    Nintendo Audio to FLAC Converter v1.1
echo ============================================

timeout /t 1 /nobreak >nul

:: -------------------- Dependency Check --------------------
set "missingDeps="

:: List all required files for vgmstream + ffmpeg + ffprobe
set "requiredFiles=vgmstream-cli.exe ffmpeg.exe ffprobe.exe avcodec-vgmstream-59.dll avformat-vgmstream-59.dll avutil-vgmstream-57.dll libatrac9.dll libcelt-0061.dll libcelt-0110.dll libg719_decode.dll libmpg123-0.dll libspeex-1.dll swresample-vgmstream-4.dll"

for %%f in (%requiredFiles%) do (
    if not exist "%%f" set "missingDeps=!missingDeps!%%f, "
)

:: Trim trailing comma and space
if defined missingDeps set "missingDeps=!missingDeps:~0,-2!"

if defined missingDeps (
    echo Warning: The following required files are missing: !missingDeps!
    echo Please make sure all files extracted by the exe are in the same folder.
    pause
    exit
)

:: -------------------- Prompt --------------------
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

:: Check for FLAC files
dir /b *.flac >nul 2>&1
if %errorlevel% equ 0 goto ask_sortFlac
goto flac_skip

:ask_sortFlac
set /p sortFlac="FLAC files detected. Sort them first? (y/n): "
if /i "%sortFlac%"=="y" goto sortFlac
goto continue_workflow

:sortFlac
set "flacdest=FLAC_Tracks"
if not exist "%flacdest%" mkdir "%flacdest%"

:: Count FLAC files before moving
set flacLeft=0
for %%f in (*.flac) do set /a flacLeft+=1

if !flacLeft! equ 0 (
    echo No FLAC files to move.
) else (
    move *.flac "%flacdest%" >nul 2>&1
    timeout /t 1 /nobreak >nul
    echo FLAC files moved to %flacdest%.
)

:: Instead of exit, just go back to workflow
goto continue_workflow

:continue_workflow
timeout /t 1 /nobreak >nul
echo Continuing with normal scan/conversion...

timeout /t 1 /nobreak >nul

:flac_skip

set countAST=0
set countBRSTM=0
set countBCSTM=0
set countBFSTM=0
set countBFWAV=0
set countBWAV=0
set countSTRM=0
set countLOPUS=0
set countIDSP=0
set countHPS=0
set countDSP=0
set countADX=0
set countMPT=0
set countCUSTOM=0

for %%f in (*.ast) do set /a countAST+=1
for %%f in (*.brstm) do set /a countBRSTM+=1
for %%f in (*.bcstm) do set /a countBCSTM+=1
for %%f in (*.bfstm) do set /a countBFSTM+=1
for %%f in (*.bfwav) do set /a countBFWAV+=1
for %%f in (*.bwav) do set /a countBWAV+=1
for %%f in (*.strm) do set /a countSTRM+=1
for %%f in (*.lopus) do set /a countLOPUS+=1
for %%f in (*.idsp) do set /a countIDSP+=1
for %%f in (*.hps) do set /a countHPS+=1
for %%f in (*.dsp) do set /a countDSP+=1
for %%f in (*.adx) do set /a countADX+=1
for %%f in (*.mp3) do set /a countMPT+=1
for %%f in (*.custom) do set /a countCUSTOM+=1
set /a totalFiles=countAST + countBRSTM + countBCSTM + countBFSTM + countBFWAV + countBWAV + countSTRM + countLOPUS + countIDSP + countHPS + countDSP + countADX + countMPT + countCUSTOM

if %countAST%==0 if %countBRSTM%==0 if %countBCSTM%==0 if %countBFSTM%==0 if %countBFWAV%==0 if %countBWAV%==0 if %countSTRM%==0 if %countLOPUS%==0 if %countIDSP%==0 if %countHPS%==0 if %countDSP%==0 if %countADX%==0 if %countCUSTOM%==0 (
    echo No supported files found in this folder.
    echo Please move the executable into the folder containing your audio files and try again.
    echo Supported formats: AST, BRSTM, BCSTM, BFSTM, BFWAV, BWAV, STRM, LOPUS, IDSP, HPS, DSP, ADX, MP3
    pause
    exit
)
  
if %countAST% gtr 0 echo %countAST% AST files found^^!
if %countBRSTM% gtr 0 echo %countBRSTM% BRSTM files found^^!
if %countBCSTM% gtr 0 echo %countBCSTM% BCSTM files found^^!
if %countBFSTM% gtr 0 echo %countBFSTM% BFSTM files found^^!
if %countBFWAV% gtr 0 echo %countBFWAV% BFWAV files found^^!
if %countBWAV% gtr 0 echo %countBWAV% BWAV files found^^!
if %countSTRM% gtr 0 echo %countSTRM% STRM files found^^!
if %countLOPUS% gtr 0 echo %countLOPUS% LOPUS files found^^!
if %countIDSP% gtr 0 echo %countIDSP% IDSP files found^^!
if %countHPS% gtr 0 echo %countHPS% HPS files found^^!
if %countDSP% gtr 0 echo %countDSP% DSP files found^^!
if %countADX% gtr 0 echo %countADX% ADX files found^^!
if %countMPT% gtr 0 echo %countMPT% MP3 files found^^!
if %countCUSTOM% gtr 0 echo %countCUSTOM% CUSTOM files found^^!
echo ^(%totalFiles% files total^)

timeout /t 1 /nobreak >nul

:: -------------------- Prompt --------------------
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

:: -------------------- Conversion (AST / BRSTM / BCSTM / BFSTM / BFWAV / BWAV / STRM / LOPUS / IDSP / HPS / DSP / ADX / MP3 / CUSTOM) --------------------

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

:: Conversion loop
for %%f in (*.ast *.brstm *.bcstm *.bfstm *.bfwav *.bwav *.strm *.lopus *.idsp *.hps *.dsp *.adx *.mp3 *.custom) do (
    set "file=%%~nf"
    echo Processing %%f...

    :: Step 1: Decode to WAV
    "%VGM%" "%%f" -o "!file!.wav"

    :: Step 2: Merge stereo _l/_r if exists
    set "merged=0"
    if "!file:~-2!"=="_l" (
        set "right=!file:_l=_r!"
        if exist "!right!.wav" (
            set "base=!file:_l=!"
            "%FFM%" -y -i "!file!.wav" -i "!right!.wav" -filter_complex "[0:a][1:a]amerge=inputs=2[a]" -map "[a]" -c:a flac "!base!.flac" >nul 2>&1
            if exist "!base!.flac" (
                del "!file!.wav" "!right!.wav"
                set /a converted+=1
                set "merged=1"
            ) else (
                set /a wavKept+=1
            )
        )
    )

    if !merged! == 0 (
        :: Step 3: Detect number of channels reliably
	set "channels=0"
	for /f "usebackq tokens=* delims=" %%c in (`%FFP% -v error -select_streams a:0 -show_entries stream^=channels -of default^=noprint_wrappers^=1:nokey^=1 "!file!.wav"`) do (
	    set "channels=%%c"
	)
	:: Force channels to a number in case it's empty
	if "!channels!"=="" set "channels=0"

	:: Step 4: Convert if <= 8 channels, else keep WAV
	if !channels! leq 8 (
  	  "%FFM%" -y -i "!file!.wav" -c:a flac "!file!.flac" >nul 2>&1
   	 if exist "!file!.flac" (
      	  del "!file!.wav"
      	  set /a converted+=1
   	 ) else (
   	     echo Conversion failed for "!file!.wav"
   	     set /a failed+=1
   	 )
	) else (
	  echo.
  	  echo Keeping "!file!.wav" because it has !channels! channels
	  echo.
   	  set /a wavKept+=1
	)
    )
)

:: End timer
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

:: Midnight rollover
if %endSec% lss %startSec% set /a endSec+=86400

:: Final calculation
set /a totalSec=endSec-startSec
set /a mins=totalSec/60
set /a secs=totalSec%%60

:: Display
set "displayFailed=!failed!"
if !failed! lss 1 set "displayFailed=None"

echo.
echo =======================================
echo Conversion Summary
echo =======================================
echo Files converted (FLAC): %converted%
echo Files kept as WAV (too many channels or merge failed): %wavKept%
echo Failed to convert: %displayFailed%
echo Total time: %mins%m %secs%s
echo =======================================
echo.

:: delay
timeout /t 1 /nobreak >nul

:ask_sort
set /p choice=Sort files? Leftover WAV files will be sorted into the same folder as FLAC files. (y/n): 
if /i "%choice%"=="y" (
	echo Sorting...
	timeout /t 1 /nobreak >nul
) else if /i "%choice%"=="n" (
	echo Process cancelled by user. This window will close automatically in 5 seconds.
	timeout /t 5 /nobreak >nul
	exit
) else (
	echo Please type y or n.
	goto ask_sort
)

:: ------------------- Sorting --------------------

:sorting
set "flacdest=FLAC_Tracks"
if not exist "%flacdest%" mkdir "%flacdest%"
move *.flac "%flacdest%" >nul 2>&1
move *.wav "%flacdest%" >nul 2>&1

if %countAST% gtr 0 (
    if not exist "AST_Tracks" mkdir "AST_Tracks"
    move *.ast "AST_Tracks" >nul 2>&1
)

if %countBRSTM% gtr 0 (
    if not exist "BRSTM_Tracks" mkdir "BRSTM_Tracks"
    move *.brstm "BRSTM_Tracks" >nul 2>&1
)

if %countBCSTM% gtr 0 (
    if not exist "BCSTM_Tracks" mkdir "BCSTM_Tracks"
    move *.bcstm "BCSTM_Tracks" >nul 2>&1
)

if %countBFSTM% gtr 0 (
    if not exist "BFSTM_Tracks" mkdir "BFSTM_Tracks"
    move *.bfstm "BFSTM_Tracks" >nul 2>&1
)

if %countBFWAV% gtr 0 (
    if not exist "BFWAV_Tracks" mkdir "BFWAV_Tracks"
    move *.bfwav "BFWAV_Tracks" >nul 2>&1
)

if %countBWAV% gtr 0 (
    if not exist "BWAV_Tracks" mkdir "BWAV_Tracks"
    move *.bwav "BWAV_Tracks" >nul 2>&1
)

if %countSTRM% gtr 0 (
    if not exist "STRM_Tracks" mkdir "STRM_Tracks"
    move *.strm "STRM_Tracks" >nul 2>&1
)

if %countLOPUS% gtr 0 (
    if not exist "LOPUS_Tracks" mkdir "LOPUS_Tracks"
    move *.lopus "LOPUS_Tracks" >nul 2>&1
)

if %countIDSP% gtr 0 (
    if not exist "IDSP_Tracks" mkdir "IDSP_Tracks"
    move *.idsp "IDSP_Tracks" >nul 2>&1
)

if %countHPS% gtr 0 (
    if not exist "HPS_Tracks" mkdir "HPS_Tracks"
    move *.hps "HPS_Tracks" >nul 2>&1
)

if %countDSP% gtr 0 (
    if not exist "DSP_Tracks" mkdir "DSP_Tracks"
    move *.dsp "DSP_Tracks" >nul 2>&1
)

if %countADX% gtr 0 (
    if not exist "ADX_Tracks" mkdir "ADX_Tracks"
    move *.adx "ADX_Tracks" >nul 2>&1
)

if %countMPT% gtr 0 (
    if not exist "MP3_Tracks" mkdir "MP3_Tracks"
    move *.mp3 "MP3_Tracks" >nul 2>&1
)

if %countCUSTOM% gtr 0 (
    if not exist "CUSTOM_Tracks" mkdir "CUSTOM_Tracks"
    move *.custom "CUSTOM_Tracks" >nul 2>&1
)

:: check files
set flacLeft=0
:: set wavLeft=0
set astLeft=0
set brstmLeft=0
set bcstmLeft=0
set bfstmLeft=0
set bfwavLeft=0
set bwavLeft=0
set strmLeft=0
set lopusLeft=0
set idspLeft=0
set hpsLeft=0
set dspLeft=0
set adxLeft=0
set mptLeft=0
set customLeft=0

for %%f in (*.flac) do set /a flacLeft+=1
:: for %%f in (*.wav) do set /a wavLeft+=1
for %%f in (*.ast) do set /a astLeft+=1
for %%f in (*.brstm) do set /a brstmLeft+=1
for %%f in (*.bcstm) do set /a bcstmLeft+=1
for %%f in (*.bfstm) do set /a bfstmLeft+=1
for %%f in (*.bfwav) do set /a bfwavLeft+=1
for %%f in (*.bwav) do set /a bwavLeft+=1
for %%f in (*.strm) do set /a strmLeft+=1
for %%f in (*.lopus) do set /a lopusLeft+=1
for %%f in (*.idsp) do set /a idspLeft+=1
for %%f in (*.hps) do set /a hpsLeft+=1
for %%f in (*.dsp) do set /a dspLeft+=1
for %%f in (*.adx) do set /a adxLeft+=1
for %%f in (*.mp3) do set /a mptLeft+=1
for %%f in (*.custom) do set /a customLeft+=1

echo.
:: Only check if there were files of that type originally
if %countAST% gtr 0 (
    if !astLeft! equ 0 (
        echo All original AST files moved.
    ) else (
        echo Couldn't move all original AST files.
    )
)

if %countBRSTM% gtr 0 (
    if !brstmLeft! equ 0 (
        echo All original BRSTM files moved.
    ) else (
        echo Couldn't move all original BRSTM files.
    )
)

if %countBCSTM% gtr 0 (
    if !bcstmLeft! equ 0 (
        echo All original BCSTM files moved.
    ) else (
        echo Couldn't move all original BCSTM files.
    )
)

if %countBFSTM% gtr 0 (
    if !bfstmLeft! equ 0 (
        echo All original BFSTM files moved.
    ) else (
        echo Couldn't move all original BFSTM files.
    )
)

if %countBFWAV% gtr 0 (
    if !bfwavLeft! equ 0 (
        echo All original BFWAV files moved.
    ) else (
        echo Couldn't move all original BFWAV files.
    )
)

if %countBWAV% gtr 0 (
    if !bwavLeft! equ 0 (
        echo All original BWAV files moved.
    ) else (
        echo Couldn't move all original BWAV files.
    )
)

if %countSTRM% gtr 0 (
    if !strmLeft! equ 0 (
	echo All original STRM files moved.
    ) else (
	echo Couldn't move all original STRM files.
    )
)

if %countLOPUS% gtr 0 (
    if !lopusLeft! equ 0 (
        echo All original LOPUS files moved.
    ) else (
        echo Couldn't move all original LOPUS files.
    )
)

if %countIDSP% gtr 0 (
    if !idspLeft! equ 0 (
        echo All original IDSP files moved.
    ) else (
        echo Couldn't move all original IDSP files.
    )
)

if %countHPS% gtr 0 (
    if !hpsLeft! equ 0 (
        echo All original HPS files moved.
    ) else (
        echo Couldn't move all original HPS files.
    )
)

if %countDSP% gtr 0 (
    if !dspLeft! equ 0 (
        echo All original DSP files moved.
    ) else (
        echo Couldn't move all original DSP files.
    )
)

if %countADX% gtr 0 (
    if !adxLeft! equ 0 (
	echo All original ADX files moved.
    ) else (
	echo Couldn't move all original ADX files.
    )
)

if %countMPT% gtr 0 (
    if !mptLeft! equ 0 (
	echo All original MP3 files moved.
    ) else (
	echo Couldn't move all original MP3 files.
    )
)

if %countCUSTOM% gtr 0 (
    if !customLeft! equ 0 (
	echo All original CUSTOM files moved.
    ) else (
	echo Couldn't move all original CUSTOM files.
    )
)

if !flacLeft! equ 0 (
    echo All FLAC/WAV files moved.
) else (
        echo Couldn't move all FLAC/WAV files.
    )
)

::  if !wavLeft! equ 0 (
::    echo All WAV files moved into "%flacdest%".
:: ) else (
::         echo Couldn't move all WAV files.
::    )
:: )

timeout /t 1 /nobreak >nul

echo Sorting complete.

timeout /t 1 /nobreak >nul

pause
