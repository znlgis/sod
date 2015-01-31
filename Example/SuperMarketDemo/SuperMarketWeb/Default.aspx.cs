using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;
using PWMIS.DataMap;
using SuperMarketDAL.Entitys;
using PWMIS.DataProvider.Adapter;
using SuperMarketModel;
using PWMIS.DataMap.Entity;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetStockCount("TM002");
          
            if (!IsPostBack)
            {
                this.tbCustomerInfo.Visible = false;
            }
            try
            {
                if (!SuperMarketBIZ.Instance.InBusiness)
                {
                    if (!SuperMarketBIZ.Instance.StartBusiness())
                    {
                        this.lblMsg.Text = SuperMarketBIZ.Instance.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                Session["errmsg"] = errmsg;
                Response.Redirect("Setup/CreateAppTables.aspx");
            }
            
           


        }

        //测试OQL的统计功能
        private int GetStockCount(string serialNumber)
        {
            SuperMarketDAL.Entitys.GoodsStock stock = new SuperMarketDAL.Entitys.GoodsStock();
            stock.SerialNumber = serialNumber;
            OQL q = new OQL(stock);
            q.Select()
                .Count(stock.Stocks, "")
                .Where(stock.SerialNumber);

            stock = EntityQuery<SuperMarketDAL.Entitys.GoodsStock>.QueryObject(q);
            int stockCount = stock.Stocks;
            return stockCount;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            Session["Curr_Customer"] = customer;
            Response.Redirect("Index.aspx");
        }

        protected void btnComeIn_Click(object sender, EventArgs e)
        {

            string customerID = txtComeIn.Text;
            CustomerManageBIZ biz = new CustomerManageBIZ();
            Customer customer = biz.GetRegistedCustomer(customerID);
            if (customer != null)
            {
                Session["Curr_Customer"] = customer;
                Response.Redirect("Index.aspx");
            }
            else
            {
                lblMsg.Text = "对不起，您输入的客户号不存在，您也可以使用该客户号注册一个会员账号。";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //实例化一个控件数据映射对象
            ControlDataMap cdm = new ControlDataMap();
            //收集数据到实体对象
            CustomerContactInfo info = cdm.CollectDataToObject<CustomerContactInfo>(
                MyWebForm.GetIBControls(this.tbCustomerInfo.Controls)
                );

            //调用业务类，保存数据
            bool result = false;
            CustomerManageBIZ biz = new CustomerManageBIZ();
            if (dbtCID.ReadOnly)
            {
                //这里规定主键对应的控件是只读状态，表示当前表单是修改状态，否则是新增状态
                result = biz.UpdateContactInfo(info);
            }
            else
            {
                result = biz.AddContactInfo(info);
                dbtCID.ReadOnly = true;
                txtComeIn.Text = dbtCID.Text;
            }
            lblMsg.Text = result ? "保存成功!请进入本超市！(也可以继续修改你的个人信息)" : "保存失败";
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            this.tbCustomerInfo.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.tbCustomerInfo.Visible = false ;
        }

      
    }
}
