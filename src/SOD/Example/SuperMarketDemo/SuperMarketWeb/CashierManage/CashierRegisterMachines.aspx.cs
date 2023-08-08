﻿using System;
using System.Web.UI;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.CashierManage
{
    public partial class CashierRegisterMachines : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //保存数据
            var ibCommandList = MyWebForm.Instance.AutoUpdateIBFormData(Controls);
            lblMsg.Text = "保存成功！";
            //重新绑定数据
            ProPageToolBar1.ReBindResultData();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            //关键，必须为主键控件赋值
            dtbSN.Text = GridView1.SelectedRow.Cells[1].Text;
            //填充数据
            MyWebForm.Instance.AutoSelectIBForm(Controls);
        }

        protected void btnNewSN_Click(object sender, EventArgs e)
        {
            dtbSN.Text = "CRM-" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
        }
    }
}