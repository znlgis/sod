using System;
using System.Configuration;
using System.Web.UI;
using PWMIS.AccessExtensions;
using PWMIS.Common;
using PWMIS.Core;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace SuperMarketWeb.Setup
{
    public partial class CreateAppTables : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var connSetting =
                    ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                lblProviderName.Text = connSetting.ProviderName;
                lblConnName.Text = connSetting.Name;
                if (Session["errmsg"] != null)
                    lblErrMsg.Text = Session["errmsg"].ToString();

                GetScriptName();

                if (MyDB.Instance.CurrentDBMSType == DBMSType.Access)
                {
                    //Access access = MyDB.Instance as Access;
                    //string dbFilePath = access.ConnectionDataSource;
                    //if (!System.IO.File.Exists(dbFilePath))
                    //{
                    //    PWMIS.AccessExtensions.AccessUility.CreateDataBase(dbFilePath, 
                    //        MyDB.Instance.ConnectionStringBuilder as System.Data.OleDb.OleDbConnectionStringBuilder);
                    //    this.lblErrMsg.Text += ";Access 数据库文件已经自动创建，请刷新或者继续操作本页面。 ";
                    //}

                    var accCtx = new AccessDbContext(MyDB.Instance as Access);
                    accCtx.CheckDB();

                    lblScript.Text = "当前数据库文件：" + accCtx.DBFilePath;
                }
            }
        }

        private string GetScriptName()
        {
            var dbmsType = MyDB.Instance.CurrentDBMSType;
            var itemName = "";
            switch (dbmsType)
            {
                case DBMSType.Access:
                case DBMSType.SqlServer:
                case DBMSType.SqlServerCe:
                    itemName = "";
                    break;
                case DBMSType.MySql:
                    itemName = "_MySQL";
                    break;
                case DBMSType.SQLite:
                    itemName = "_SQLite";
                    break;
                default:
                    itemName = "No";
                    break;
            }

            if (itemName != "No")
            {
                var scriptName = string.Format("SuperMarketDAL.Entitys.CreateTables{0}.sql", itemName);
                //string str = PWMIS.Core.CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
                lblScript.Text = "创建超市管理系统数据库，当前创建脚本文件名称：" + scriptName;

                return scriptName;
            }

            lblScript.Text = "暂不支持自动创建当前数据库类型的超市管理系统数据库，请手工创建数据库。";
            return "";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var connSetting = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
            var scriptName = GetScriptName();
            var str = CommonUtil.GetAssemblyResource("SuperMarketDAL", scriptName);
            try
            {
                var db = MyDB.Instance;
                var createTableSql = str.Replace("go\r\n", ";\r\n");
                if (db.CurrentDBMSType == DBMSType.Access || db.CurrentDBMSType == DBMSType.SqlServerCe)
                {
                    createTableSql = createTableSql.Replace("--创建超市信息表，数据库类型：SqlServer", "");
                    var sqls = createTableSql.Split(new[] { ';' }, 7); //一共只有7个表
                    foreach (var sql in sqls) db.ExecuteNonQuery(sql);
                }
                else
                {
                    db.ExecuteNonQuery(createTableSql);
                }

                lblMsg.Text = "初始化数据库成功！";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                //this.lblMsg.Text = "执行查询错误，具体错误信息可以开启PDF.NET的SQL日志功能。";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var sql = @"SELECT [工号],[姓名] 
FROM [雇员表]
   Where [职务名称]='ss'
       ORDER BY [姓名] asc";

            var db = MyDB.Instance;
            try
            {
                var ds = db.ExecuteDataSet(sql);
                lblErrMsg.Text = "test ok.";
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Test Error:" + ex.Message;
            }
        }
    }
}