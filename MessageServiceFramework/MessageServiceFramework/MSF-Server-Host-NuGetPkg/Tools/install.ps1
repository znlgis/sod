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

# 修改项目文件的输出方式
$project.ProjectItems.Item("IOCConfig.xml").Properties.Item("CopyToOutputDirectory").Value=2
$project.ProjectItems.Item("PdfNetEF.MessageServiceHost.exe.config").Properties.Item("CopyToOutputDirectory").Value=2

# 修改编译前后后事件(复制文件，启动主进程)
$preBEvent=$project.Properties.Item("PreBuildEvent")
if(! $preBEvent.Value.Contains("PreCompile.bat")) { 
 $preBEvent.Value= $preBEvent.Value+"`r`n start `"`" `"`$(SolutionDir)Host\PreCompile.bat`""
}

$postBEvent= $project.Properties.Item("PostBuildEvent")
if(! $postBEvent.Value.Contains("(TargetDir)*.*")) { 
 $postBEvent.Value= $postBEvent.Value+"`r`n copy /y `"`$(TargetDir)*.*`" `"`$(SolutionDir)Host`""
}
if(! $postBEvent.Value.Contains("SucessCompiled.vbs")) { 
   $myBat= @"
`r`n
cd `"`$(SolutionDir)Host`" `r`n 
start `"MessageService Host`"  `"SucessCompiled.vbs`" `r`n
"@
   $postBEvent.Value= $postBEvent.Value+$myBat
}
