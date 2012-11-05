using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using PWMIS.DataProvider.Adapter;

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
    }
}