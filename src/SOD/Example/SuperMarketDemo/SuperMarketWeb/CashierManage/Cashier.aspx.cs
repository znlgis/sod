using System;
using System.Web.UI;
using PWMIS.DataForms.Adapter;
using SuperMarketBLL;

namespace SuperMarketWeb.CashierManage
{
    public partial class Cashier : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var biz = new CashierManageBIZ();
                var list = biz.GetAllCashiers();
                GridView1.DataSource = list;
                GridView1.DataBind();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtbWorkNumber.Text = GridView1.SelectedRow.Cells[2].Text;
            MyWebForm.Instance.AutoSelectIBForm(Controls);
            lblMsg.Text = "修改后请保存！";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (dtbWorkNumber.Text == "")
            {
                lblMsg.Text = "请先选择一条记录进行修改（如果没记录请先进行[雇员管理]）！";
            }
            else
            {
                MyWebForm.Instance.AutoUpdateIBFormData(Controls);
                lblMsg.Text = "修改成功！";
            }
        }
    }
}