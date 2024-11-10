@echo off
echo 请使用管理员身份运行
copy "D:\OpenSource\Repos\sod\src\SOD.NET5\publish\NugetPkgs\*.nupkg" "C:\Program Files (x86)\Microsoft SDKs\NuGetPackages"
copy "D:\OpenSource\Repos\sod\src\SOD.NET5\publish\NugetPkgs\*.snupkg" "C:\Program Files (x86)\Microsoft SDKs\NuGetPackages"

pause
