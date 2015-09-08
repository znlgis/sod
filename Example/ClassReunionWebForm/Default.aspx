<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClassReunionWeb._Default" %>

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
    
    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
 <div class="container">
      
  <div class="row-fluid">
   <div class="span2">
     <!--Sidebar content-->
      
    </div>
    <div class="span10">
      <!--Body content-->
          <div class="row-fluid">
            <div class ="row">
                <h1>同学会联系信息</h1>
            </div>
   
        <div class="row">
    <div >
    <table class="table table-striped table-bordered">
        <tr >
            <td>姓名</td><td class="auto-style1">
            <cc1:DataTextBox ID="dtName" runat="server" LinkProperty="Name" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="10" ></cc1:DataTextBox>
            </td><td>请填写同学实名，修改信息需要</td>
        </tr>
        <tr>
            <td class="auto-style2">身份证号</td><td class="auto-style3">
            <cc1:DataTextBox ID="dtPersonID" runat="server" LinkProperty="PersonID" SysTypeCode="String" LinkObject="ContactInfo" MaxLength="20"></cc1:DataTextBox>
            </td><td class="auto-style2">请填写，修改信息需要，18位数字</td>
        </tr>
        <tr>
            <td class="auto-style2">联系电话</td><td class="auto-style3">
            <cc1:DataTextBox ID="dtPhone" runat="server" LinkProperty="ContactPhone" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="20"></cc1:DataTextBox>
            </td><td class="auto-style2">必填，建议手机</td>
        </tr>
        <tr>
            <td>家属人数</td><td class="auto-style1">
            <cc1:DataTextBox ID="DataTextBox1" runat="server" LinkProperty="HomeMemberCount" LinkObject="ContactInfo" SysTypeCode="Int32" MaxLength="5"></cc1:DataTextBox>
            </td><td>没有，请写0</td>
        </tr>
        <tr>
            <td>是否预定房间</td><td class="auto-style1">
            <cc1:DataCheckBox ID="DataCheckBox1" runat="server" Text="预定" LinkProperty="NeedRoom" LinkObject="ContactInfo" SysTypeCode="Boolean"/>
            </td><td>我们将为你安排住宿</td>
        </tr>
        <tr>
            <td>出发地</td><td class="auto-style1">
            <cc1:DataTextBox ID="DataTextBox4" runat="server" LinkProperty="ComeFrom" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="50"></cc1:DataTextBox>
            </td><td>比如来自北京</td>
        </tr>
        <tr>
            <td>其它说明</td><td class="auto-style1">
            <cc1:DataTextBox ID="DataTextBox5" runat="server" Height="65px" TextMode="MultiLine" Width="264px" LinkProperty="OtherInfo"  SysTypeCode="String" LinkObject="ContactInfo" MaxLength="200"></cc1:DataTextBox>
            </td><td>你的特殊需求，200字</td>
        </tr>
    </table>
    </div>
       
    <p>
        &nbsp;
        <asp:Button ID="btnSubmit" runat="server"  Text="提交" Width="71px" OnClick="btnSubmit_Click" CssClass="btn btn-success" />&nbsp;
        <input type="reset" value ="重置" class="btn btn-info "/>
        <br />
<cc1:DataLabel ID="dlMsg" runat="server" DataFormatString="" IsNull="True" LinkObject="" LinkProperty="" MaxLength="0" PrimaryKey="False" ReadOnly="True" SysTypeCode="Empty">就绪，如果输入姓名和身份证号已经存在，则是修改信息</cc1:DataLabel>
    </p>
          </div>
        <div class ="row">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" EnableModelValidation="True" CssClass="table table-striped" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
            </div>

        </div>
   </div>
  </div>
        <div class="row">
            技术支持 <a href="http://www.pwmis.com/sqlmap" target="_blank"> PDF.NET SOD开源框架</a>

        </div>
</div>
          </form>
    </body>
</html>
