using System;
using System.Web.UI;
using PWMIS.Common.DataMap;
using PWMIS.DataForms.Adapter;
using PWMIS.DataMap;
using SuperMarketBLL;
using SuperMarketDAL.Entitys;

namespace SuperMarketWeb.GoodsManage
{
    public partial class BaseInfo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var biz = new GoodsManageBIZ();

                ProPageToolBar1.PageSize = 5;
                ProPageToolBar1.AllCount = biz.GetAllGoodsBaseInfoCount();

                GridView1.DataSource = biz.GetGoodsBaseInfoList(ProPageToolBar1.PageSize, 1, ProPageToolBar1.AllCount);
                GridView1.DataBind();
            }
        }

        private void bindGrid()
        {
            var biz = new GoodsManageBIZ();
            ProPageToolBar1.AllCount = biz.GetAllGoodsBaseInfoCount();
            GridView1.DataSource = biz.GetGoodsBaseInfoList(ProPageToolBar1.PageSize, 1, ProPageToolBar1.AllCount);
            GridView1.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (dbtSN.Text.Trim() == "")
            {
                lblMsg.Text = "没有条码号，请输入!";
                return;
            }

            //实例化一个控件数据映射对象
            var cdm = new ControlDataMap();
            //收集数据到实体对象
            var info = cdm.CollectDataToObject<GoodsBaseInfo>(
                MyWebForm.GetIBControls(tbGoosBaseInfo.Controls)
            );
            //调用业务类，保存数据
            var biz = new GoodsManageBIZ();
            if (biz.SaveGoodsBaseInfo(info))
                lblMsg.Text = "保存成功!";
            else
                lblMsg.Text = "保存失败.";

            bindGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            var sn = GridView1.SelectedRow.Cells[1].Text;
            //调用业务层方法获取实体
            var biz = new GoodsManageBIZ();
            var info = biz.GetGoodsBaseInfo(sn);
            //将实体与页面控件绑定
            WebControlDataMap.FillDataFromEntityClass(info,
                MyWebForm.GetIBControls(tbGoosBaseInfo.Controls)
            );
        }

        protected void ProPageToolBar1_PageChangeIndex(object sender, EventArgs e)
        {
            var biz = new GoodsManageBIZ();
            GridView1.DataSource = biz.GetGoodsBaseInfoList(
                ProPageToolBar1.PageSize,
                ProPageToolBar1.CurrentPage,
                ProPageToolBar1.AllCount);
            GridView1.DataBind();
        }
    }
}