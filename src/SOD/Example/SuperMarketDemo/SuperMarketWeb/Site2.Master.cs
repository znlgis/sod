using System;
using System.Web.UI;

namespace SuperMarketWeb
{
    public partial class Site2 : MasterPage
    {
        public string NavigateMessage
        {
            get => lblNavgate.Text;
            set => lblNavgate.Text = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}