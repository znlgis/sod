===============================================================================
****                  SOD Framework MVVM Example (WinForm)               ******
===============================================================================
about the SOD, more infomation :http://pwmis.codeplex.com
about SOD MVVM news :https://www.oschina.net/news/78949/pdf-net-5-6-0-1111
about SOD MVVM blog :http://www.cnblogs.com/bluedoctor/p/6060278.html

powered by the SOD Framework Team, Blue Doctor.
date: 2016.11.11 
------------------------------------------------------------------------------

SOD 框架 MVVM WinForm 示例程序说明

本示例程序解决方案分为三个项目程序集：
WinFormMvvm:            WinForm 示例程序主程序，视图类所在程序集
WinFormMvvm.Model:      模型类程序集
WinFormMvvm.ViewModel:  视图模型程序集

程序在App.config中指定了本次附加测试的数据库，数据库类型为 Access，默认的连接字符串可能要求Office 2007以上版本支持。
如果你需要更低版本的 Access 数据库支持，或者换用其它数据库（比如 SqlServer)，请阅读参考下面步骤提供的信息：
1，打开下面链接：
   http://pwmis.codeplex.com/ 
2，看到内容章节“3，修改下App.config 文件的连接配置”；
3，点击本节下的链接“2.2.3 扩展数据访问类配置”。

了解更多信息或者加入社区QQ群讨论，或者捐助本框架，请移步框架官网：
http://www.pwmis.com/sqlmap

感谢你选择SOD框架，相信它能够为你的开发带来很大的便利！

注意：如果你发现从 codeplex 直接源码下载的编译有问题，请看到
TFS，SVN 源码位置：\PWMIS\Example\WinFormMvvm
下载一个 packages.rar 和 包说明.txt 2个文件，按照说明操纵。