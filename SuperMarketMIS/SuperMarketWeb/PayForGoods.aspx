<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="PayForGoods.aspx.cs" Inherits="SuperMarketWeb.PayForGoods" %>
<%@ Import Namespace="SuperMarketModel"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
您好，欢迎您来到收银台，请选择为您服务的收银员：
<asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        Width="479px" AutoGenerateColumns="False">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="选择" />
            <asp:TemplateField HeaderText="收银员">
                <ItemTemplate>
                    <%# ((SuperMarketBLL.CashierRegisterBIZ)Container.DataItem).CurrCashier.CashierName  %>
                </ItemTemplate>
               
                <ItemStyle Width="100px" />
               
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工号">
                <ItemTemplate>
                    <%# ((SuperMarketBLL.CashierRegisterBIZ)Container.DataItem).CurrCashier.WorkNumber  %>
                </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="收银机号">
                <ItemTemplate>
                     <%# ((SuperMarketBLL.CashierRegisterBIZ)Container.DataItem).CurrCRManchines.CashRegisterNo   %>
                </ItemTemplate>
                <ItemStyle Width="200px" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p>
        <asp:Button ID="btnWaite" runat="server" Text="在此收银台排队" 
            onclick="btnWaite_Click" OnClientClick ="" />
        <asp:Label ID="lblQueue" runat="server"></asp:Label>
    </p>
    <p>
        您本次的购物价格清单：
    <asp:GridView ID="gvSPCart" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2"  
            AutoGenerateColumns="False" 
       >
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
            <asp:BoundField DataField="DiscountPrice" HeaderText="折扣价" ReadOnly="True" />
            <asp:BoundField DataField="GoodsNumber" HeaderText="购买数量" />
            <asp:BoundField DataField="GoodsMoney" HeaderText="实收金额" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>  
    </p>
    <p>
        说明：这里的购买数量改变为0，表示实际库存为0，不可销售。</p>
    <p>
    【合计】<asp:Label ID ="lblAmout" runat="server"></asp:Label> 元 
    </p>
    <p>
    &nbsp;<asp:Button ID="btnOK" runat="server" onclick="btnOK_Click" Text="同意支付" />
&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="返回继续购物" 
            onclick="btnCancel_Click" />
    &nbsp;
        <asp:Button ID="btnQuitBuy" runat="server" onclick="btnQuitBuy_Click" 
            Text="放弃本次购物" />
    </p>

</asp:Content>
