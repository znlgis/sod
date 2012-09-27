using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuperMarketWeb
{
    public partial class LoginMarketMIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.txtLoginName.Text == "SuperMarket" && this.txtLoginPwd.Text  == "PDF.NET")
            {
                Session["Admin_Logon"] = true; 
                Response.Redirect("ManagerIndex.aspx");
            }
            else
            {
                this.RegisterStartupScript("alert", "<script>alert('登录失败，用户名或者密码不正确！');</script>");
            }
        }
    }
}
