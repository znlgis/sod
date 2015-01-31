using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuperMarketWeb
{
    public partial class Site2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string NavigateMessage
        {
            get { return this.lblNavgate.Text; }
            set { this.lblNavgate.Text = value; }
        }
    }
}
