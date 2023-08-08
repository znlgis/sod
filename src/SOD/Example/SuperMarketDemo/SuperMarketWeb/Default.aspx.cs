using System;
using System.Web.UI;
using PWMIS.DataForms.Adapter;
using PWMIS.DataMap;
using PWMIS.DataMap.Entity;
using SuperMarketBLL;
using SuperMarketDAL.Entitys;
using SuperMarketModel;
using GoodsStock = SuperMarketDAL.Entitys.GoodsStock;

namespace SuperMarketWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetStockCount("TM002");

            if (!IsPostBack) tbCustomerInfo.Visible = false;
            try
            {
                if (!SuperMarketBIZ.Instance.InBusiness)
                    if (!SuperMarketBIZ.Instance.StartBusiness())
                        lblMsg.Text = SuperMarketBIZ.Instance.Message;
            }
            catch (Exception ex)
            {
                var errmsg = ex.Message;
                Session["errmsg"] = errmsg;
                Response.Redirect("Setup/CreateAppTables.aspx");
            }
        }

        //测试OQL的统计功能
        private int GetStockCount(string serialNumber)
        {
            var stock = new GoodsStock();
            stock.SerialNumber = serialNumber;
            var q = new OQL(stock);
            q.Select()
                .Count(stock.Stocks, "")
                .Where(stock.SerialNumber);

            stock = EntityQuery<GoodsStock>.QueryObject(q);
            var stockCount = stock.Stocks;
            return stockCount;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var customer = new Customer();
            Session["Curr_Customer"] = customer;
            Response.Redirect("Index.aspx");
        }

        protected void btnComeIn_Click(object sender, EventArgs e)
        {
            var customerID = txtComeIn.Text;
            var biz = new CustomerManageBIZ();
            var customer = biz.GetRegistedCustomer(customerID);
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
            var cdm = new ControlDataMap();
            //收集数据到实体对象
            var info = cdm.CollectDataToObject<CustomerContactInfo>(
                MyWebForm.GetIBControls(tbCustomerInfo.Controls)
            );

            //调用业务类，保存数据
            var result = false;
            var biz = new CustomerManageBIZ();
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
            tbCustomerInfo.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbCustomerInfo.Visible = false;
        }
    }
}