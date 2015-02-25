<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="BuyGoodsOK.aspx.cs" Inherits="SuperMarketWeb.BuyGoodsOK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Label ID="lblWelcomeMsg" runat="server"></asp:Label>

下面是你的购物小票：<br />
    <asp:TextBox ID="txtSellNote" runat="server" BorderStyle="Dashed" 
        Height="317px" TextMode="MultiLine" Width="469px"></asp:TextBox>
    <br />

</asp:Content>
