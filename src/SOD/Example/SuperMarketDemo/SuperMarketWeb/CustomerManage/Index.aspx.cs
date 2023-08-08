using System;
using System.Web.UI;
using PWMIS.Common.DataMap;
using PWMIS.DataForms.Adapter;
using PWMIS.DataMap;
using SuperMarketBLL;
using SuperMarketDAL.Entitys;

namespace SuperMarketWeb.CustomerManage
{
    public partial class Index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbCustomerInfo.Visible = false;
                ProPageToolBar1.PageSize = 5;
                bindGrid();
                GridView1.DataKeyNames = new[] { "CustomerID" };
            }
        }

        private void bindGrid()
        {
            var biz = new CustomerManageBIZ();
            ProPageToolBar1.AllCount = biz.GetContactInfoCount();
            GridView1.DataSource = biz.GetContactInfoList(ProPageToolBar1.PageSize, 1, ProPageToolBar1.AllCount);
            GridView1.DataBind();
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
                info.Integral = 10; //新建客户默认送10个积分
                result = biz.AddContactInfo(info);
                dbtCID.ReadOnly = true;
            }

            lblMsg.Text = result ? "保存成功!" : "保存失败";
            bindGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            var customerId = GridView1.SelectedRow.Cells[1].Text;
            //调用业务层方法获取实体
            var biz = new CustomerManageBIZ();
            var info = biz.GetCustomerContactInfo(customerId);
            //将实体与页面控件绑定
            WebControlDataMap.FillDataFromEntityClass(info,
                MyWebForm.GetIBControls(tbCustomerInfo.Controls)
            );
            //标记当前表单为编辑状态，主键控件不可编辑
            dbtCID.ReadOnly = true;
            tbCustomerInfo.Visible = true;
            lblMsg.Text = "编辑客户联系信息后请保存！";
        }

        protected void ProPageToolBar1_PageChangeIndex(object sender, EventArgs e)
        {
            var biz = new CustomerManageBIZ();
            ProPageToolBar1.AllCount = biz.GetContactInfoCount();
            GridView1.DataSource = biz.GetContactInfoList(
                ProPageToolBar1.PageSize,
                ProPageToolBar1.CurrentPage,
                ProPageToolBar1.AllCount);
            GridView1.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            dbtCID.ReadOnly = false;
            MyWebForm.ClearIBData(Controls);
            tbCustomerInfo.Visible = true;
            lblMsg.Text = "请输入客户联系信息！(新建客户默认送10个积分)";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedIndex >= 0)
            {
                var customerId = GridView1.SelectedRow.Cells[1].Text;
                var info = new CustomerContactInfo { CustomerID = customerId };

                var biz = new CustomerManageBIZ();
                if (biz.RemoveContactInfo(info))
                {
                    tbCustomerInfo.Visible = false;
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