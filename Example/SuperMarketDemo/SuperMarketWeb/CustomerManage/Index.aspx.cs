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
using PWMIS.Common.DataMap;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.CustomerManage
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                this.tbCustomerInfo.Visible = false ;
                this.ProPageToolBar1.PageSize = 5;
                bindGrid();
                this.GridView1.DataKeyNames = new string[] { "CustomerID" };
            }
        }

        private void bindGrid()
        {
            CustomerManageBIZ biz = new CustomerManageBIZ();
            this.ProPageToolBar1.AllCount = biz.GetContactInfoCount();
            this.GridView1.DataSource = biz.GetContactInfoList(this.ProPageToolBar1.PageSize, 1, this.ProPageToolBar1.AllCount);
            this.GridView1.DataBind();
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
              result=  biz.UpdateContactInfo(info);
            }
            else
            {
                info.Integral = 10;//新建客户默认送10个积分
                result=  biz.AddContactInfo(info);
              dbtCID.ReadOnly = true;
            }
            lblMsg.Text = result ? "保存成功!" : "保存失败";
            bindGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            string customerId = GridView1.SelectedRow.Cells[1].Text;
            //调用业务层方法获取实体
            CustomerManageBIZ biz = new CustomerManageBIZ();
            CustomerContactInfo info = biz.GetCustomerContactInfo (customerId);
            //将实体与页面控件绑定
            WebControlDataMap.FillDataFromEntityClass(info,
                MyWebForm.GetIBControls(this.tbCustomerInfo.Controls)
                );
            //标记当前表单为编辑状态，主键控件不可编辑
            dbtCID.ReadOnly = true;
            this.tbCustomerInfo.Visible = true ;
            lblMsg.Text = "编辑客户联系信息后请保存！";
        }

        protected void ProPageToolBar1_PageChangeIndex(object sender, EventArgs e)
        {
            CustomerManageBIZ biz = new CustomerManageBIZ();
            this.ProPageToolBar1.AllCount = biz.GetContactInfoCount();
            this.GridView1.DataSource = biz.GetContactInfoList(
                this.ProPageToolBar1.PageSize, 
                this.ProPageToolBar1.CurrentPage, 
                this.ProPageToolBar1.AllCount);
            this.GridView1.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            dbtCID.ReadOnly = false ;
            MyWebForm.ClearIBData(this.Controls);
            this.tbCustomerInfo.Visible = true;
            lblMsg.Text = "请输入客户联系信息！(新建客户默认送10个积分)";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedIndex >= 0)
            {
                string customerId = GridView1.SelectedRow.Cells[1].Text;
                CustomerContactInfo info = new CustomerContactInfo() { CustomerID = customerId };

                CustomerManageBIZ biz = new CustomerManageBIZ();
                if (biz.RemoveContactInfo(info))
                {
                    this.tbCustomerInfo.Visible = false;
                    lblMsg.Text = "删除成功！";
                    bindGrid();
                }
            }
            else
            {
                lblMsg.Text = "请先选择一行有效的记录！";
            }
           
        }
    }
}
