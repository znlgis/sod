using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PWMIS.DataProvider.Adapter;
using System.IO;

namespace TestWebApp
{
    public partial class TestSqlite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //SQLite 它会自动创建库文件
            //string fileName = Server.MapPath("~/App_Data/testdb.db");
            //if (!File.Exists(fileName))
            //    CreatNewSQLite(fileName);
            string sql = @"
DROP TABLE IF EXISTS  Table1;
CREATE TABLE Table1(
[ID] int,
[Name] varchar(20),
[AddTime] datetime
);
";
            try
            {
                MyDB.Instance.ExecuteNonQuery(sql);
                this.Label1.Text = "初始化数据成功！";
            }
            catch (Exception ex)
            {
                this.Label1.Text = "Error:"+ex.Message;
            }
          

        }

        public void CreatNewSQLite(string SQLiteName)
        {
            FileStream fi = File.Create(SQLiteName);
            fi.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(this.txtID.Text);
                string name = this.txtName.Text;
                System.Data.IDataParameter[] paras = { 
                                            MyDB.Instance.GetParameter("ID",id),
                                            MyDB.Instance.GetParameter("Name",name),
                                            MyDB.Instance.GetParameter("At",DateTime.Now)
                                                     };
                ((System.Data.IDbDataParameter)paras[1]).Size=20;

                int count = MyDB.Instance.ExecuteNonQuery("insert into Table1(ID,Name,AddTime) values(@ID,@Name,@At)", System.Data.CommandType.Text, paras);
                if (count > 0)
                {
                    this.Label1.Text = "插入成功 ！";
                    this.GridView1.DataSource = MyDB.Instance.ExecuteDataSet("select * from Table1 order by AddTime desc limit 5");
                    this.GridView1.DataBind();
                }


            }
            catch (Exception ex)
            {
                this.Label1.Text = "Error:" + ex.Message;
            }
           

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            this.GridView1.DataSource = MyDB.Instance.ExecuteDataSet("select * from Table1 order by AddTime desc limit 5");
            this.GridView1.DataBind();
            this.Label1.Text = "操作成功 ！";
        }
    }
}