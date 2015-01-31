<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox1" runat="server" Height="222px" TextMode="MultiLine" 
            Width="467px"></asp:TextBox>
    
    </div>
    <cc1:DataTextBox ID="DataTextBox1" runat="server"></cc1:DataTextBox>
    <cc1:DataRadioButton ID="DataRadioButton1" runat="server" SysTypeCode="Boolean" 
        Value="True" GroupName="sex" Text="男"  />
    <cc1:DataRadioButton ID="DataRadioButton2" runat="server" GroupName="sex" 
        SysTypeCode="Boolean" Text="女" Value="False" />
    </form>
</body>
</html>
