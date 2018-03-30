; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!
#define MyAppVersion "1.0.0.0"

[Setup]
AppName=FaceDesktop
AppVersion={#MyAppVersion}
DefaultDirName={pf}\FaceDesktop
DefaultGroupName=FaceDesktop
UninstallDisplayIcon={app}\FaceDesktop.exe
Compression=lzma2
SolidCompression=yes
OutputDir=userdocs:FaceDesktop Output
OutputBaseFilename=NoticeSetup{#MyAppVersion}
[Files]
;Source: "FaceDesktop.exe"; DestDir: "{app}"
Source: "Readme.txt"; DestDir: "{app}"; Flags: isreadme
Source: "compare\*"; DestDir: "{app}\compare"; Flags: ignoreversion 
Source: "zh-cn\*"; DestDir: "{app}\zh-cn"; Flags: ignoreversion 
Source: "x86\*"; DestDir: "{app}\x86"; Flags: ignoreversion 
Source: "compare\asset\*"; DestDir: "{app}\compare\asset"; Flags: ignoreversion 
Source: "compare\iconengines\*"; DestDir: "{app}\compare\iconengines"; Flags: ignoreversion 
Source: "compare\imageformats\*"; DestDir: "{app}\compare\imageformats"; Flags: ignoreversion 
Source: "compare\platforms\*"; DestDir: "{app}\compare\platforms"; Flags: ignoreversion 
Source: "compare\translations\*"; DestDir: "{app}\compare\translations"; Flags: ignoreversion 
Source: "idr210sdk\*"; DestDir: "{app}\idr210sdk"; Flags: ignoreversion
Source: "*"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\FaceDesktop"; Filename: "{app}\FaceDesktop.exe"
