using System;
using System.Windows;
using MonitorTerminal.CustomEntities;
using System.Net;

namespace MonitorTerminal.Dialogs
{
    /// <summary>
    /// ServiceConnected.xaml 的交互逻辑
    /// </summary>
    public partial class ServiceConnected : Window
    {
        public Service service { get; set; }

        public ServiceConnected()
        {
            InitializeComponent();
            this.btnConnected.Click += new RoutedEventHandler(btnConnected_Click);
            service = new Service(Login.CurrentLoginUserName);
            service.ConnectedComplete += new Action(service_ConnectedComplete);
            if (!string.IsNullOrEmpty(service.ServiceAddress))
            {
                var ipandport = service.ServiceAddress.Split(':');
                if (ipandport.Length == 2)
                {
                    this.txtServiceAddress.Text = ipandport[0];
                    this.txtServicePort.Text = ipandport[1];
                }
            }
        }

        void service_ConnectedComplete()
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    if (service.State == ConnectState.Connected)
                    {
                        service.ConnectedComplete -= service_ConnectedComplete;
                        DialogResult = true;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Connected Fail!");
                        SetEnabled(true);
                    }
                }));
        }

        void btnConnected_Click(object sender, RoutedEventArgs e)
        {
            var ipaddress = this.txtServiceAddress.Text.Trim();
            var port = this.txtServicePort.Text.Trim();
            if (ValidInput(ipaddress,port))
            {
                SetEnabled(false);
                service.ServiceAddress = ipaddress + ":" + port;
                service.ConnectService();
            }
        }

        private void SetEnabled(bool enabled)
        {
            this.txtServiceAddress.IsEnabled = enabled;
            this.btnConnected.IsEnabled = enabled;
        }

        private bool ValidInput(string ipaddress,string port)
        {
            if (string.IsNullOrEmpty(ipaddress))
            {
                ShowMessage("IP Address is empty!");
                return false;
            }
            if (string.IsNullOrEmpty(port))
            {
                ShowMessage("Port is empty!");
                return false;
            }
            if (IsCorrenctIP(ipaddress))
            {
                if (!ipaddress.StartsWith("192.") && !ipaddress.StartsWith("172.") && !ipaddress.StartsWith("127."))
                {
                    ShowMessage("Please Input IP Address of LAN!");
                    return false;
                }
            }
            else
            {
                ShowMessage("Please Input Correct IP Address!");
                return false;
            }
            uint t;
            if (uint.TryParse(port, out t))
            {
                if (t < 0 || t > 65535)
                {
                    ShowMessage("Please Input correct port number!");
                    return false;
                }
            }
            else
            {
                ShowMessage("Please Input correct port number!");
                return false;
            }
                
            return true;
        }

        private void ShowMessage(string msg)
        {
            CommonMethods.ShowMessage(msg);
        }

        public bool IsCorrenctIP(string ip)
        {
            string pattrn = @"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])";
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, pattrn))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
