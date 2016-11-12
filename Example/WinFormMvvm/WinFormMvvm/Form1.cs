using PWMIS.Common;
using PWMIS.DataForms.Adapter;
using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormMvvm.ViewModel;

namespace WinFormMvvm
{
    public partial class Form1 : MvvmForm
    {
        SubmitedUsersViewModel DataContext{get;set;}
       
        public Form1()
        {
            InitializeComponent();

            DataContext = new SubmitedUsersViewModel();
            
            this.listBox1.DataSource =  DataContext.Users;
            //this.listBox1.DisplayMember = "Name";
            //this.listBox1.ValueMember = "ID";
            //设置成可修改
            this.listBox1.ReadOnly = false;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            base.BindDataControls(this.Controls);
            base.BindCommandControls(this.button1, DataContext.SubmitCurrentUsers);
            base.BindCommandControls(this.button2, DataContext.UpdateUser);
            base.BindCommandControls(this.button3, DataContext.RemoveUser);
        }

        //下面的事件方法仅作为示例，实际上窗体设计器并没有注册这些事件
        #region Button Click

        private void button1_Click(object sender, EventArgs e)
        {
            //下面被注释的代码与未注释的效果相同
            //UserEntity newUser = DataContext.CreateNewUser(DataContext.CurrentUser.Name);
            //DataContext.SubmitUsers(newUser);
            //DataContext.CurrentUser.ID = newUser.ID ;

            DataContext.SubmitCurrentUsers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //下面被注释的代码与未注释的效果相同
            //UserEntity user = listBox1.SelectedItem as UserEntity;
            //user.Name = this.dataTextBox1.Text;
            //DataContext.UpdateUser(user.ID,user.Name );

            DataContext.UpdateUser();


        }
        #endregion

    }
}
