<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAppTables.aspx.cs" Inherits="SuperMarketWeb.Setup.CreateAppTables" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    欢迎使用<a href="http://pwmis.codeplex.com" target="_blank">PDF.NET 开源项目</a>之“超市管理系统”DEMO程序。
     
        <br />
        <br />
        [<a href="../Default.aspx">返回首页</a>]<br />
    当前页面用于在网站上创建使用的表，如果系统已经创建数据库且请勿执行该页面。
     
        <br />
        <br />
        ----------------------------------------------------------------------------<br />
    当前系统的数据访问提供程序类型： 
        <asp:Label ID="lblProviderName" runat="server" Text="lblProviderName"></asp:Label>
    <br />
    当前系统数据库默认的连接名称（connectionStrings配置节）：<asp:Label ID="lblConnName" 
            runat="server" Text="lblConnName"></asp:Label>
&nbsp;<br />
    脚本资源情况：
     
        <asp:Label ID="lblScript" runat="server" Text="lblScript"></asp:Label>
        <br />
        <br />
        如果本页出现错误信息，请单击测试按钮，检查数据表是否创建，如果没有请执行建表脚本。<br />
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="执行建表脚本" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="测试" />
        <br />
        <asp:Label ID="lblMsg" runat="server" Text="就绪"></asp:Label>
    <br />

    </div>
    <asp:Label ID="lblErrMsg" runat="server" Text="错误信息"></asp:Label>
    </form>
</body>
</html>
