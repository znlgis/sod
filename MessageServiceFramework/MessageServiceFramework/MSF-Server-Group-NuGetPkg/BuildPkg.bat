@echo off
echo 开始之前，请先检查 obj\Debug\PDF.Net.MSF.Client.nuspec 文件是否生成，并且移除了打包 JSON 文件
pause
"C:\Program Files (x86)\MSBuild\NuProj\nuget.exe" pack obj\Debug\PDF.Net.MSF.Server.Group.nuspec
copy *.nupkg ..\LocalPkgs\

echo 文件包生成和复制完成
pause
