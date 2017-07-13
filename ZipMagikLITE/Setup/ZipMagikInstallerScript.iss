; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ZipMagikLITE"
#define MyAppVersion "1.0"
#define MyAppPublisher "Stuart Harrison"
#define MyAppURL "http://www.stuart-harrison.com"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{7F9C1D87-1420-4210-893D-5A0DAB06CCFE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\Users\Stuart Harrison\Desktop\ZipMagikLicence.txt
OutputDir=C:\Users\Stuart Harrison\Desktop
OutputBaseFilename=ZipMagikLITE_v1_setup
SetupIconFile=C:\dev-stuart\_Additional\ZipMagikLITE\ZipMagikLITE\Content\app_icon.ico
Compression=lzma
SolidCompression=yes
UninstallDisplayIcon={app}\app_icon.ico
UninstallDisplayName={#MyAppName} {#MyAppVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "C:\dev-stuart\_Libraries\srm\srm.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\dev-stuart\_Git\ZipMagikLITE\ZipMagikLITE\bin\Debug\ZipMagikLITE.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\dev-stuart\_Git\ZipMagikLITE\ZipMagikLITE\bin\Debug\SharpShell.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\dev-stuart\_Git\ZipMagikLITE\ZipMagikLITE\bin\Debug\Ionic.Zip.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\dev-stuart\_Git\ZipMagikLITE\ZipMagikLITE\bin\Debug\ZipMagikLicence.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\dev-stuart\_Git\ZipMagikLITE\ZipMagikLITE\Content\app_icon.ico"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\srm.exe"; Parameters: """install"" ""{app}\ZipMagikLITE.dll"" ""-codebase"""

[UninstallRun]
Filename: "{app}\srm.exe"; Parameters: """uninstall"" ""{app}\ZipMagikLITE.dll"""

