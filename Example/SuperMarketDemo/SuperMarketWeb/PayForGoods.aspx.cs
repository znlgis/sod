using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;
using SuperMarketModel;
using SuperMarketModel.ViewModel;

namespace SuperMarketWeb
{
    public partial class PayForGoods : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SuperMarketBIZ.Instance.CashierConsole != null)
                {
                    this.GridView1.DataSource = SuperMarketBIZ.Instance.CashierConsole;
                    this.GridView1.DataBind();

                    btnOK.Enabled = false;
                    btnCancel.Enabled = false;
                }
            }
          
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                int length = SuperMarketBIZ.Instance.CashierConsole[index].QueueLength;
                lblQueue.Text = "当前收银台排队的顾客人数是：" + length;
            }
            ViewState["AddQueue"] = null;
        }

        /// <summary>
        /// 显示排队信息
        /// </summary>
        private void showQueueInfo()
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                CashierRegisterBIZ crBiz=SuperMarketBIZ.Instance.CashierConsole[index];
                if (ViewState["AddQueue"] != null && (bool)ViewState["AddQueue"])
                {
                    int length = crBiz.QueueLength;
                    string name = crBiz.CurrCashier.CashierName;

                    Customer customer = (Customer)Session["Curr_Customer"];
                    if (crBiz.Waite(customer))
                    {
                        lblQueue.Text = "当前收银台排队的顾客人数是：" + length;
                        
                    }
                    else
                    {
                        if (!btnOK.Enabled)
                        {
                            lblQueue.Text = "欢迎您，" + name + "为您服务，下面是您的购物价格信息，请确认后付款！";

                            //绑定购物车中的商品售价信息
                            gvSPCart.DataSource = crBiz.CurrCRManchines.GoodsSalePriceList;
                            gvSPCart.DataBind();

                            lblAmout.Text = crBiz.CurrCRManchines.GoodsSalePriceList.Sum(p => p.GoodsMoney).ToString();

                            btnOK.Enabled = true;
                            btnCancel.Enabled = true;
                            btnWaite.Enabled = false;
                        }
                       
                    }
                }
                else
                {
                    lblQueue.Text = "请在此收银台排队！";
                }
                
               
            }
        }

        protected void btnWaite_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                string scriptTemplate = @"
  <script type='text/javascript'>
        function AutoRefsh()
        {
         document.getElementById('@btnID').click();
        }
        window.setInterval(AutoRefsh, 5000);
    </script>
";
                if (!btnOK.Enabled)
                {
                    //同意支付的时候，页面不需要继续刷新
                    string script = scriptTemplate.Replace("@btnID", this.btnWaite.ClientID);
                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "autoRef", script);
                }
                else
                {
                    this.ClientScript.RegisterStartupScript(this.Page.GetType(), "autoRef", "");
                   
                }
               
                
                if (ViewState["AddQueue"] == null)
                {
                    Customer customer = (Customer)Session["Curr_Customer"];
                    SuperMarketBIZ.Instance.CashierConsole[index].AddQueue(customer);

                    ViewState["AddQueue"] = true;
                }

                showQueueInfo();
                
            }
            
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                SuperMarketBIZ.Instance.CashierConsole[index].Processing();
                //收款成功
                Response.Redirect("BuyGoodsOK.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                SuperMarketBIZ.Instance.CashierConsole[index].ExitQueue ((Customer)Session["Curr_Customer"]);
                //收款成功
                Response.Redirect("Index.aspx");
            }
        }

        protected void btnQuitBuy_Click(object sender, EventArgs e)
        {
            int index = GridView1.SelectedIndex;
            if (index != -1)
            {
                SuperMarketBIZ.Instance.CashierConsole[index].ExitQueue((Customer)Session["Curr_Customer"]);
                Session.Remove("Curr_Customer");
                Response.Redirect("Default.aspx");
            }
        }
    }
}
