param($installPath, $toolsPath, $package, $project)
$hostDir= Join-Path $([System.IO.Path]::GetDirectoryName($dte.Solution.FileName)) Host
if(!(Test-Path $hostDir) ){ 
md  $hostDir
}
# 复制文件到解决方案的 Host目录