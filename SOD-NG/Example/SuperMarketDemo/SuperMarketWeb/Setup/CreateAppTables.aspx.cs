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
               if (Session["errmsg"] != null)
                   this.lblErrMsg.Text = Session["errmsg"].ToString();

               GetScriptName();

               if (MyDB.Instance.CurrentDBMSType == PWMIS.Common.DBMSType.Access)
               {
                   Access access = MyDB.Instance as Access;
                   string dbFilePath = access.ConnectionDataSource;
                   if (!System.IO.File.Exists(dbFilePath))
                   {
                       PWMIS.AccessExtensions.AccessUility.CreateDataBase(dbFilePath);
                       this.lblErrMsg.Text += ";Access 数据库文件已经自动创建，请刷新或者继续操作本页面。 ";
                   }

               }
            }
        }

        private string GetScriptName()
        {
            var dbmsType = MyDB.Instance.CurrentDBMSType;
            string itemName = "";
            switch (dbmsType)
            {
                case PWMIS.Common.DBMSType.Access:
                case PWMIS.Common.DBMSType.SqlServer:
                case PWMIS.Common.DBMSType.SqlServerCe:
                    itemName = "";
                    break;
                case PWMIS.Common.DBMSType.MySql:
                    itemName = "_MySQL";
                    break;
                case PWMIS.Common.DBMSType.SQLite:
                    itemName = "_SQLite";
                    break;
                default:
                    itemName = "No";
                    break;
            }
            if (itemName != "No")
            {
                string scriptName = string.Format("SuperMarketDAL.Entitys.CreateTables{0}.sql", itemName);
                //string str = PWMIS.Core.CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
                this.lblScript.Text = "创建超市管理系统数据库，当前创建脚本文件名称：" + scriptName;
               
                return scriptName;
            }
            else
            {
                this.lblScript.Text = "暂不支持自动创建当前数据库类型的超市管理系统数据库，请手工创建数据库。";
            }
            return "";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
            string scriptName = GetScriptName();
            string str = PWMIS.Core.CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
            try
            {
                var db = MyDB.Instance;
                string createTableSql = str.Replace("go\r\n", ";\r\n");
                if (db.CurrentDBMSType == PWMIS.Common.DBMSType.Access || db.CurrentDBMSType == PWMIS.Common.DBMSType.SqlServerCe)
                {
                    createTableSql = createTableSql.Replace("--创建超市信息表，数据库类型：SqlServer", "");
                    string[] sqls = createTableSql.Split(new char[]{';'}, 7);//一共只有7个表
                    foreach (string sql in sqls)
                    {
                        db.ExecuteNonQuery(sql);
                    }
                    
                }
                else
                    db.ExecuteNonQuery(createTableSql);

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