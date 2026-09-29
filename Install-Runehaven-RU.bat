@echo off
setlocal EnableExtensions
title Runehaven Russian Translation Installer

set "SCRIPT_DIR=%~dp0"
set "DEFAULT_GAME_PATH=E:\SteamLibrary\steamapps\common\Runehaven"

echo.
echo   Runehaven Russian Translation Installer
echo   ======================================
echo.
echo This installer copies the Russian localization to your Runehaven folder.
echo Close Runehaven before continuing.
echo.

if exist "%DEFAULT_GAME_PATH%\Runehaven.exe" (
    set "GAME_PATH=%DEFAULT_GAME_PATH%"
    echo Runehaven was found here:
    echo %GAME_PATH%
    echo.
    set /p "USE_DEFAULT=Use this folder? [Y/n]: "
    if /I not "%USE_DEFAULT%"=="n" goto :install
)

:ask_path
echo.
set "GAME_PATH="
set /p "GAME_PATH=Paste the full path to the Runehaven folder: "
if not defined GAME_PATH (
    echo A folder path is required.
    goto :ask_path
)

:install
if not exist "%GAME_PATH%\Runehaven.exe" (
    echo.
    echo Runehaven.exe was not found in:
    echo %GAME_PATH%
    echo Please check the folder and try again.
    goto :ask_path
)

echo.
echo Installing Russian translation...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%SCRIPT_DIR%Install-Runehaven-RU.ps1" -GamePath "%GAME_PATH%"
if errorlevel 1 (
    echo.
    echo Installation failed. Read the message above and try again.
    pause
    exit /b 1
)

echo.
echo Installation complete. You can now start Runehaven.
pause
exit /b 0
