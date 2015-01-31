<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StockInfo.aspx.cs" Inherits="SuperMarketWeb.GoodsManage.StockInfo" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4
        {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;" id="tbGoosBaseInfo" runat="server">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[存货记录号]*</td><td >
           <cc1:DataLabel ID="dlCHJLH" runat="server" DataFormatString="" isNull="True" 
               LinkObject="存货信息表" LinkProperty="存货记录号" PrimaryKey="True" ReadOnly="True" 
               SysTypeCode="Int32"></cc1:DataLabel></td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[商品名称]</td><td >
           <cc1:DataDropDownList ID="ddlGoodsNames" runat="server" LinkObject="" 
               LinkProperty="" SysTypeCode="String" AutoPostBack="True" 
               onselectedindexchanged="ddlGoodsNames_SelectedIndexChanged" 
               >
           </cc1:DataDropDownList>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td class="style4" >[厂商]</td><td class="style4" >
           <cc1:DataDropDownList ID="ddlManufacturer" runat="server" LinkObject="" 
               LinkProperty="" SysTypeCode="String">
               <asp:ListItem Selected="True">请选择</asp:ListItem>
           </cc1:DataDropDownList>
                                <cc1:DataTextBox ID="dtbSN" runat="server" LinkObject="存货信息表" 
                                    LinkProperty="条码号" SysTypeCode="String" Width="78px" Type="Currency" 
                                    Visible="False"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[售价]</td><td >
                                <cc1:DataTextBox ID="DataTextBox3" runat="server" LinkObject="存货信息表" 
                                    LinkProperty="售价" SysTypeCode="Decimal" Width="128px" Type="Currency" 
                                    OftenType="浮点数"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[成本价]</td><td >
           <cc1:DataTextBox ID="DataTextBox4" runat="server" LinkObject="存货信息表" 
               LinkProperty="成本价" SysTypeCode="Decimal" width="128px" 
               OftenType="浮点数"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[生产日期]</td><td >
           <cc1:DataCalendar ID="DataCalendar3" runat="server" LinkObject="存货信息表" 
               LinkProperty="生产日期" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[上货时间]</td><td >
           <cc1:DataCalendar ID="DataCalendar2" runat="server" LinkObject="存货信息表" 
               LinkProperty="上货时间" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[库存数量]</td><td >
           <cc1:DataTextBox ID="DataTextBox6" runat="server" LinkObject="存货信息表" 
               LinkProperty="库存数量" SysTypeCode="Int32" Type="Integer" 
               OftenType="整数"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           <asp:Button ID="btnNew" runat="server" onclick="btnNew_Click" Text="新建" />
           <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" Text="删除" />
           </td></tr>
       </table>
       <p>
       
           <asp:Label ID="lblMsg" runat="server" Text="操作"></asp:Label>
       
       </p>
       
       <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        Width="504px">
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
        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#8C4510" PageSize="5" 
        AutoBindData="True" AutoConfig="True" AutoIDB="True" BindToControl="GridView1" 
        SQL="SELECT [存货记录号],[条码号],[售价],[成本价],[生产日期],[上货时间],[库存数量]  FROM [存货信息表] order by [存货记录号] desc" />

</asp:Content>
