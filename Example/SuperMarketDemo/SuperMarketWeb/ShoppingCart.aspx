<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="SuperMarketWeb.ShoppingCart" %>
<%@ Import Namespace="SuperMarketModel"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="lblWelcomeMsg" runat="server"></asp:Label>
 
 <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2"  
            AutoGenerateColumns="False" 
        onrowcancelingedit="GridView1_RowCancelingEdit" 
        onrowdeleting="GridView1_RowDeleting" onrowediting="GridView1_RowEditing" 
        onrowupdating="GridView1_RowUpdating">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="选择" />
            <asp:BoundField DataField="SerialNumber" HeaderText="条码号" ReadOnly="True" />
            <asp:BoundField DataField="GoodsName" HeaderText="商品名称" ReadOnly="True" />
            <asp:BoundField DataField="GoodsPrice" HeaderText="单价" ReadOnly="True" />
            <asp:BoundField DataField="GoodsNumber" HeaderText="购买数量" />
           
            <asp:TemplateField HeaderText="金额">
            <ItemTemplate>
            <%# ((Goods)Container.DataItem).GoodsNumber * ((Goods)Container.DataItem).GoodsPrice %>
            </ItemTemplate>
            </asp:TemplateField>
           
            <asp:CommandField ShowEditButton="True" />
            <asp:CommandField ShowDeleteButton="True" />
           
        </Columns>
    </asp:GridView>  
    【合计】<asp:Label ID ="lblAmout" runat="server"></asp:Label> 元 
    <p>
    去收银台 <a href="PayForGoods.aspx">付款</a>
    </p>
</asp:Content>
