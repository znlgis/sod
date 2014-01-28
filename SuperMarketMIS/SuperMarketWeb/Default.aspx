<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SuperMarketWeb._Default" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 190px;
        }
        .style3
        {
            width: 191px;
        }
        .style4
        {
            font-size: xx-large;
        }
        .style5
        {
            font-size: x-large;
        }
    </style>
    <script type ="text/javascript">
        function CancelInput() {
            document.getElementById("dbtCID").value = "NULL";
            document.getElementById("dtbCustomerName").value = "NULL";
        }
    </script>
</head>
<body>

    <form id="form1" runat="server">
    <div style="background-color: #FFCC99; border-style: groove">
        <a href="欢迎.htm">
    <img  src="images/MarketLog.jpg" alt ="PDF.NET SupperMarket" 
            style="border-width: 0px"/></a><span class="style4"><a 
            href="http://www.pwmis.com/sqlmap">PDF.NET 框架</a>之<a 
            href="http://pwmis.codeplex.com">超市管理系统 Demo实例程序</a></span><br />
        欢迎光临本超市，请输入你的会员号：<br />
        <table>
        <tr><td class="style3">
         <asp:TextBox ID="txtComeIn" runat="server" Width="170px"></asp:TextBox>
        </td>
        <td>
         <asp:Button ID="btnComeIn" runat="server" Text="进入超市" 
            onclick="btnComeIn_Click" />
        </td></tr>
        </table>
      
    </div>
   
    <div style="line-height: normal; background-color: #FFCC66; border-style: outset">
    <table>
    <tr><td class="style2">没有会员号?请注册!&nbsp;</td><td> <asp:Button ID="btnRegister" runat="server" 
            onclick="btnRegister_Click" Text="注册会员" />
        </td></tr>
     <tr><td class="style2">如果不想注册，也可以</td><td> <asp:Button ID="btnNiming" runat="server" Text="匿名进入" 
            onclick="Button1_Click" />
      
         </td></tr>
         <tr>
         <td class="style2">如果你是管理员，请这里</td>
         <td class="style5"><a href="LoginMarketMIS.aspx">管理员登录</a></td>
         </tr>
    </table>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        (拥有会员号,将能够享受积分以及折扣)<br /> 
        &nbsp;
      
   </div>
   <div style="background-color: #FFCC66; border-style: outset">
        <table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;" id="tbCustomerInfo" runat="server">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[客户号]*</td><td >
           <cc1:DataTextBox ID="dbtCID" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="CustomerID" PrimaryKey="True" Width="256px" 
               SysTypeCode="String" ScriptPath="/System/WebControls/script.js" 
               isNull="False" OftenType="身份证 15位|18位" ErrorMessage="客户号必须为身份证号" 
               MessageType="层" onclickshowinfo="True" RegexName="客户号"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[姓名]</td><td >
           <cc1:DataTextBox ID="dtbCustomerName" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="CustomerName" Width="256px" SysTypeCode="String" 
               ErrorMessage="姓名不能为空" isNull="False" OftenType="无"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[性别]</td><td >
&nbsp;<cc1:DataRadioButton ID="DataRadioButton1" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Sex" Text="男" SysTypeCode="Boolean" Value="True" 
               GroupName="Sex" />
                                <cc1:DataRadioButton ID="DataRadioButton2" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Sex" Text="女" SysTypeCode="Boolean" Value="False" 
               GroupName="Sex" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[出生日期]</td><td >
           <cc1:DataCalendar ID="DataCalendar2" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Birthday" ScriptPath="/Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[联系电话]</td><td >
           <cc1:DataTextBox ID="DataTextBox5" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="PhoneNumber" SysTypeCode="String" ErrorMessage="联系电话填写不正确" 
               OftenType="电话号码或手机" onclickshowinfo="True" RegexName="电话号码"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[联系地址]</td><td >
           <cc1:DataTextBox ID="DataTextBox6" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Address" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           &nbsp;<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           &nbsp;
           
           <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" Text="取消"  OnClientClick="CancelInput()"/>
           
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >填表说明</td><td >
          客户号请输入您的身份证号</td></tr>
       </table>

          <asp:Label ID="lblMsg" runat="server" BackColor="White" ForeColor="Red"></asp:Label>
        </div>
        <br />
        服务器操作系统：<%=Environment.OSVersion.ToString() %> <%=Environment.Is64BitOperatingSystem? 64:32 %>位系统
        <br />
        .NET 运行库版本：<%=Environment.Version.ToString() %>
         <br />
        Web服务器版本:(<span  style=" color:Red; font-size: 16pt;"> <%:Request.ServerVariables["Server_SoftWare"]%></span>)
       
        <hr />
        本系统基于<a href="http://www.pwmis.com/sqlmap">PDF.NET</a> Ver 5.1 构建，<a href="index.htm">系统说明</a>
        <br />
        使用的数据驱动程序：<%: ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1].ProviderName%>
        <br />
        
         </form>
<hr />
请使用IE浏览器使用本系统！

</body>  
</html>
