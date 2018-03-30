; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!
#define MyAppVersion "1.0.0.11"
#define app1 "c:\ccompare"
[Setup]
AppName=FaceWinSeven
AppVersion={#MyAppVersion}
DefaultDirName={pf}\{#app1}
DefaultGroupName=FaceWinSeven

Compression=lzma2
SolidCompression=yes
OutputDir=userdocs:FaceWinSeven Output
OutputBaseFilename=FaceWinSeven{#MyAppVersion}
[Files]
Source: "{#app1}\asset\*"; DestDir: "{#app1}\asset"; Flags: ignoreversion 
Source: "{#app1}\asset\models\*"; DestDir: "{#app1}\asset\models"; Flags: ignoreversion 
Source: "{#app1}\*"; DestDir: "{#app1}"; Flags: ignoreversion
[Run]
Filename: "{#app1}\unreg.bat"
Filename: "{#app1}\reg.bat"
