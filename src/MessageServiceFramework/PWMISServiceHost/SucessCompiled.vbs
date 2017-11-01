'const vbOKOnly =0
'const vbOKCancel=1
'const vbOK =1
'const vbCancel=2

if vbOK = MsgBox ("启动 PWMIS 服务宿主吗？",vbOKCancel,"消息服务框架")  then
  Set objShell = CreateObject("Wscript.Shell")
  objShell.Run("PdfNetEF.MessageServiceHost.exe")
end if





