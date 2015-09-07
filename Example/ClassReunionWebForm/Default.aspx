<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 274px;
        }
        .auto-style2 {
            height: 20px;
        }
        .auto-style3 {
            width: 274px;
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width: 754px" border="1">
        <tr>
            <td>姓名</td><td class="auto-style1">
            <cc1:DataTextBox ID="dtName" runat="server"></cc1:DataTextBox>
            </td><td>请填写同学实名</td>
        </tr>
        <tr>
            <td class="auto-style2">身份证号</td><td class="auto-style3">
            <cc1:DataTextBox ID="dtPersonID" runat="server"></cc1:DataTextBox>
            </td><td class="auto-style2">请填写，修改信息需要</td>
        </tr>
        <tr>
            <td class="auto-style2">联系电话</td><td class="auto-style3">
            <cc1:DataTextBox ID="DataTextBox3" runat="server"></cc1:DataTextBox>
            </td><td class="auto-style2">必填，建议手机</td>
        </tr>
        <tr>
            <td>是否预定房间</td><td class="auto-style1">
            <cc1:DataCheckBox ID="DataCheckBox1" runat="server" Text="预定" />
            </td><td>&nbsp;</td>
        </tr>
        <tr>
            <td>出发地</td><td class="auto-style1">
            <cc1:DataTextBox ID="DataTextBox4" runat="server"></cc1:DataTextBox>
            </td><td>比如来自北京</td>
        </tr>
        <tr>
            <td>其它说明</td><td class="auto-style1">
            <cc1:DataTextBox ID="DataTextBox5" runat="server" Height="47px" TextMode="MultiLine" Width="264px"></cc1:DataTextBox>
            </td><td>你的特殊需求</td>
        </tr>
    </table>
    </div>
    <p>
        &nbsp;
        <asp:Button ID="btnSubmit" runat="server" Height="30px" Text="提交" Width="71px" OnClick="btnSubmit_Click" />
&nbsp;<cc1:DataLabel ID="dlMsg" runat="server" DataFormatString="" IsNull="True" LinkObject="" LinkProperty="" MaxLength="0" PrimaryKey="False" ReadOnly="True" SysTypeCode="Empty">就绪，如果输入姓名和身份证号已经存在，则是修改信息</cc1:DataLabel>
    </p>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
    </form>
    </body>
</html>
