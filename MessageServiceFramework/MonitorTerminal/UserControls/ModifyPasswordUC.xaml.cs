using System;
using System.Windows;
using MonitorTerminal.CustomEntities;
using TranstarAuction.Presenters.Presenter;

namespace MonitorTerminal.UserControls
{
    /// <summary>
    /// ModifyPasswordUC.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyPasswordUC : BaseUserControl
    {
        protected ManagePresenter presenter = new ManagePresenter();
        public ModifyPasswordUC()
        {
            InitializeComponent();
            this.btnSubmit.Click += new RoutedEventHandler(btnSubmit_Click);
            this.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
        }
        public override void CloseServiceProxy()
        {
            presenter.Close();
        }
        public override void LoadData(ServiceItem servicenode)
        {
            base.LoadData(servicenode);
            presenter.ServiceProxy.ServiceBaseUri = servicenode.ServiceUri;
            ClearInput();
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearInput();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidInput())
            {
                var original = this.txtOriginal.Password.Trim();
                var newly = this.txtNewly.Password.Trim();
                presenter.RequestChangePassword(result =>
                    {
                        if (result)
                        {
                            CommonMethods.ShowMessage("Submit Change!");
                            ClearInput();
                        }
                        else
                        {
                            CommonMethods.ShowMessage("Change Failure!");
                        }
                    }, original, newly);
            }
        }

        private void ClearInput()
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    this.txtOriginal.Clear();
                    this.txtNewly.Clear();
                    this.txtConfirm.Clear();
                }));
        }

        private bool ValidInput()
        {
            var original = this.txtOriginal.Password.Trim();
            var newly = this.txtNewly.Password.Trim();
            var confirm = this.txtConfirm.Password.Trim();
            if (string.Compare(newly, confirm) != 0)
            {
                CommonMethods.ShowMessage("the new password is different from the confirm password!");
                return false;
            }
            return true;
        }
    }
}
