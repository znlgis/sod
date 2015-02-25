<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CashierConsole.aspx.cs" Inherits="SuperMarketWeb.CashierManage.CashierConsole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2"  
        Width="336px" onrowdatabound="GridView1_RowDataBound">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="选择收银机">
            <ItemTemplate>
            <asp:DropDownList ID ="ddlSN" runat="server" AutoPostBack="true" OnSelectedIndexChanged="btn1_Click" >
            <asp:ListItem>请选择收银机编号</asp:ListItem>
            </asp:DropDownList>
            </ItemTemplate>
            
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Button ID ="btnRef" runat ="server"  Text ="刷新" onclick="btnRef_Click"/>
    <div  style ="display :none ">
<asp:Button ID ="btn1" runat ="server" OnClick ="btn1_Click" />
</div>
</asp:Content>
