using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;

namespace SuperMarketWeb.CashierManage
{
    public partial class CashierConsole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initPage();
            }
        }

        private void initPage()
        {
            if (Cache["CashRegisterNos"] == null)
            {
                Cache["CashRegisterNos"] = getCashRegisterNos();
            }

            CashierManageBIZ biz = new CashierManageBIZ();
            var list = biz.GetAllCashiers();
            this.GridView1.DataSource = list;
            this.GridView1.DataBind();
        }

        private List<string> getCashRegisterNos()
        {
            CashierManageBIZ biz = new CashierManageBIZ();
            var list = biz.GetAllCashierRegisterMachines();
            List<string> result = list.ConvertAll<string>(p => p.CashRegisterNo);
            return result;

        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            SuperMarketModel.Cashier cashier = new SuperMarketModel.Cashier();

            DropDownList ddlSN = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlSN.Parent.Parent;
            cashier.CashierName = row.Cells[1].Text;
            cashier.WorkNumber = row.Cells[2].Text ;
            string cashRegisterNo=ddlSN .SelectedValue ;
            if (cashRegisterNo != "请选择收银机号")
            {
                CashierManageBIZ biz = new CashierManageBIZ();
                if (biz.AssignCashier(cashRegisterNo, cashier))
                {
                    lblMsg.Text = "分派成功！";
                }
                else
                {
                    lblMsg.Text = "分配失败，收银机【" + cashRegisterNo + "】已经被使用！";
                    ddlSN.SelectedIndex = 0;
                }
            }
            

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> result = new List<string>();
                foreach (string str in (List<string>)Cache["CashRegisterNos"])
                {
                    result.Add(str);
                }
                result.Insert(0, "请选择收银机号");
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlSN");
                ddl.DataSource = result;
                ddl.DataBind();
                //选中当前收银员使用的收银机
                SuperMarketModel.Cashier cashier = e.Row.DataItem as SuperMarketModel.Cashier;
                if (cashier != null)
                {
                    CashierManageBIZ biz = new CashierManageBIZ();
                    if (biz.TestAssignedCashier(cashier))
                        ddl.SelectedValue = cashier.UsingCashierRegister.CashRegisterNo;
                }
            }
        }

        protected void btnRef_Click(object sender, EventArgs e)
        {
            Cache.Remove("CashRegisterNos");
            initPage();
        }
    }
}
