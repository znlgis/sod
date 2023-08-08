using System;
using System.Web.UI;

namespace SuperMarketWeb
{
    public partial class LoginMarketMIS : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginName.Text == "SuperMarket" && txtLoginPwd.Text == "PDF.NET")
            {
                Session["Admin_Logon"] = true;
                Response.Redirect("ManagerIndex.aspx");
            }
            else
            {
                RegisterStartupScript("alert", "<script>alert('登录失败，用户名或者密码不正确！');</script>");
            }
        }
    }
}