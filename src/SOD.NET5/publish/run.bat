@echo off
echo -------------------------------------
echo publish SOD6 NugetPkgs....
echo power by bluedoctor,2024.5.14
echo -------------------------------------
echo publish SOD.Core
copy ..\Lib\PWMIS.SOD.Core\bin\Debug\*.nupkg NugetPkgs
copy ..\Lib\PWMIS.SOD.Core\bin\Debug\*.snupkg NugetPkgs
echo publish SOD.Core.Extensions
copy ..\Lib\PWMIS.SOD.Core.Extensions\bin\Debug\*.nupkg NugetPkgs
copy ..\Lib\PWMIS.SOD.Core.Extensions\bin\Debug\*.snupkg NugetPkgs

echo publish MySQL
copy ..\OtherProvider\PWMIS.DataProvider.Data.MySQL\bin\Debug\*.nupkg NugetPkgs
echo publish Oracle
copy ..\OtherProvider\PWMIS.DataProvider.Data.Oracle\bin\Debug\*.nupkg NugetPkgs
echo publish PostgreSQL
copy ..\OtherProvider\PWMIS.DataProvider.Data.PostgreSQL\bin\Debug\*.nupkg NugetPkgs

echo -------------------------------------
echo publish All Done.
pause

