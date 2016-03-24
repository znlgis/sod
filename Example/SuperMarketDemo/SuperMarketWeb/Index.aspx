<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SuperMarketWeb.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        <asp:Label ID="lblWelcomeMsg" runat="server"></asp:Label>
        </p>
        
        <table>
        <tr style =" background-color :#DEBA84">
        <td>商品价格信息</td>
        <td><a href="ShoppingCart.aspx">我的购物车</a></td>
        </tr>
        <tr>
        <td>
    <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
            AutoGenerateColumns="False">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="购买" />
            <asp:BoundField DataField="GoodsID" HeaderText="存货记录号" Visible="False" />
            <asp:BoundField DataField="Manufacturer" HeaderText="厂商名称" />
            <asp:BoundField DataField="GoodsName" HeaderText="商品名称" />
            <asp:BoundField DataField="SerialNumber" HeaderText="条码号" />
            <asp:BoundField DataField="GoodsPrice" HeaderText="售价" />
            <asp:BoundField DataField="MakeOnDate" HeaderText="生产日期" />
            <asp:BoundField DataField="CanUserMonth" HeaderText="保质期（月）" />
            <asp:BoundField DataField="ExpireDate" HeaderText="过期时间" />
            <asp:BoundField DataField="Stocks" HeaderText="库存数量" />
           
        </Columns>
    </asp:GridView>  
            <br />
    价格说明： 
            <ul>
               
                   
                    <li> 价格分为原价，折扣价，会员价。    
                    </li>
                     <li> 标注有折扣价的，执行折扣价，否则执行原价；    
                    </li>
                     <li> 标注有会员价的，如果是会员，执行会员价，否则执行原价；    
                    </li>
                     <li> 如果商品有折扣价，不论是否会员，只能享受折扣价；    
                    </li>
                    <li>执行折扣价或者会员价，会员不享受积分。</li>
                    <li>商品的最终售卖价格由库存，会员积分等综合决定，在顾客结账的时候可以看到。</li>
            </ul>
        </td>
        <td>
        <img src ="images/ShoppingCar.JPG" alt="购物车"/><br />已选购商品 购物车<br />已选购商品 <asp:Label ID ="lblGoodsCount" runat="server"></asp:Label>种
        &nbsp;<a href ="ShoppingCart.aspx">详细</a>
        <table style="border-style: double; background-color :#DEBA84">
        <tr>
        <td>
            购买数量
        </td>
        <td>
            <asp:TextBox ID="txtBuyCount" runat="server" Width="44px"></asp:TextBox>
            件<asp:Button ID="btnEditBuyCount" runat="server" Text="修改" 
                onclick="btnEditBuyCount_Click" />
&nbsp;</td>
        </tr>
        <tr>
        <td>
        合计价格
        </td>
        <td>
            <asp:Label ID="lblBuyPrice" runat="server"></asp:Label>
&nbsp;元</td>
        </tr>
        <tr>
        <td></td>
        <td><asp:Button ID="btnBuy" runat="server" Text="加入购物车" onclick="btnBuy_Click" />  </td>
        </tr>
        </table>
          
  
&nbsp;</td>
        </tr>
        </table>
    
</asp:Content>
