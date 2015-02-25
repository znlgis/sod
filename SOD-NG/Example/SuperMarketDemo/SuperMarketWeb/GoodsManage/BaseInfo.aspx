<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BaseInfo.aspx.cs" Inherits="SuperMarketWeb.GoodsManage.BaseInfo" Title="无标题页" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;" id="tbGoosBaseInfo" runat="server">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[条码号]*</td><td >
           <cc1:DataTextBox ID="dbtSN" runat="server" LinkObject="GoodsBaseInfo" 
               LinkProperty="SerialNumber" PrimaryKey="True" Width="256px" 
               SysTypeCode="String" ScriptPath=""></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[商品名称]</td><td >
           <cc1:DataTextBox ID="DataTextBox2" runat="server" LinkObject="GoodsBaseInfo" 
               LinkProperty="GoodsName" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[厂商名称]</td><td >
                                <cc1:DataTextBox ID="DataTextBox3" runat="server" LinkObject="GoodsBaseInfo" 
                                    LinkProperty="Manufacturer" SysTypeCode="String" Width="256px"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[保质期]</td><td >
           <cc1:DataTextBox ID="DataTextBox4" runat="server" LinkObject="GoodsBaseInfo" 
               LinkProperty="CanUserMonth" SysTypeCode="Int32" OftenType="整数" 
               Type="Integer"></cc1:DataTextBox>
           月</td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           </td></tr>
       </table>
       <p>
       
           <asp:Label ID="lblMsg" runat="server" Text="操作"></asp:Label>
       
       </p>
       
       <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        Width="505px">
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
   <cc1:ProPageToolBar ID="ProPageToolBar1" runat="server" BackColor="#FFF7E7" 
        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#8C4510" 
        onpagechangeindex="ProPageToolBar1_PageChangeIndex" PageSize="5" />
</asp:Content>
