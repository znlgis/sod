using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PWMIS.AccessExtensions;
using PWMIS.DataMap.Entity;
using PWMIS.DataProvider.Adapter;
using PWMIS.DataProvider.Data;

namespace SimpleAccessWinForm
{
    public partial class Form1 : Form
    {
        private readonly BindingList<User> UserBindingList = new BindingList<User>();

        public Form1()
        {
            InitializeComponent();
        }

        public void AddUserModel(User user)
        {
            UserBindingList.Add(user);
        }

        public User GetUserByID(int uid)
        {
            return UserBindingList.FirstOrDefault(p => p.UserID == uid);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            var frm = new Form2();
            frm.Show(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dbpath = Application.StartupPath + "\\TEST.accdb";
            if (!File.Exists(dbpath))
            {
                MessageBox.Show("没有找到数据库文件，请先[创建数据库]");
                lblMsg.Text = "没有找到数据库文件，请先[创建数据库]";
            }
            else
            {
                btnCreateDB.Enabled = false;
                lblMsg.Text = "当前数据库文件：" + dbpath;
                //配置连接
                AccessUility.ConfigConnectionSettings("AccessConn", dbpath);
            }

            UserTypeInfoDataSource.InitDataSource(userTypeInfoBindingSource);
            dataGridView1.AutoGenerateColumns = false;
        }

        private void btnCreateDB_Click(object sender, EventArgs e)
        {
            var dbpath = Application.StartupPath + "\\TEST.accdb";
            if (!File.Exists(dbpath))
            {
                //创建数据库文件
                AccessUility.CreateDataBase(dbpath, null);
                //创建表
                var access = new Access();
                access.ConnectionString = AccessUility.CreateConnectionString(dbpath);

                AccessUility.CreateTable(access, new User());
                //配置连接
                AccessUility.ConfigConnectionSettings("AccessConn", dbpath);

                MessageBox.Show("创建数据成功！");
            }
            else
            {
                MessageBox.Show("数据库已经创建过了，如需重新创建，请先删除数据库文件。");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            //List<User> list = OQL.From<User>().Select().END.ToList<User>();
            //上面一行是简写的方式，下面是传统的方式，带分页，当前选定第一页
            var user = new User();
            var q = OQL.From(user)
                .Select(user.UserID, user.UserName, user.UserType, user.RegisterDate, user.Expenditure)
                .OrderBy(user.UserName, "asc")
                .END;
            q.Distinct = true;
            q.Limit(10);

            var list = EntityQuery<User>.QueryList(q);

            foreach (var item in list)
                UserBindingList.Add(item);
            dataGridView1.DataSource = UserBindingList;
            //this.dataGridView1.DataSource = list;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var user = dataGridView1.CurrentRow.DataBoundItem as User;
                if (user != null)
                {
                    //事务的例子
                    var db = MyDB.GetDBHelperByConnectionName("AccessConn");
                    db.BeginTransaction();
                    try
                    {
                        EntityQuery<User>.Instance.Update(user, db);
                        db.Commit();
                        MessageBox.Show("标识为" + user.UserID + " 的对象修改成功！");
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        MessageBox.Show("修改数据失败：" + ex.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var user = dataGridView1.CurrentRow.DataBoundItem as User;
                if (user != null)
                {
                    EntityQuery<User>.Instance.Delete(user);
                    MessageBox.Show("标识为" + user.UserID + " 的对象删除成功！");

                    UserBindingList.Remove(user);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("iexplore.exe", "http://www.pwmis.com/sqlmap");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn bc = dataGridView1.Columns[e.ColumnIndex] as DataGridViewButtonColumn;
                if (bc != null)
                {
                    //bc.Text = DateTime.Now.ToString();
                    //dateTimePicker1.Location=dataGridView1.
                }
            }
        }
    }
}