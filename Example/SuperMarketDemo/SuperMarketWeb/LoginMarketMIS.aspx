<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LoginMarketMIS.aspx.cs" Inherits="SuperMarketWeb.LoginMarketMIS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
<tr><td>登录名称：</td><td>
    <asp:TextBox ID="txtLoginName" runat="server" Width="134px"></asp:TextBox>
    </td></tr>
<tr><td>登录密码：</td><td>
    <asp:TextBox ID="txtLoginPwd" runat="server" TextMode="Password" Width="135px"></asp:TextBox>
    </td></tr>
<tr><td>&nbsp;</td><td>
    <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" Text="登录" />
    </td></tr>
</table>
不知道管理账号?<a href="index.htm">请看这里！</a>
</asp:Content>
