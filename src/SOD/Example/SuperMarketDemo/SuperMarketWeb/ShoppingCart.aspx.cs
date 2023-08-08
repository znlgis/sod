using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class ShoppingCart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Curr_Customer"] == null)
                Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
            {
                var customer = (Customer)Session["Curr_Customer"];
                GridView1.DataKeyNames = new[] { "GoodsID" };
                bindGrid(customer);
                lblWelcomeMsg.Text = string.Format("你好[{0}]，你的客户号是：{1}", customer.CustomerName, customer.CustomerID);

                ((Site2)Master).NavigateMessage = "我的购物车";
            }
        }

        private void bindGrid()
        {
            var customer = (Customer)Session["Curr_Customer"];
            GridView1.DataSource = customer.Goodss;
            GridView1.DataBind();
            lblAmout.Text = customer.GoodsAmount().ToString();
        }

        private void bindGrid(Customer customer)
        {
            GridView1.DataSource = customer.Goodss;
            GridView1.DataBind();
            lblAmout.Text = customer.GoodsAmount().ToString();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var goodsID = GridView1.DataKeys[e.RowIndex].Value.ToString();
            var customer = (Customer)Session["Curr_Customer"];
            customer.Goodss.RemoveAll(p => p.GoodsID == int.Parse(goodsID));
            bindGrid(customer);
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var goodsID = GridView1.DataKeys[e.RowIndex].Value.ToString();
            var strCount = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].Controls[0]).Text.Trim();
            var customer = (Customer)Session["Curr_Customer"];
            var goods = customer.Goodss.Where(p => p.GoodsID == int.Parse(goodsID)).FirstOrDefault();
            goods.GoodsNumber = int.Parse(strCount);
            GridView1.EditIndex = -1;
            bindGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            bindGrid();
        }
    }
}