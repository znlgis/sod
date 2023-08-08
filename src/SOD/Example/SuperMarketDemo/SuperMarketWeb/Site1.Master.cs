using System;
using System.Web.UI;

namespace SuperMarketWeb
{
    public partial class Site1 : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RawUrl != "/LoginMarketMIS.aspx" && Session["Admin_Logon"] == null)
                Response.Redirect("/LoginMarketMIS.aspx");
        }
    }
}