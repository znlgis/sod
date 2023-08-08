using System;
using System.Web.UI;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class BuyGoodsOK : Page
    {
        public string SellNoteMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var customer = (Customer)Session["Curr_Customer"];
            txtSellNote.Text = customer.SalesNote;
            lblWelcomeMsg.Text = string.Format("你好[{0}]，购物成功，感谢您的本次光临！", customer.CustomerName);
        }
    }
}