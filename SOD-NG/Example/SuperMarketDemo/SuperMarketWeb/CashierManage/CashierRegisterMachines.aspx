
<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CashierRegisterMachines.aspx.cs" Inherits="SuperMarketWeb.CashierManage.CashierRegisterMachines" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[编号]*</td><td >
           <cc1:DataTextBox ID="dtbSN" runat="server" LinkObject="设备表" 
               LinkProperty="编号" PrimaryKey="True" Width="126px" SysTypeCode="String"></cc1:DataTextBox>
           <asp:Button ID="btnNewSN" runat="server" onclick="btnNewSN_Click" Text="生成编号" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[设备名称]</td><td >
           <cc1:DataTextBox ID="DataTextBox2" runat="server" LinkObject="设备表" 
               LinkProperty="设备名称" Width="256px" SysTypeCode="String">收银机</cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[型号]</td><td >
           <cc1:DataTextBox ID="DataTextBox3" runat="server" LinkObject="设备表" 
               LinkProperty="型号" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[购置时间]</td><td >
           <cc1:DataCalendar ID="DataCalendar2" runat="server" LinkObject="设备表" 
               LinkProperty="购置时间" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
      <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[备注]</td><td >
           <cc1:DataTextBox ID="DataTextBox1" runat="server" LinkObject="设备表" 
               LinkProperty="备注" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           </td></tr>
       </table>
    <asp:Label ID="lblMsg" runat="server" Text="操作信息"></asp:Label>
    <br />
    <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="编辑" />
        </Columns>
    </asp:GridView>
    <br />
<cc1:ProPageToolBar ID="ProPageToolBar1" runat="server" Width="624px" 
        AutoBindData="True" AutoConfig="True" AutoIDB="True" BindToControl="GridView1" 
        SQL="SELECT  [编号] ,[设备名称],[型号] ,[购置时间] ,[备注]  FROM [设备表] where [设备名称]='收银机' order by [编号] desc" />

</asp:Content>
