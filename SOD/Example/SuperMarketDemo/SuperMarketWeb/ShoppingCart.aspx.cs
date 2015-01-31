using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketModel;

namespace SuperMarketWeb
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Curr_Customer"] == null)
                Response.Redirect("~/Default.aspx");

            if (!IsPostBack)
            {
                Customer customer = (Customer)Session["Curr_Customer"];
                this.GridView1.DataKeyNames = new string[] { "GoodsID" };
                bindGrid(customer);
                this.lblWelcomeMsg.Text = string.Format("你好[{0}]，你的客户号是：{1}", customer.CustomerName, customer.CustomerID);

                ((Site2)this.Master).NavigateMessage = "我的购物车"; 
            }
          
        }

        private void bindGrid()
        {
            Customer customer = (Customer)Session["Curr_Customer"];
            this.GridView1.DataSource = customer.Goodss;
            this.GridView1.DataBind();
            this.lblAmout.Text = customer.GoodsAmount().ToString ();
        }

        private void bindGrid(Customer customer)
        {
            this.GridView1.DataSource = customer.Goodss;
            this.GridView1.DataBind();
            this.lblAmout.Text = customer.GoodsAmount().ToString();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string goodsID = GridView1.DataKeys[e.RowIndex].Value.ToString();
            Customer customer = (Customer)Session["Curr_Customer"];
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
            string goodsID = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string strCount = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].Controls[0]).Text.ToString().Trim();
            Customer customer = (Customer)Session["Curr_Customer"];
            Goods goods = customer.Goodss.Where(p => p.GoodsID == int.Parse(goodsID)).FirstOrDefault ();
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
