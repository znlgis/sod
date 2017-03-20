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
using TranstarAuction.Presenters.Presenter;

namespace MonitorTerminal.Dialogs
{
    /// <summary>
    /// SingelInputBox.xaml 的交互逻辑
    /// </summary>
    public partial class SingelInputBox : Window
    {
        protected ManagePresenter presenter = new ManagePresenter();

        public SingelInputBox(string ServiceUri)
        {
            InitializeComponent();
            presenter.ServiceProxy.ServiceBaseUri = ServiceUri;
            this.btnOK.Click += new RoutedEventHandler(btnOK_Click);
        }

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            presenter.RequestManagePermission(result =>
            {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (result)
                        {
                            DialogResult = result;
                        }
                        else
                            CommonMethods.ShowMessage("Password is Wrong!");
                    }));             
            }, this.txtPwd.Password.Trim());
        }
    }
}
