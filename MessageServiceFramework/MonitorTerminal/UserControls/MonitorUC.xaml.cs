using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TranstarAuction.Presenters.Presenter;
using System.Collections.ObjectModel;
using MonitorTerminal.CustomEntities;

using System.Linq;
using PWMIS.EnterpriseFramework.Service.Runtime.Principal;

namespace MonitorTerminal.UserControls
{
    /// <summary>
    /// Online User Monitor
    /// </summary>
    public partial class MonitorUC : BaseUserControl
    {
        protected MonitorTerminalPresenter presenter = new MonitorTerminalPresenter();
        protected ObservableCollection<ServiceIdentity> datasource = new ObservableCollection<ServiceIdentity>();
        protected ServiceIdentityComparer comparer = new ServiceIdentityComparer();

        public MonitorUC()
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
            presenter.RequestServiceIdentity(o =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    lock (this)
                    {
                        var t = datasource.Intersect(o, comparer).ToList();//公有的
                        var t1 = datasource.Except(t, comparer).ToList();//预删除的
                        var t2 = o.Except(t, comparer).ToList();//预增加的
                        foreach (var d in t1)
                        {
                            datasource.Remove(d);
                        }
                        foreach (var d in t2)
                        {
                            datasource.Add(d);
                        }
                    }
                }));
            });
        }
    }

    public class ServiceIdentityComparer : IEqualityComparer<ServiceIdentity>
    {
        public bool Equals(ServiceIdentity x, ServiceIdentity y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(ServiceIdentity obj)
        {
            return obj.Id;
        }
    }
}
