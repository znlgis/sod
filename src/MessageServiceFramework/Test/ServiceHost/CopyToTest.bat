@echo off
set d= %date:/=-%
echo 优信拍客户端服务程序正在运行中(输出日志模式)...
echo ".\Log\%d%.txt"

.\TranstarAuctionServiceHost.exe A B OutLog




