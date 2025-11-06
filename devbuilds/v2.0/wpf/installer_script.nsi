; ===============================================
; NSIS Installer for NA2FLAC v2.0 WPF
; Combines modern Program Files setup with Legacy MUI + file list
; ===============================================

!include "MUI2.nsh"

;--------------------------------
; Installer properties
;--------------------------------
Name "NA2FLAC v2.0"
OutFile "NA2FLAC_v2.0_Installer.exe"
InstallDir "$PROGRAMFILES\NA2FLAC 2.0"
InstallDirRegKey HKLM "Software\NA2FLAC" "Install_Dir"
RequestExecutionLevel admin

;--------------------------------
; Modern UI
;--------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON "icon.ico"
!define MUI_UNICON "icon.ico"
!define MUI_WELCOMEPAGE_TEXT "Install NA2FLAC?"
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

; Finish page with run checkbox
!define MUI_FINISHPAGE_RUN "$INSTDIR\NA2FLAC v2.0.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Run NA2FLAC after installation"
!insertmacro MUI_PAGE_FINISH

;--------------------------------
; Section: Main Install
;--------------------------------
Section "Install"

  SetOutPath "$INSTDIR"
  ; Create dependencies folders
  CreateDirectory "$INSTDIR"
  CreateDirectory "$INSTDIR\dependencies"
  CreateDirectory "$INSTDIR\dependencies\licenses"

  ;--------------------------
  ; Copy all executables / dependencies
  ;--------------------------
  File "NA2FLAC v2.0.exe"

  ; Main files
  File "NA2FLAC v2.0.runtimeconfig.json"
  File "NA2FLAC v2.0.deps.json"
  File "NA2FLAC v2.0.pdb"
  File "NA2FLAC v2.0.dll"
  File "C#.txt"
  File "XAML.txt"
  File "CODE_OF_CONDUCT.md"
  File "CONTRIBUTING.md"
  File "README.md"

  ; Dependencies
  SetOutPath "$INSTDIR\dependencies"
  File "vgmstream-cli.exe"
  File "ffmpeg.exe"
  File "ffprobe.exe"
  File "avcodec-vgmstream-59.dll"
  File "avformat-vgmstream-59.dll"
  File "avutil-vgmstream-57.dll"
  File "libatrac9.dll"
  File "libcelt-0061.dll"
  File "libcelt-0110.dll"
  File "libg719_decode.dll"
  File "libmpg123-0.dll"
  File "libspeex-1.dll"
  File "libvorbis.dll"
  File "swresample-vgmstream-4.dll"

  ; Licenses
  SetOutPath "$INSTDIR\licenses"
  File "NSIS_COPYING.md"
  File "FFMPEG_COPYING.GPLv3.md"
  File "FFMPEG_LICENSE.md"
  File "VGMSTREAM_COPYING.md"
  File "LICENSE.txt"

  ;--------------------------
  ; Create desktop shortcut
  ;--------------------------
  CreateShortcut "$DESKTOP\NA2FLAC 2.0.lnk" "$INSTDIR\NA2FLAC v2.0.exe"
  CreateShortCut "$SMPROGRAMS\NA2FLAC\NA2FLAC v2.0.lnk" "$INSTDIR\NA2FLAC v2.0.exe"

  ;--------------------------
  ; Write uninstaller
  ;--------------------------
  WriteUninstaller "$INSTDIR\Uninstall.exe"

  ;--------------------------
  ; Optional registry keys (for uninstall info)
  ;--------------------------
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "DisplayName" "NA2FLAC 2.0"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "Publisher" "n.jcbz"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "DisplayVersion" "2.0"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC" "NoRepair" 1

SectionEnd

;--------------------------------
; Section: Uninstall
;--------------------------------
Section "Uninstall"

  ; Remove desktop shortcut
  Delete "$DESKTOP\NA2FLAC 2.0.lnk"

  ; Remove installation folder
  RMDir /r "$INSTDIR"

  ; Remove uninstall registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NA2FLAC"
  DeleteRegKey HKLM "Software\NA2FLAC"

SectionEnd
