; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Nickvision Denaro"
#define MyAppShortName "Denaro"
#define MyAppVersion "2024.4.0"
#define MyAppPublisher "Nickvision"
#define MyAppURL "https://nickvision.org"
#define MyAppExeName "org.nickvision.money.winui.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6B35090B-5CC0-464A-8B1F-64D115E4C031}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
UsePreviousAppDir=no
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\COPYING
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=..\inno
OutputBaseFilename=NickvisionDenaroSetup
SetupIconFile=..\resources\org.nickvision.money.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
DirExistsWarning=no
CloseApplications=force
ChangesEnvironment=yes
AlwaysRestart=yes

[Code]
procedure SetupVC();
var
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\deps\VC_redist.x64.exe'), '/install /quiet /norestart', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
  then
    MsgBox('Unable to install VC . Please try again', mbError, MB_OK);
end;

procedure SetupWinAppSDK();
var
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\deps\WindowsAppRuntimeInstall-x64.exe'), '--quiet', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
  then
    MsgBox('Unable to install Windows App SDK. Please try again', mbError, MB_OK);
end;

procedure SetupGnuPlot();
var
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\deps\gnuplot.exe'), '/verysilent /norestart /allusers', '', SW_HIDE, ewWaitUntilTerminated, ResultCode)
  then
    MsgBox('Unable to install gnuplot. Please try again', mbError, MB_OK);
end;

function NeedsAddPath(Param: string): boolean;
var
  OrigPath: string;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
    'Path', OrigPath)
  then begin
    Result := True;
    exit;
  end;
  { look for the path with leading and trailing semicolon }
  { Pos() returns 0 if not found }
  Result := Pos(';' + Param + ';', ';' + OrigPath + ';') = 0;
end;

[Registry]
Root: HKLM; Subkey: "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"; \
    ValueType: expandsz; ValueName: "Path"; ValueData: "{olddata};C:\Program Files\gnuplot\bin"; \
    Check: NeedsAddPath('C:\Program Files\gnuplot\bin')

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\..\VC_redist.x64.exe"; DestDir: "{app}\deps"; AfterInstall: SetupVC  
Source: "..\..\WindowsAppRuntimeInstall-x64.exe"; DestDir: "{app}\deps"; AfterInstall: SetupWinAppSDK  
Source: "..\..\gnuplot.exe"; DestDir: "{app}\deps"; AfterInstall: SetupGnuPlot
Source: "..\build\org.nickvision.money.winui\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion 
Source: "..\build\org.nickvision.money.winui\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppShortName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppShortName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

