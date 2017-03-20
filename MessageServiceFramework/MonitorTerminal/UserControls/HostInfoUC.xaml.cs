using System;
using MonitorTerminal.CustomEntities;

using System.Collections.ObjectModel;
using TranstarAuction.Presenters.Presenter;

namespace MonitorTerminal.UserControls
{
    /// <summary>
    /// HostInfoUC.xaml 的交互逻辑
    /// </summary>
    public partial class HostInfoUC : BaseUserControl
    {
        protected MonitorTerminalPresenter presenter = new MonitorTerminalPresenter();
        protected ObservableCollection<NotifyServiceHostInfo> datasource = new ObservableCollection<NotifyServiceHostInfo>();

        public HostInfoUC()
        {
            InitializeComponent();
            this.dgMainData.ItemsSource = datasource;
        }

        public override void CloseServiceProxy()
        {
            presenter.Close();
        }

        public override void LoadData(ServiceItem servicenode)
        {
            base.LoadData(servicenode);
            datasource.Clear();
            presenter.Close();
            presenter.ServiceProxy.ServiceBaseUri = servicenode.ServiceUri;
            presenter.RequestServiceHostInfo(o =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (datasource.Count == 0)
                        datasource.Add(o);
                    else
                    {
                        datasource[0].ActiveConnectCount = o.ActiveConnectCount;
                        datasource[0].ListenerCount = o.ListenerCount;
                        datasource[0].ListenerMaxCount = o.ListenerMaxCount;
                        datasource[0].ListenerMaxDateTime = o.ListenerMaxDateTime;
                    }
                }));
            });
        }
    }
}

