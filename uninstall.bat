@Echo Off
SETLOCAL ENABLEDELAYEDEXPANSION

cd %systemroot%\system32
call :IsAdmin

reg delete "HKLM\SOFTWARE\Clients\StartMenuInternet\Link Redirector" /f >nul
reg delete "HKLM\SOFTWARE\Classes\LinkRedirector" /f >nul
reg delete "HKLM\SOFTWARE\RegisteredApplications" /v "Link Redirector" /f >nul

set $path=
for %%i in ("%path:;=","%") do (
	set path_part=%%i
	echo !path_part! | findstr /r "\\Link Redirector$" >nul || set $path=!$path!;!path_part:~1,-1!
)
set $path=!$path:;;=;!
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /t REG_EXPAND_SZ /v PATH /d "!$path:~1!" /f >nul

echo Unistall complete!
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
