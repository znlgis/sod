using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.CashierManage
{
    public partial class Cashier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CashierManageBIZ biz = new CashierManageBIZ();
                var list= biz.GetAllCashiers();
                this.GridView1.DataSource = list;
                this.GridView1.DataBind();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtbWorkNumber.Text  = this.GridView1.SelectedRow.Cells[2].Text;
            MyWebForm.Instance.AutoSelectIBForm(this.Controls);
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
                MyWebForm.Instance.AutoUpdateIBFormData(this.Controls);
                lblMsg.Text = "修改成功！";
            }
            
        }
    }
}
