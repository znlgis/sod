using System;
using System.Web.UI;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.FacilityManage
{
    public partial class Index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (dtbSN.Text != "")
            {
                //保存数据
                var ibCommandList = MyWebForm.Instance.AutoUpdateIBFormData(Controls);
                lblMsg.Text = "保存成功！";
                //重新绑定数据
                ProPageToolBar1.ReBindResultData();
            }
            else
            {
                lblMsg.Text = "设备编号不能为空！";
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            //关键，必须为主键控件赋值
            //this.dtbSN 控件为可更新的主键，故不赋值
            //this.dtbSN.Text = GridView1.SelectedRow.Cells[1].Text;
            dlblSN.Text = GridView1.SelectedRow.Cells[1].Text;
            //填充数据
            MyWebForm.Instance.AutoSelectIBForm(Controls);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            MyWebForm.ClearIBData(Controls);
        }
    }
}