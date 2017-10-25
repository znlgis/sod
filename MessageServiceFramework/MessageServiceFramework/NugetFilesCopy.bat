@echo off
echo PDF.NET.MSF Nuget 打包文件前更新文件 批处理程序
echo MSF Client
copy ..\PWMISServiceClient\bin\Debug\PWMIS.EnterpriseFramework.Common.xml  MSF-Client-NuGetPkg\lib\net40\
copy ..\PWMISServiceClient\bin\Debug\PWMIS.EnterpriseFramework.Message.PublishService.xml  MSF-Client-NuGetPkg\lib\net40\
copy ..\PWMISServiceClient\bin\Debug\PWMIS.EnterpriseFramework.Message.SubscriberLib.xml  MSF-Client-NuGetPkg\lib\net40\
copy ..\PWMISServiceClient\bin\Debug\PWMIS.EnterpriseFramework.Service.Basic.xml  MSF-Client-NuGetPkg\lib\net40\
copy ..\PWMISServiceClient\bin\Debug\PWMIS.EnterpriseFramework.Service.Client.xml  MSF-Client-NuGetPkg\lib\net40\

echo MSF Server Group
copy ..\PWMISServiceHost\bin\Debug\PWMIS.EnterpriseFramework.Service.Group.*  MSF-Server-Group-NuGetPkg\lib\

echo MSF Server
copy ..\PWMISServiceHost\bin\Debug\PWMIS.EnterpriseFramework.Service.Runtime.* MSF-Server-NuGetPkg\lib\

echo PWMIS Enterprise Framework
copy ..\PWMISServiceHost\bin\Debug\PWMIS.EnterpriseFramework.IOC.xml  PWMIS.EnterpriseFramework-NugetPkg\lib\
copy ..\PWMISServiceHost\bin\Debug\PWMIS.EnterpriseFramework.ModuleRoute.XML  PWMIS.EnterpriseFramework-NugetPkg\lib\

echo MSF Host
copy ..\PWMISServiceHost\bin\Debug\PWMIS.EnterpriseFramework.Message.PublisherLib.*  MSF-Server-Host-NuGetPkg\lib\
copy ..\PWMISServiceHost\bin\Debug\PdfNetEF.MessageServiceHost.exe MSF-Server-Host-NuGetPkg\Tools\
copy ..\PWMISServiceHost\bin\Debug\PdfNetEF.MessageServiceHost.pdb MSF-Server-Host-NuGetPkg\Tools\

echo 文件复制全部完成
pause






