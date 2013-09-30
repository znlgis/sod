<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SuperMarketWeb.EmployeeManage.Index" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        雇员管理</p>
        <hr />
       <table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[工号]*</td><td >
           <cc1:DataTextBox ID="dtbWorkNumber" runat="server" LinkObject="雇员表" 
               LinkProperty="工号" PrimaryKey="True" Width="256px" SysTypeCode="String" 
               ErrorMessage="工号不能为空" MessageType="提示框"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[姓名]</td><td >
           <cc1:DataTextBox ID="DataTextBox2" runat="server" LinkObject="雇员表" 
               LinkProperty="姓名" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[性别]</td><td >
           <cc1:DataRadioButton ID="rdbSexMan" runat="server" LinkObject="雇员表" 
               LinkProperty="性别" Text="男" SysTypeCode="Boolean" Value="True" 
               GroupName="Sex" isNull="False" />
&nbsp;<cc1:DataRadioButton ID="rdbSexWomen" runat="server" LinkObject="雇员表" 
               LinkProperty="性别" Text="女" SysTypeCode="Boolean" Value="False" 
               GroupName="Sex" isNull="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[出生日期]</td><td >
           <cc1:DataCalendar ID="DataCalendar2" runat="server" LinkObject="雇员表" 
               LinkProperty="出生日期" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[入职时间]</td><td >
           <cc1:DataCalendar ID="DataCalendar1" runat="server" LinkObject="雇员表" 
               LinkProperty="入职时间" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[职务名称]</td><td >
           <cc1:DataDropDownList ID="DataDropDownList1" runat="server" LinkObject="雇员表" 
               LinkProperty="职务名称" SysTypeCode="String">
               <asp:ListItem Selected="True">收银员</asp:ListItem>
               <asp:ListItem>收银主管</asp:ListItem>
               <asp:ListItem>营业员</asp:ListItem>
               <asp:ListItem>仓管员</asp:ListItem>
               <asp:ListItem>营运经理</asp:ListItem>
               <asp:ListItem>采购主管</asp:ListItem>
           </cc1:DataDropDownList>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           &nbsp;<asp:Button ID="btnNew" runat="server" onclick="btnNew_Click" Text="新建" />
           </td></tr>
       </table>
    <asp:Label ID="lblMsg" runat="server" Text="操作信息"></asp:Label>
    <br />
    <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        Width="626px">
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
        SQL="SELECT [工号],[姓名],[性别],[出生日期],[入职时间],[职务名称]  FROM [雇员表] order by [姓名]" />
</asp:Content>
