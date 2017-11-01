using System;
using System.Collections.Generic;
 
using System.ComponentModel;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Client.Model;
using PWMIS.EnterpriseFramework.Service.Runtime.Principal;

namespace TranstarAuction.Presenters.Presenter
{
    public class MonitorTerminalPresenter : PresenterBase
    {
        public void RequestServiceIdentity(Action<List<ServiceIdentity>> action)
        {
            ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "User";
            request.MethodName = "GetIdentitys";
            request.Parameters = new object[] { };

            base.ServiceProxy.Subscribe<List<ServiceIdentity>>(request, DataType.Json, o =>
            {
                if (o.Succeed)
                {
                    if (action != null)
                        action(o.Result);
                }
            }); 
        }

        public void RequestServiceHostInfo(Action<NotifyServiceHostInfo> action)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "RegService";
            request.MethodName = "GetCurrentHostInfo";
            request.Parameters = new object[] { };

            base.ServiceProxy.Subscribe<NotifyServiceHostInfo>(request, DataType.Json, o =>
            {
                if (o.Succeed)
                {
                    if (action != null)
                        action(o.Result);
                }
            });
        }

        public void Close()
        {
            base.ServiceProxy.Close();
        }
    }

    public class NotifyServiceHostInfo : ServiceHostInfo, INotifyPropertyChanged
    {
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 服务宿主上活动的连接数量（长连接）
        /// </summary>
        public int ActiveConnectCount
        {
            get
            {
                return base.ActiveConnectCount;
            }
            set
            {
                base.ActiveConnectCount = value;
                OnPropertyChanged("ActiveConnectCount");
            }
        }
        /// <summary>
        /// 当前监听器数量（快照）
        /// </summary>
        public int ListenerCount
        {
            get
            {
                return base.ListenerCount;
            }
            set
            {
                base.ListenerCount = value;
                OnPropertyChanged("ListenerCount");
            }
        }
        /// <summary>
        /// 监听器最大值
        /// </summary>
        public int ListenerMaxCount
        {
            get
            {
                return base.ListenerMaxCount;
            }
            set
            {
                base.ListenerMaxCount = value;
                OnPropertyChanged("ListenerMaxCount");
            }
        }
        /// <summary>
        /// 监听器最大值发生时间
        /// </summary>
        public DateTime ListenerMaxDateTime
        {
            get
            {
                return base.ListenerMaxDateTime;
            }
            set
            {
                base.ListenerMaxDateTime = value;
                OnPropertyChanged("ListenerMaxDateTime");
            }
        }
    }
}
