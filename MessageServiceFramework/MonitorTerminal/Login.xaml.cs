using PWMIS.EnterpriseFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
 

namespace MonitorTerminal
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            
        }

        public static string CurrentLoginUserName { get; private set; }

        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("请输入密码！");
                return;
            }

            string domain = this.txtDomain.Text;//读取域信息
            string roleName = System.Configuration.ConfigurationManager.AppSettings["role"].ToString();
            
            string loginName =txtUserName.Text;
            string userName = "";
            string ADPath = "LDAP://" + domain; ;
            bool isRole = AdHelper.IsExistUser(ADPath, roleName, loginName, out userName);
            bool isTrueUser = AdHelper.IsDomainUser(domain, loginName, txtPassword.Password);
            bool result = isRole && isTrueUser;
            //result = true;
            if (result)
            {
                CurrentLoginUserName = userName;
                this.DialogResult = true;
            }
            else
            {
                if(!isRole)
                    MessageBox.Show("用户不存在或者不属于指定的用户组！");
                else if(!isTrueUser)
                    MessageBox.Show("登录密码错误！");
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtDomain.Text = System.Configuration.ConfigurationManager.AppSettings["domain"].ToString();
        }

        
    }
}
