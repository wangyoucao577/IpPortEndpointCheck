@echo off

set VS_14_EXE_PATH="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.com"

%VS_14_EXE_PATH% IpPortEndpointCheck.sln /rebuild "Debug|Any CPU" 
echo "ERRORLEVEL:"%ERRORLEVEL%
IF NOT %ERRORLEVEL% EQU 0 (
echo "ERROR!!! Build failed here!" 
goto end)

%VS_14_EXE_PATH% IpPortEndpointCheck.sln /rebuild "Release|Any CPU" 
echo "ERRORLEVEL:"%ERRORLEVEL%
IF NOT %ERRORLEVEL% EQU 0 (
echo "ERROR!!! Build failed here!" 
goto end)

echo "Build all succeed!"

:end
