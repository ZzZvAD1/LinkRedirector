@Echo Off

set currentLocation=%~dp0
set currentLocation=%currentLocation:~0,-1%

cd %systemroot%\system32
call :IsAdmin

reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /t REG_EXPAND_SZ /v PATH /d "%path%;%currentLocation%" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector" /ve /t REG_SZ /d "Link Redirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities" /v "ApplicationName" /t REG_SZ /d "Link Redirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities" /v "ApplicationIcon" /t REG_SZ /d "%currentLocation%\LinkRedirector.exe,0" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities" /v "ApplicationDescription" /t REG_SZ /d "Link Redirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities\StartMenu" /v "StartMenuInternet" /t REG_SZ /d "Link Redirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities\URLAssociations" /v "https" /t REG_SZ /d "LinkRedirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities\URLAssociations" /v "http" /t REG_SZ /d "LinkRedirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\Capabilities\URLAssociations" /v "ftp" /t REG_SZ /d "LinkRedirector" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\DefaultIcon" /ve /t REG_SZ /d "%currentLocation%\LinkRedirector.exe,0" /f >nul
reg add "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector\shell\open\command" /ve /t REG_SZ /d "\"%currentLocation%\LinkRedirector.exe\"" /f >nul
reg add "HKLM\SOFTWARE\RegisteredApplications" /v "Link Redirector" /t REG_SZ /d "Software\Clients\StartMenuInternet\Link Redirector\Capabilities" /f >nul
reg add "HKLM\SOFTWARE\Classes\LinkRedirector" /v "URL Protocol" /t REG_SZ /d "" /f >nul
reg add "HKLM\SOFTWARE\Classes\LinkRedirector" /ve /t REG_SZ /d "Link Redirector" /f >nul
reg add "HKLM\SOFTWARE\Classes\LinkRedirector\DefaultIcon" /ve /t REG_SZ /d "%currentLocation%\LinkRedirector.exe,0" /f >nul
reg add "HKLM\SOFTWARE\Classes\LinkRedirector\shell\open\command" /ve /t REG_SZ /d "\"%currentLocation%\LinkRedirector.exe\" \"%%1\"" /f >nul

echo Setup complete!
pause
Exit

:IsAdmin
reg query "HKU\S-1-5-19\Environment"
If Not %ERRORLEVEL% EQU 0 (
 Cls & Echo You must have administrator rights to continue ... 
 Pause & Exit
)
Cls
goto:eof
