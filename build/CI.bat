set build-base-path=../exceptionless.webhooks
set outpust-base-path=../../build
set username=justmine

for /f "delims=> tokens=2" %%i in ('findstr "<VersionMajor>.*</VersionMajor>" version.props')do set "Major=%%i"; 
for /f "delims=> tokens=2" %%j in ('findstr "<VersionMinor>.*</VersionMinor>" version.props')do set "Minor=%%j"; 
for /f "delims=> tokens=2" %%h in ('findstr "<VersionPatch>.*</VersionPatch>" version.props')do set "Patch=%%h"; 

set Major=%Major:~0,1%
set Minor=%Minor:~0,1%
set Patch=%Patch:~0,1%
set Version=%Major%.%Minor%.%Patch%

set first=Web
set version=%Version%
set imagename=exceptionless.api.webhook
set imagefullname=%username%/%imagename%:%version%

dotnet publish %build-base-path%/%first%/%first%.csproj -o %outpust-base-path%/%first% -c release
docker build -t %imagefullname% ./%first%
docker push %imagefullname%

rd /s /q %first%

pause
goto EOF