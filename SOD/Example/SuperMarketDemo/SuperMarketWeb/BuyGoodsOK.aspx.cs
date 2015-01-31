using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class BuyGoodsOK : System.Web.UI.Page
    {
        public string SellNoteMessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Customer customer = (Customer)Session["Curr_Customer"];
            txtSellNote.Text  = customer.SalesNote;
            this.lblWelcomeMsg.Text = string.Format("你好[{0}]，购物成功，感谢您的本次光临！", customer.CustomerName);
        }
    }
}
