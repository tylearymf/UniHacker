@echo off
REM 注意版本号必须是长度为3的，比如 1.x、2.x之类的，1.x.x 太长了不用这种，下面 line:~9,3 固定读取长度3的字符串

setlocal enabledelayedexpansion
set defaultVersion=0.0
set version=%defaultVersion%
for /f "tokens=*" %%x in (UniHacker.csproj) do (
    set line=%%x
	set prefix=!line:~0,9!
	set pattern="<Version>"
	if "!prefix!"==!pattern! (
		set version=!line:~9,3!
	)
)

if "%version%"=="%defaultVersion%" (
	echo could not find the version number.
	pause
	exit
)

echo detect project version : "%version%"
echo build start

set exeName=UniHacker
set publish=.\publish
set verName=V%version%
if exist "%publish%" (
	echo cleanup publish folder
	rd /s /q "%publish%"
)

set param=--self-contained:true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeAllContentForSelfExtract=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false

echo dotnet publish windows x86
dotnet publish -c:Release -f:net6.0 -r:win-x86 -o:%publish%\win-x86 %param% > nul

echo dotnet publish windows x64
dotnet publish -c:Release -f:net6.0 -r:win-x64 -o:%publish%\win-x64 %param% > nul

echo dotnet publish macos x64
dotnet publish -c:Release -f:net6.0 -r:osx-x64 -o:%publish%\osx-x64 %param% -p:PublishSingleFile=false > nul

echo dotnet publish linux x64
dotnet publish -c:Release -f:net6.0 -r:linux-x64 -o:%publish%\linux-x64 %param% > nul

echo rename executable file
set exeFullName=%exeName%%verName%
ren %publish%\win-x86\%exeName%.exe %exeFullName%.exe
ren %publish%\win-x64\%exeName%.exe %exeFullName%.exe
ren %publish%\linux-x64\%exeName% %exeFullName%.AppImage

echo create macos bundle
set osxPath=%publish%\osx-x64
set bundlePath=%osxPath%\%exeFullName%.app
echo d| xcopy /E/Y ".\Bundle\%exeName%.app" "%bundlePath%" > nul
echo f| xcopy /Y "%osxPath%\%exeName%" "%bundlePath%\Contents\Resources\script" > nul
del /q %osxPath%\%exeName% > nul
echo d| move /Y "%osxPath%\*.*" "%bundlePath%\Contents\Resources\" > nul

echo compress windows x86 file
7z a %publish%\%exeName%-win-x86.7z %publish%\win-x86\* > nul

echo compress windows x64 file
7z a %publish%\%exeName%-win-x64.7z %publish%\win-x64\* > nul

echo compress macos x64 file
7zg a -ttar -so -an %publish%\osx-x64\* | 7zg a -si %publish%\%exeName%-osx-x64.tgz > nul

echo compress linux x64 file
7zg a -ttar -so -an %publish%\linux-x64\* | 7zg a -si %publish%\%exeName%-linux-x64.tgz > nul

echo calculate file hash
7z h -scrcSHA256 %publish%\*\*.exe %publish%\*\*.AppImage >> %publish%\hash.txt
7z h -scrcSHA256 %publish%\*\*.app >> %publish%\hash_mac.txt

echo build finished. output:"%publish%"
pause