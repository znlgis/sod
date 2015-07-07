using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.EmployeeManage
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dtbWorkNumber.Text == "")
            {
                lblMsg.Text = "请输入工号！";
                return;
            }
            if (!this.rdbSexMan.Checked && !this.rdbSexWomen.Checked)
            {
                lblMsg.Text = "请选择性别！";
            }
            else
            {
                //保存数据
                bool result = MyWebForm.Instance.AutoUpdateIBFormData(this.Controls, this.dlbWorkNumber);
                if (result)
                {
                    lblMsg.Text = "保存成功！";
                    //重新绑定数据
                    this.ProPageToolBar1.ReBindResultData();
                }
                else
                {
                    lblMsg.Text = "保存失败！[数据库操作异常，详细信息请检查SQL日志]";
                }
            }
            
            
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            //关键，必须为主键控件赋值
            dlbWorkNumber.Text = GridView1.SelectedRow.Cells[1].Text;
            //填充数据
            MyWebForm.Instance.AutoSelectIBForm(this.Controls);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            MyWebForm.ClearIBData(this.Controls);
        }
    }
}
