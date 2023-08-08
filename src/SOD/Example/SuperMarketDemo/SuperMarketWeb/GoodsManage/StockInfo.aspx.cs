using System;
using System.Web.UI;
using PWMIS.DataForms.Adapter;
using SuperMarketBLL;
using SuperMarketDAL.Entitys;

namespace SuperMarketWeb.GoodsManage
{
    public partial class StockInfo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var biz = new GoodsManageBIZ();

                var nameList = biz.GetAllGoodsNames();
                nameList.Insert(0, "请选择");
                ddlGoodsNames.DataSource = nameList;
                ddlGoodsNames.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dtbSN.Text = ddlManufacturer.SelectedValue;
            //保存数据
            var ibCommandList = MyWebForm.Instance.AutoUpdateIBFormData(Controls);
            //获取插入的ID
            if (dlCHJLH.Text == "")
                if (ibCommandList.Count > 0)
                {
                    var command = ibCommandList[0];
                    dlCHJLH.Text = command.InsertedID.ToString();
                } //end if

            lblMsg.Text = "保存成功！";
            //重新绑定数据
            ProPageToolBar1.ReBindResultData();
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            //关键，必须为主键控件赋值
            dlCHJLH.Text = GridView1.SelectedRow.Cells[1].Text;
            //填充数据
            MyWebForm.Instance.AutoSelectIBForm(Controls);

            //设置下拉框的选项
            //得到当前条码号
            var sn = dtbSN.Text;
            var biz = new GoodsManageBIZ();
            var info = biz.GetGoodsBaseInfo(sn);

            ddlGoodsNames.Text = info.GoodsName;

            var list = biz.GetGoodsBaseInfoWhithGoodsName(info.GoodsName);
            ddlManufacturer.DataTextField = "Manufacturer";
            ddlManufacturer.DataValueField = "SerialNumber";
            ddlManufacturer.DataSource = list;
            ddlManufacturer.DataBind();

            ddlManufacturer.SelectedValue = sn;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            MyWebForm.ClearIBData(Controls);
        }

        protected void ddlGoodsNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "请选择厂商";
            var goodsName = ddlGoodsNames.SelectedItem.Text;
            if (goodsName != "请选择")
            {
                var biz = new GoodsManageBIZ();
                var list = biz.GetGoodsBaseInfoWhithGoodsName(goodsName);
                ddlManufacturer.DataTextField = "Manufacturer";
                ddlManufacturer.DataValueField = "SerialNumber"; //"SerialNumber"
                ddlManufacturer.DataSource = list;
                ddlManufacturer.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (dlCHJLH.Text != "")
            {
                var biz = new GoodsManageBIZ();
                var info = new GoodsStock { GoodsID = int.Parse(dlCHJLH.Text) };
                biz.DeleteGoodsStock(info);
                lblMsg.Text = "删除成功！";
                MyWebForm.ClearIBData(Controls);
                //重新绑定数据
                ProPageToolBar1.ReBindResultData();
            }
            else
            {
                lblMsg.Text = "请先选择一条记录！";
            }
        }
    }
}