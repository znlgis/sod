using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.DataProvider.Adapter;
using SuperMarketBLL;
using SuperMarketModel.ViewModel;
using SuperMarketDAL.Entitys;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.GoodsManage
{
    public partial class StockInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                GoodsManageBIZ biz = new GoodsManageBIZ();
               
                List<string> nameList = biz.GetAllGoodsNames();
                nameList.Insert(0, "请选择");
                this.ddlGoodsNames.DataSource = nameList;
                this.ddlGoodsNames.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.dtbSN.Text = this.ddlManufacturer.SelectedValue; 
            //保存数据
            List<IBCommand> ibCommandList = MyWebForm.Instance.AutoUpdateIBFormData(this.Controls);
            //获取插入的ID
            if (dlCHJLH.Text == "")
            {
                if (ibCommandList.Count > 0)
                {
                    IBCommand command = ibCommandList[0];
                    dlCHJLH.Text = command.InsertedID.ToString();
                }
            }//end if
            lblMsg.Text = "保存成功！";
            //重新绑定数据
            this.ProPageToolBar1.ReBindResultData();
        }

      
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            //关键，必须为主键控件赋值
            this.dlCHJLH.Text = GridView1.SelectedRow.Cells[1].Text;
            //填充数据
            MyWebForm.Instance.AutoSelectIBForm(this.Controls);

            //设置下拉框的选项
            //得到当前条码号
            string sn = this.dtbSN.Text;
            GoodsManageBIZ biz = new GoodsManageBIZ();
            GoodsBaseInfo info = biz.GetGoodsBaseInfo(sn);
            
            this.ddlGoodsNames.Text = info.GoodsName;

            List<GoodsBaseInfoVM> list = biz.GetGoodsBaseInfoWhithGoodsName(info.GoodsName);
            this.ddlManufacturer.DataTextField = "Manufacturer";
            this.ddlManufacturer.DataValueField = "SerialNumber";
            this.ddlManufacturer.DataSource = list;
            this.ddlManufacturer.DataBind();

            this.ddlManufacturer.SelectedValue = sn;


        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            MyWebForm.ClearIBData(this.Controls);
        }

        protected void ddlGoodsNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "请选择厂商";
            string goodsName = this.ddlGoodsNames.SelectedItem.Text;
            if (goodsName != "请选择")
            {
                GoodsManageBIZ biz = new GoodsManageBIZ();
                List<GoodsBaseInfoVM> list = biz.GetGoodsBaseInfoWhithGoodsName(goodsName);
                this.ddlManufacturer.DataTextField = "Manufacturer";
                this.ddlManufacturer.DataValueField = "SerialNumber";//"SerialNumber"
                this.ddlManufacturer.DataSource = list ;
                this.ddlManufacturer.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dlCHJLH.Text != "")
            {
                GoodsManageBIZ biz = new GoodsManageBIZ();
                GoodsStock info = new GoodsStock() { GoodsID = int.Parse( this.dlCHJLH.Text) };
                biz.DeleteGoodsStock(info);
                lblMsg.Text = "删除成功！";
                MyWebForm.ClearIBData(this.Controls);
                //重新绑定数据
                this.ProPageToolBar1.ReBindResultData();
            }
            else
            {
                lblMsg.Text = "请先选择一条记录！";
            }
        }
    }
}
