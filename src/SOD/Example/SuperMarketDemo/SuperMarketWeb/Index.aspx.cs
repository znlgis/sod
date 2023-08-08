using System;
using System.Web.UI;
using SuperMarketBLL;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class Index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SuperMarketBIZ.Instance.InBusiness || Session["Curr_Customer"] == null)
                Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
            {
                var biz = new GoodsManageBIZ();
                GridView1.DataKeyNames = new[] { "GoodsID" };
                GridView1.DataSource = biz.GetGoodsSaleInfo();
                GridView1.DataBind();

                var customer = (Customer)Session["Curr_Customer"];
                lblWelcomeMsg.Text = string.Format("你好[{0}]，你的客户号是：{1}", customer.CustomerName, customer.CustomerID);

                btnBuy.Enabled = false;
                btnEditBuyCount.Enabled = false;
                lblGoodsCount.Text = customer.Goodss.Count.ToString();

                ((Site2)Master).NavigateMessage = "今日商品信息";
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var goodsID = GridView1.DataKeys[GridView1.SelectedRow.RowIndex].Value.ToString();
            var strPrice = GridView1.SelectedRow.Cells[5].Text;
            txtBuyCount.Text = "1";
            lblBuyPrice.Text = strPrice;

            ViewState["BuyGoodsID"] = goodsID;
            ViewState["BuyOnePrice"] = strPrice;
            ViewState["BuyGoodsName"] = GridView1.SelectedRow.Cells[3].Text;
            ViewState["SerialNumber"] = GridView1.SelectedRow.Cells[4].Text;

            var stocks = int.Parse(GridView1.SelectedRow.Cells[9].Text);
            ViewState["Stocks"] = stocks;
            btnBuy.Enabled = stocks > 0;
            btnEditBuyCount.Enabled = true;
        }

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            var goods = new Goods();
            goods.GoodsID = int.Parse(ViewState["BuyGoodsID"].ToString());
            goods.GoodsName = ViewState["BuyGoodsName"].ToString();
            goods.SerialNumber = ViewState["SerialNumber"].ToString();
            goods.GoodsPrice = decimal.Parse(ViewState["BuyOnePrice"].ToString());
            goods.GoodsNumber = int.Parse(txtBuyCount.Text);


            var customer = (Customer)Session["Curr_Customer"];
            customer.LikeBuy(goods);
            lblGoodsCount.Text = customer.Goodss.Count.ToString();
        }

        protected void btnEditBuyCount_Click(object sender, EventArgs e)
        {
            var count = 0;
            if (int.TryParse(txtBuyCount.Text, out count))
            {
                var stocks = (int)ViewState["Stocks"];
                var price = decimal.Parse(ViewState["BuyOnePrice"].ToString());
                if (count <= stocks && count > 0)
                {
                    var allPrice = count * price;

                    lblBuyPrice.Text = allPrice.ToString();
                }
                else
                {
                    txtBuyCount.Text = "1";
                    lblBuyPrice.Text = price.ToString();
                }
            }
            else
            {
                txtBuyCount.Text = "1";
            }
        }
    }
}