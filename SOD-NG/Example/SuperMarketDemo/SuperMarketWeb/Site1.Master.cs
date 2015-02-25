using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuperMarketWeb
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RawUrl!= "/LoginMarketMIS.aspx" && Session["Admin_Logon"] == null)
            {
                Response.Redirect("/LoginMarketMIS.aspx");
            }
        }
    }
}
