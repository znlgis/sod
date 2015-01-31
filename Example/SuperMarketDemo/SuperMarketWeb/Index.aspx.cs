using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!SuperMarketBIZ.Instance.InBusiness  || Session["Curr_Customer"] == null)
                Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
            {
                GoodsManageBIZ biz = new GoodsManageBIZ();
                this.GridView1.DataKeyNames = new string[] { "GoodsID" };   
                this.GridView1.DataSource = biz.GetGoodsSaleInfo();
                this.GridView1.DataBind();

                Customer customer = (Customer)Session["Curr_Customer"];
                this.lblWelcomeMsg.Text = string.Format("你好[{0}]，你的客户号是：{1}", customer.CustomerName, customer.CustomerID);

                btnBuy.Enabled = false;
                btnEditBuyCount.Enabled = false;
                lblGoodsCount.Text = customer.Goodss.Count.ToString();

                ((Site2)this.Master).NavigateMessage = "今日商品信息";

                
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string goodsID = GridView1.DataKeys[GridView1.SelectedRow.RowIndex].Value.ToString () ;
            string strPrice = GridView1.SelectedRow.Cells[5].Text;
            txtBuyCount.Text = "1";
            lblBuyPrice.Text  = strPrice;

            ViewState["BuyGoodsID"] = goodsID;
            ViewState["BuyOnePrice"] = strPrice;
            ViewState["BuyGoodsName"] = GridView1.SelectedRow.Cells[3].Text;
            ViewState["SerialNumber"] = GridView1.SelectedRow.Cells[4].Text;

            int stocks = int.Parse(GridView1.SelectedRow.Cells[8].Text);
            ViewState["Stocks"] = stocks;
            btnBuy.Enabled = stocks > 0;
            btnEditBuyCount.Enabled = true;
  
        }

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            Goods goods = new Goods();
            goods.GoodsID = int.Parse(ViewState["BuyGoodsID"].ToString ());
            goods.GoodsName = ViewState["BuyGoodsName"].ToString();
            goods.SerialNumber = ViewState["SerialNumber"].ToString();
            goods.GoodsPrice = decimal.Parse(ViewState["BuyOnePrice"].ToString());
            goods.GoodsNumber =int.Parse( this.txtBuyCount.Text);


            Customer customer = (Customer)Session["Curr_Customer"];
            customer.LikeBuy(goods);
            lblGoodsCount.Text = customer.Goodss.Count.ToString();
        }

        protected void btnEditBuyCount_Click(object sender, EventArgs e)
        {
            int count=0;
            if (int.TryParse(this.txtBuyCount.Text, out count))
            {
                int stocks = (int)ViewState["Stocks"];
                decimal price = decimal.Parse(ViewState["BuyOnePrice"].ToString());
                if (count <= stocks && count >0)
                {
                   
                    decimal allPrice = count * price;

                    lblBuyPrice.Text = allPrice.ToString();
                }
                else
                {
                    this.txtBuyCount.Text = "1";
                    lblBuyPrice.Text = price.ToString();
                }
                
            }
            else
            {
                this.txtBuyCount.Text  = "1";
            }
        }

       

       
    }
}
