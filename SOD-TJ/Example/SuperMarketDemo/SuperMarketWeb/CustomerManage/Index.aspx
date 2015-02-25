<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"  CodeBehind="Index.aspx.cs" Inherits="SuperMarketWeb.CustomerManage.Index" %>
<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style="background-color:#DEBA84;border-color:#DEBA84;border-width:1px;border-style:None;" id="tbCustomerInfo" runat="server">
       <tr style="color:#8C4510;background-color:#FFF7E7;">
<td >[客户号]*</td><td >
           <cc1:DataTextBox ID="dbtCID" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="CustomerID" PrimaryKey="True" Width="256px" 
               SysTypeCode="String" ScriptPath="/System/WebControls/script.js" 
               isNull="False" OftenType="身份证 15位|18位" ErrorMessage="客户号必须为身份证号" 
               MessageType="层" onclickshowinfo="True" RegexName="客户号"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[姓名]</td><td >
           <cc1:DataTextBox ID="dtbCustomerName" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="CustomerName" Width="256px" SysTypeCode="String" 
               ErrorMessage="姓名不能为空" isNull="False" OftenType="无"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >[性别]</td><td >
&nbsp;<cc1:DataRadioButton ID="DataRadioButton1" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Sex" Text="男" SysTypeCode="Boolean" Value="True" 
               GroupName="Sex" />
                                <cc1:DataRadioButton ID="DataRadioButton2" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Sex" Text="女" SysTypeCode="Boolean" Value="False" 
               GroupName="Sex" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[出生日期]</td><td >
           <cc1:DataCalendar ID="DataCalendar2" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Birthday" ScriptPath="../Calendar/" ReadOnly="False" />
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[联系电话]</td><td >
           <cc1:DataTextBox ID="DataTextBox5" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="PhoneNumber" SysTypeCode="String" ErrorMessage="联系电话填写不正确" 
               OftenType="电话号码或手机" onclickshowinfo="True" RegexName="电话号码"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td>[联系地址]</td><td >
           <cc1:DataTextBox ID="DataTextBox6" runat="server" LinkObject="CustomerContactInfo" 
               LinkProperty="Address" Width="256px" SysTypeCode="String"></cc1:DataTextBox>
           </td></tr>
       <tr style="color:#8C4510;background-color:#FFF7E7;">
       <td >&nbsp;</td><td >
           &nbsp;<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="保存" />
           &nbsp;
           
           </td></tr>
       </table>
       <p>
       
           <asp:Button ID="btnNew" runat="server" onclick="btnNew_Click" Text="新建客户" />&nbsp;
           <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
               Text="删除客户" OnClientClick="return confirm('你确认要删吗？')" />
&nbsp;
           <input type="button" value ="刷新" onclick ="window.location.href=window.location.href" />
           <br />
           <asp:Label ID="lblMsg" runat="server" Text="操作"></asp:Label>
       
       </p>
       
       <asp:GridView ID="GridView1" runat="server" BackColor="#DEBA84" 
        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
        CellSpacing="2" onselectedindexchanged="GridView1_SelectedIndexChanged" 
        AutoGenerateColumns="False" Width="821px">
        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:ButtonField CommandName="Select" HeaderText="选择" ShowHeader="True" 
                Text="编辑" />
            <asp:BoundField DataField="CustomerID" HeaderText="客户号" />
            <asp:BoundField DataField="CustomerName" HeaderText="姓名" />
            <asp:TemplateField HeaderText="性别">
              
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Sex").ToString()=="True"?"男":"女" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Birthday" DataFormatString="{0:d}" 
                HeaderText="出生日期" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="联系电话" />
            <asp:BoundField DataField="Address" HeaderText="联系地址" />
            <asp:BoundField DataField="Integral" HeaderText="积分" />
        </Columns>
    </asp:GridView>
    
   <cc1:ProPageToolBar ID="ProPageToolBar1" runat="server" BackColor="#FFF7E7" 
        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#8C4510" 
        onpagechangeindex="ProPageToolBar1_PageChangeIndex" PageSize="5" />

</asp:Content>
