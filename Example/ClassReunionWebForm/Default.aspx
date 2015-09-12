<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClassReunionWeb._Default" %>

<%@ Register assembly="PWMIS.Web" namespace="PWMIS.Web.Controls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"  />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title></title>
   
    
    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="http://cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

</head>
<body>
    
 <div class="container-fluid">
  <div class="jumbotron">
  <h1>同学们, 大家好!</h1>
  <p>今年时值<span class="label label-info"><%=this.SchoolName %></span>毕业<%=this.SchoolYear %>周年了，<%=this.Head_Holidays %>间有些在外地的同学可以回来，
看能不能搞一个同学聚会庆祝下，愿意参加的同学，请网上填报下表格；不能来的
同学，也请奔走相告，捎个信儿：）</p>
      <p>初步计划时间：<%=this.Head_ReunionDate %>，活动内容：<%=this.Head_ReunionNote %>。
    网上报名地址：<%=this.Head_reg_link %> ( <%=this.Head_reg_memo %> ，该网址手机也可以访问。)
</p>
     
      <h2>“大家都是一个班级”，<%=this.SchoolYear %>周年同学会，期待您的参与和支持！</h2>
      <p> 2015.9月</p>
</div>
  <div class="row-fluid">
      <form id="form1" runat="server">
   <div class="col-md-3">
     <!--Sidebar content-->
        <div class="row">
          <div class="col-sm-6 col-md-12">
            <div class="thumbnail">
              <img src="images/95_20.png" alt="风雨20年">
              <div class="caption">
                <h3><%=this.SchoolName %></h3>
                <p>风雨<%=this.SchoolYear %>年，难忘同窗情...</p>
                
              </div>
            </div>
          </div>
        </div>
       <div class="panel panel-info">
          <!-- Default panel contents -->
          <div class="panel-heading">同学会议程</div>
          <div class="panel-body">
              <div class="list-group">
              <a href="#" class="list-group-item list-group-item-success">发起倡议--2015春节</a>
              <a href="#" class="list-group-item list-group-item-success">网站上线--9.11</a>
              <a href="#" class="list-group-item list-group-item-info active">网上报名</a>
              <a href="#" class="list-group-item list-group-item-info">成立筹备委员会</a>
              <a href="#" class="list-group-item list-group-item-info">现场筹备</a>
              <a href="#" class="list-group-item list-group-item-info">会后纪念</a>
            </div>
          </div>
      
        </div>
      <div class="row">
          <div class="col-sm-5 col-md-12">
            <div class="thumbnail">
              <img src="http://www.pwmis.com/sqlmap/alipay-jk.png" alt="风雨20年">
              <div class="caption">
                <h3>土豪，我们交个朋友吧！</h3>
                <p>如果有哪位土豪（不限于）同学原意捐助本次同学会，请用手机扫描此二维码，我们将在同学会上为你打广告：）</p>
                <p>捐助名单：
                    <br />虚位以待......
                </p>
              </div>
            </div>
          </div>
        </div>
    </div>
    <div class="col-md-9">
      <!--Body content-->
          <div class="row-fluid">
              <div class="panel panel-info">
                  <!-- Default panel contents -->
                  <div class="panel-heading"><h4>同学会联系信息</h4></div>
                  <div class="panel-body">
                    <p>同学会具体组织形式，可以在“其它说明”中简短说明，或者在QQ群讨论；</p>
                    <p> 填写本联系表<span class="label label-info">无需注册，无需登录</span>，所以请填写<span class="label label-primary">“身份ID”</span>，当密码使用，用于修改数据和其它功能，
                        请使用你常用不会遗忘的数字，建议使用身份证号，这样如果您需要联系住宿或者预定车票的话，方便为您服务。</p>
                      <p>
                          <cc1:DataLabel ID="dlMsg" runat="server" CssClass="label label-info" DataFormatString="" IsNull="True" LinkObject="" LinkProperty="" MaxLength="0" PrimaryKey="False" ReadOnly="True" SysTypeCode="Empty">就绪，如果输入姓名和身份证号已经存在，则是修改信息</cc1:DataLabel>
                          </p>
                  </div>

                  <!-- Table -->
                  <table class="table table-striped table-bordered">
        <tr >
            <td>姓名</td><td >
            <cc1:DataTextBox ID="dtName" runat="server" CssClass="form-control" LinkProperty="Name" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="10" ErrorMessage="请输入汉字格式的姓名！" IsNull="False" MessageType="提示框" OftenType="中文字符" OnClickShowInfo="True" RegexName="姓名" ></cc1:DataTextBox>
            </td><td>请填写同学实名，修改信息需要</td>
        </tr>
        <tr>
            <td >身份ID</td><td >
            <cc1:DataTextBox ID="dtPersonID" runat="server" CssClass="form-control" LinkProperty="PersonID" SysTypeCode="String" LinkObject="ContactInfo" MaxLength="20"></cc1:DataTextBox>
            </td><td class="auto-style2">请填写，修改信息需要，6-18位数字</td>
        </tr>
        <tr>
            <td >原班级</td><td >
            <cc1:DataTextBox ID="dtClassNum" runat="server" CssClass="form-control" LinkProperty="ClassNum" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="10"></cc1:DataTextBox>
            </td><td >必填，如： 一班</td>
        </tr>
        <tr>
            <td >联系电话</td><td >
            <cc1:DataTextBox ID="dtPhone" runat="server" CssClass="form-control" LinkProperty="ContactPhone" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="20" ErrorMessage="联系电话填写不正确" IsNull="False" MessageType="提示框" OftenType="电话号码或手机" OnClickShowInfo="True" RegexName="联系电话"></cc1:DataTextBox>
            </td><td >必填，建议手机</td>
        </tr>
        <tr>
            <td>家属人数</td><td >
            <cc1:DataTextBox ID="DataTextBox1" runat="server" CssClass="form-control" LinkProperty="HomeMemberCount" LinkObject="ContactInfo" SysTypeCode="Int32" MaxLength="5"></cc1:DataTextBox>
            </td><td>没有，请写0</td>
        </tr>
        <tr>
            <td>是否预定房间</td><td >
            <cc1:DataCheckBox ID="DataCheckBox1" runat="server" CssClass="form-control-static" Text="预定" LinkProperty="NeedRoom" LinkObject="ContactInfo" SysTypeCode="Boolean"/>
            </td><td>我们将为你安排住宿</td>
        </tr>
        <tr>
            <td>出发地</td><td >
            <cc1:DataTextBox ID="DataTextBox4" runat="server" CssClass="form-control" LinkProperty="ComeFrom" LinkObject="ContactInfo" SysTypeCode="String" MaxLength="50"></cc1:DataTextBox>
            </td><td>比如来自北京</td>
        </tr>
        <tr>
            <td>其它说明</td><td >
            <cc1:DataTextBox ID="DataTextBox5" runat="server" CssClass="form-control" Height="65px" TextMode="MultiLine"  LinkProperty="OtherInfo"  SysTypeCode="String" LinkObject="ContactInfo" MaxLength="200"></cc1:DataTextBox>
            </td><td>你的特殊需求，200字</td>
        </tr>
    </table>
                   <div class="panel-footer">
                    <asp:Button ID="btnSubmit" runat="server"  Text="提交" Width="71px" OnClick="btnSubmit_Click" CssClass="btn btn-success" />&nbsp;
                    <input type="reset" value ="重置" class="btn btn-info "/>
                 </div>
            </div>

              <div class="panel panel-success">
              <!-- Default panel contents -->
              <div class="panel-heading"><h4>已报名的同学</h4></div>
           <div class="panel-body">
               <% if(this.MemberCount>0) { %>
               感谢下列同学报名支持本次活动！
                   <button class="btn btn-success" type="button">
                          报名人数 <span class="badge"><%= this.MemberCount %></span>
                    </button>
               <% }else { %>
               期待您的参与！
               <% } %>
            </div>
              <!-- Table -->
               <asp:GridView ID="GridView1" runat="server" CellPadding="4" EnableModelValidation="True" CssClass="table table-striped" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>    
                  <div class="panel-footer">
                      技术支持 <a href="http://www.pwmis.com/sqlmap" target="_blank"> PDF.NET SOD开源框架</a>
                  </div> 
            </div>
        </div>
          <div >
              
        </div>
   </div>
      </form>
  </div>
  
</div>
          
    </body>
</html>
