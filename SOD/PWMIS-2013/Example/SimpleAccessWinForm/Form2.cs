using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PWMIS.DataProvider.Adapter;

using PWMIS.DataForms.Adapter;

namespace SimpleAccessWinForm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UserTypeInfoDataSource.InitDataSource(this.userTypeInfoBindingSource);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!MyWinForm.ValidateIBControls(Controls))
            {
                //MessageBox.Show("请输入用户名！");
                return;
            }
            

            var ibCommandList = MyWinForm.Instance.AutoUpdateIBFormData(this.Controls);
            if (dlbUID.Text == "" || dlbUID.Text == "0")
            {
                //====插入数据的模式====
                IBCommand command = ibCommandList[0];
                dlbUID.Text = command.InsertedID.ToString();//为自增数据的控件赋值

                ////收集数据到实体类中
                User user = new User();
                MyWinForm.DataMap.CollectData(user, this.Controls,true);
                //添加实体类到主窗体的网格控件中
                Form1 form1 = this.Owner as Form1;
                form1.AddUserModel(user);
            }
            else
            { 
                //====修改数据的模式=====
                Form1 form1 = this.Owner as Form1;
                //找到主窗体中的实体类对象
                User user1 = form1.GetUserByID(int.Parse(dlbUID.Text));
                //收集当前窗体的数据到实体类中，从而同步更新主窗体的数据
                MyWinForm.DataMap.CollectData(user1, this.Controls, true);

            }
           MessageBox.Show("保存成功！");
        }

        
    }
}
