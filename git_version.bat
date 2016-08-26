@echo off

set GIT_COMMAND_PATH="C:\Program Files\Git\cmd\"
set PATH=%PATH%;%GIT_COMMAND_PATH%

git.exe --version
IF NOT %ERRORLEVEL% EQU 0 (
echo "ERROR!!! Cannot find git.exe, set GIT version to default 0(0000000)" 
set GIT_VERSION_COMMIT=0
set GIT_VERSION=0000000
goto end)

for /f "delims=" %%t in ('git.exe rev-list HEAD  ^| find /v /c ""') do set GIT_VERSION_COMMIT=%%t
echo "GIT_VERSION_COMMIT:" %GIT_VERSION_COMMIT%

for /f "delims=" %%t in ('git.exe rev-list HEAD -n 1') do set GIT_VERSION=%%t
set GIT_VERSION=%GIT_VERSION:~0,7%
echo "GIT_VERSION:" %GIT_VERSION%

echo "Generate git version done!"

:end
