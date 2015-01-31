using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;
using System.Data;

namespace SuperMarketWeb.Setup
{
    public partial class CreateAppTables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               var connSetting= ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
               this.lblProviderName.Text = connSetting.ProviderName;
               this.lblConnName.Text = connSetting.Name;
               string scriptName = string.Format("SuperMarketDAL.Entitys.CreateTables{0}.sql", connSetting.ProviderName == "SqlServer" ? "" : "_SQLite"); ;
               string str = PWMIS.Core.CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
               this.lblScript.Text = str.Substring(0, 30);
               if(Session["errmsg"]!=null)
                   this.lblErrMsg.Text = Session["errmsg"].ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
            string scriptName = string.Format("SuperMarketDAL.Entitys.CreateTables{0}.sql", connSetting.ProviderName == "SqlServer" ? "" : "_SQLite"); ;
            string str = PWMIS.Core.CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
            try
            {
                MyDB.Instance.ExecuteNonQuery(str.Replace("go\r\n",";\r\n"));
                this.lblMsg.Text = "初始化数据库成功！";

            }
            catch (Exception ex)
            {
                this.lblMsg.Text = ex.Message;
                //this.lblMsg.Text = "执行查询错误，具体错误信息可以开启PDF.NET的SQL日志功能。";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT `工号`,`姓名` 
FROM `雇员表`
   Where `职务名称`='ss'
       ORDER BY `姓名` asc";

            AdoHelper db = MyDB.Instance;
            try
            {
                DataSet ds = db.ExecuteDataSet(sql);
                this.lblErrMsg.Text = "test ok.";
            }
            catch (Exception ex)
            {
                this.lblErrMsg.Text ="Test Error:"+ ex.Message;
            }

        }
    }
}