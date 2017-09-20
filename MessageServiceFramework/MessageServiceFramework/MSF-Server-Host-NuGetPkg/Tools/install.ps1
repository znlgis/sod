param($installPath, $toolsPath, $package, $project)
$hostDir= Join-Path $([System.IO.Path]::GetDirectoryName($dte.Solution.FileName)) Host
if(!(Test-Path $hostDir) ){ 
md  $hostDir
}
# 复制文件到解决方案的 Host目录
$host_exe_target= Join-Path $hostDir PdfNetEF.MessageServiceHost.exe
$host_pdb_target= Join-Path $hostDir PdfNetEF.MessageServiceHost.pdb
$updateSrv_target =  Join-Path $hostDir UpdateService.bat

$host_exe_src= Join-Path $toolsPath PdfNetEF.MessageServiceHost.exe
$host_pdb_src= Join-Path $toolsPath PdfNetEF.MessageServiceHost.pdb
$updateSrv_src =  Join-Path $toolsPath UpdateService.bat

if(! (Test-Path $host_exe_target)) { Copy-Item $host_exe_src $hostDir }
if(! (Test-Path $host_pdb_target)) { Copy-Item $host_pdb_src $hostDir }
if(! (Test-Path $updateSrv_target)) { Copy-Item $updateSrv_src $hostDir }

# 修改编译后事件，复制文件，启动主进程