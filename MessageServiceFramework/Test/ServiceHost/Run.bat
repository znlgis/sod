@echo off
set d= %date:/=-%
echo PWMIS MSF 服务程序正在运行中(输出日志模式)...
echo ".\Log\%d%.txt"

.\PdfNetEF.MessageServiceHost.exe A B OutLog




