param($installPath, $toolsPath, $package, $project)
$hostDir= Join-Path $([System.IO.Path]::GetDirectoryName($dte.Solution.FileName)) Host
if(!(Test-Path $hostDir) ){ 
  md  $hostDir
}

# 复制文件到解决方案的 Host目录
$host_PreCompile_target= Join-Path $hostDir PreCompile.bat
$host_SucessCompiled_target= Join-Path $hostDir SucessCompiled.vbs

$host_PreCompile_src= Join-Path $toolsPath PreCompile.bat
$host_SucessCompiled_src= Join-Path $toolsPath SucessCompiled.vbs
if(! (Test-Path $host_PreCompile_target)) { Copy-Item $host_PreCompile_src $hostDir }
if(! (Test-Path $host_SucessCompiled_target)) { Copy-Item $host_SucessCompiled_src $hostDir }

# 修改编译前后后事件(复制文件)
$preBEvent=$project.Properties.Item("PreBuildEvent")
if(! $preBEvent.Value.Contains("PreCompile.bat")) { 
 $preBEvent.Value= $preBEvent.Value+"`r`n start `"`" `"`$(SolutionDir)Host\PreCompile.bat`""
}

$postBEvent= $project.Properties.Item("PostBuildEvent")
if(! $postBEvent.Value.Contains("(TargetDir)*.*")) { 
 $postBEvent.Value= $postBEvent.Value+"`r`n copy /y `"`$(TargetDir)*.*`" `"`$(SolutionDir)Host`""
}