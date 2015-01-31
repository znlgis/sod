using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;
using PWMIS.DataMap ;
using SuperMarketDAL.Entitys;
using SuperMarketBLL;
using PWMIS.Common;
using PWMIS.DataProvider.Adapter;
using PWMIS.Common.DataMap;
using PWMIS.DataForms.Adapter;

namespace SuperMarketWeb.GoodsManage
{
    public partial class BaseInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GoodsManageBIZ biz = new GoodsManageBIZ();

                this.ProPageToolBar1.PageSize = 5;
                this.ProPageToolBar1.AllCount = biz.GetAllGoodsBaseInfoCount();

                this.GridView1.DataSource = biz.GetGoodsBaseInfoList(this.ProPageToolBar1.PageSize, 1, this.ProPageToolBar1.AllCount);
                this.GridView1.DataBind();
            }
        }

        private void bindGrid()
        {
            GoodsManageBIZ biz = new GoodsManageBIZ();
            this.ProPageToolBar1.AllCount = biz.GetAllGoodsBaseInfoCount();
            this.GridView1.DataSource = biz.GetGoodsBaseInfoList(this.ProPageToolBar1.PageSize, 1, this.ProPageToolBar1.AllCount);
            this.GridView1.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (dbtSN.Text.Trim() == "")
            {
                lblMsg.Text = "没有条码号，请输入!";
                return;
            }
            //实例化一个控件数据映射对象
            ControlDataMap cdm = new ControlDataMap();
            //收集数据到实体对象
            GoodsBaseInfo info = cdm.CollectDataToObject<GoodsBaseInfo>(
                MyWebForm.GetIBControls(this.tbGoosBaseInfo.Controls)
                );
            //调用业务类，保存数据
            GoodsManageBIZ biz = new GoodsManageBIZ();
            if (biz.SaveGoodsBaseInfo(info))
               lblMsg.Text = "保存成功!";
           else
               lblMsg.Text = "保存失败.";

            bindGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "Selected id=" + GridView1.SelectedRow.Cells[1].Text;
            string sn = GridView1.SelectedRow.Cells[1].Text;
            //调用业务层方法获取实体
            GoodsManageBIZ biz = new GoodsManageBIZ();
            GoodsBaseInfo info=biz.GetGoodsBaseInfo(sn);
            //将实体与页面控件绑定
            WebControlDataMap.FillDataFromEntityClass(info, 
                MyWebForm.GetIBControls(this.tbGoosBaseInfo.Controls)
                );
        }

        protected void ProPageToolBar1_PageChangeIndex(object sender, EventArgs e)
        {
            GoodsManageBIZ biz = new GoodsManageBIZ();
            this.GridView1.DataSource = biz.GetGoodsBaseInfoList(
                this.ProPageToolBar1.PageSize, 
                this.ProPageToolBar1.CurrentPage,
                this.ProPageToolBar1.AllCount );
            this.GridView1.DataBind();
        }

      
    }
}
