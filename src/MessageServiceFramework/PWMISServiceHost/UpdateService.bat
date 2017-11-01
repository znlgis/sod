@echo off
echo ------------------------------
echo PWMIS MSF Service Host Update
echo Power by bluedoctor, 2012.5.22
echo 注：本文件必须保存为 ANSI 格式
echo ------------------------------
if "%1"=="" goto err

echo 检测服务进程...
:start
ping 127.0.0.1 -n 2 >nul 2>nul
tasklist >tasklist.txt
find /i tasklist.txt "PdfNetEF.MessageServiceHo"
if errorlevel 1 ((del /q tasklist.txt)&(goto end))
if errorlevel 0 ((goto start))
:end 
echo 服务进程已经退出，执行文件复制操作...
copy %1%\*.* .\ >CopyedFile.txt
echo 文件全部复制完成
call run.bat
goto ok

:err
echo 批处理参数错误，未指定源目录

:ok
exit





