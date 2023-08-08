using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SuperMarketBLL;

namespace SuperMarketWeb.CashierManage
{
    public partial class CashierConsole : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) initPage();
        }

        private void initPage()
        {
            if (Cache["CashRegisterNos"] == null) Cache["CashRegisterNos"] = getCashRegisterNos();

            var biz = new CashierManageBIZ();
            var list = biz.GetAllCashiers();
            GridView1.DataSource = list;
            GridView1.DataBind();
        }

        private List<string> getCashRegisterNos()
        {
            var biz = new CashierManageBIZ();
            var list = biz.GetAllCashierRegisterMachines();
            var result = list.ConvertAll(p => p.CashRegisterNo);
            return result;
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            var cashier = new SuperMarketModel.Cashier();

            var ddlSN = (DropDownList)sender;
            var row = (GridViewRow)ddlSN.Parent.Parent;
            cashier.CashierName = row.Cells[1].Text;
            cashier.WorkNumber = row.Cells[2].Text;
            var cashRegisterNo = ddlSN.SelectedValue;
            if (cashRegisterNo != "请选择收银机号")
            {
                var biz = new CashierManageBIZ();
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
                var result = new List<string>();
                foreach (var str in (List<string>)Cache["CashRegisterNos"]) result.Add(str);
                result.Insert(0, "请选择收银机号");
                var ddl = (DropDownList)e.Row.FindControl("ddlSN");
                ddl.DataSource = result;
                ddl.DataBind();
                //选中当前收银员使用的收银机
                var cashier = e.Row.DataItem as SuperMarketModel.Cashier;
                if (cashier != null)
                {
                    var biz = new CashierManageBIZ();
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