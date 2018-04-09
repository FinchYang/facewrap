; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!
#define MyAppVersion "1.0.0.3"
#define sourcedir "I:\faceoutput\desktop"
[Setup]
AppName=FaceDesktop
AppVersion={#MyAppVersion}
DefaultDirName={pf}\FaceDesktop
DefaultGroupName=FaceDesktop
UninstallDisplayIcon={app}\FaceDesktop.exe
Compression=lzma2
SolidCompression=yes
OutputDir=userdocs:FaceDesktop Output
OutputBaseFilename=FaceDesktop{#MyAppVersion}
[Files]
;Source: "FaceDesktop.exe"; DestDir: "{app}"
Source: "{#sourcedir}\Readme.txt"; DestDir: "{app}"; Flags: isreadme
Source: "{#sourcedir}\compare\*"; DestDir: "{app}\compare"; Flags: ignoreversion 
Source: "{#sourcedir}\zh-cn\*"; DestDir: "{app}\zh-cn"; Flags: ignoreversion 
Source: "{#sourcedir}\x86\*"; DestDir: "{app}\x86"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\asset\*"; DestDir: "{app}\compare\asset"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\asset\models\*"; DestDir: "{app}\compare\asset\models"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\iconengines\*"; DestDir: "{app}\compare\iconengines"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\imageformats\*"; DestDir: "{app}\compare\imageformats"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\platforms\*"; DestDir: "{app}\compare\platforms"; Flags: ignoreversion 
Source: "{#sourcedir}\compare\translations\*"; DestDir: "{app}\compare\translations"; Flags: ignoreversion 
Source: "{#sourcedir}\idr210sdk\*"; DestDir: "{app}\idr210sdk"; Flags: ignoreversion
Source: "{#sourcedir}\*"; DestDir: "{app}"; Flags: ignoreversion
[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkablealone
[Icons]
Name: "{group}\FaceDesktop"; Filename: "{app}\FaceDesktop.exe"
Name: "{userdesktop}\FaceDesktop";Filename: "{app}\FaceDesktop.EXE";WorkingDir: "{app}";Comment:"快速人证合一比对！"