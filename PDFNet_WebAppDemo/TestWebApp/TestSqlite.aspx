<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSqlite.aspx.cs" Inherits="TestWebApp.TestSqlite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="初始化数据库" />
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="加载数据" />
        <br />
        <br />
        <br />
        <table>
        <tr><td>ID：</td><td> 
        <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
            </td></tr>
        <tr><td>Name：</td><td><asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td></tr>
        <tr><td>&nbsp;</td><td>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="插入数据" />
            </td></tr>
        </table>
        &nbsp;
        <br />
        <asp:Label ID="Label1" runat="server" Text="msg:"></asp:Label>
        <br />
        <asp:GridView ID="GridView1" runat="server" BackColor="White" 
            BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
            <RowStyle BackColor="White" ForeColor="#003399" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
            <SortedAscendingCellStyle BackColor="#EDF6F6" />
            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
            <SortedDescendingCellStyle BackColor="#D6DFDF" />
            <SortedDescendingHeaderStyle BackColor="#002876" />
        </asp:GridView>
        <br />
    
    </div>
    </form>
</body>
</html>
