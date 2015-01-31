<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Cashier.aspx.cs" Inherits="SuperMarketWeb.CashierManage.Cashier" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        Width="355px">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="查看详细" />
        </Columns>
    </asp:GridView>
    <hr />
    <table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[工号]*</td><td >
           <cc1:datatextbox ID="dtbWorkNumber" runat="server" LinkObject="雇员表" 
               LinkProperty="工号" PrimaryKey="True" Width="256px" SysTypeCode="String" 
               ReadOnly="True"></cc1:datatextbox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[姓名]</td><td >
           <cc1:datatextbox ID="DataTextBox2" runat="server" LinkObject="雇员表" 
               LinkProperty="姓名" Width="256px" SysTypeCode="String"></cc1:datatextbox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[性别]</td><td >
           <cc1:dataradiobutton ID="DataRadioButton1" runat="server" LinkObject="雇员表" 
               LinkProperty="性别" Text="男" SysTypeCode="Boolean" Value="True" 
               GroupName="Sex" />
&nbsp;<cc1:dataradiobutton ID="DataRadioButton2" runat="server" LinkObject="雇员表" 
               LinkProperty="性别" Text="女" SysTypeCode="Boolean" Value="False" 
               GroupName="Sex" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[出生日期]</td><td >
           <cc1:datacalendar ID="DataCalendar2" runat="server" LinkObject="雇员表" 
               LinkProperty="出生日期" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[入职时间]</td><td >
           <cc1:datacalendar ID="DataCalendar1" runat="server" LinkObject="雇员表" 
               LinkProperty="入职时间" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           
           </td></tr>
       </table>
       <asp:Label ID="lblMsg" runat="server" Text="操作"></asp:Label>
</asp:Content>
