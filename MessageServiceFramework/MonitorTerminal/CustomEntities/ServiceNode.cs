using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TranstarAuction.Presenters.Presenter;
using MonitorTerminal.UserControls;
using TranstarAuction.Model;
using System.ComponentModel;

namespace MonitorTerminal.CustomEntities
{
    public class TreeNode:INotifyPropertyChanged
    {
        public TreeNode()
        { Children = new List<TreeNode>(); }

        public string Text { get; set; }

        public bool HasChildren
        {
            get
            {
                if (Children == null) return false;
                else return Children.Count > 0;
            }
        }

        public List<TreeNode> Children { get; set; }

        public object Tag { get; set; }

        public TreeNode RootNode { get; set; }

        public event EventHandler Expanded;
        protected void OnExpanded()
        {
            if (Expanded != null)
                Expanded(this, EventArgs.Empty);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                if(_isExpanded)
                    OnExpanded();
                OnPropertyChanged("IsExpanded");
            }
        }

        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ServiceItem : TreeNode
    {
        private TreeNode manageItem = null;

        public string NickName
        {
            get
            { return  service.NickName; }
            set
            {
                service.NickName = value;
                OnPropertyChanged("NickName");
            }
        }

        public string IdentityName
        {
            get
            {
                return service.IdentityName; 
            }
            set
            {
                service.IdentityName = value;
            }
        }

        public ServiceItem(Service s)
        {
            service = s;
            RootNode = this;
            Text = ServiceAddress;
            service.ConnectedComplete += new Action(service_ConnectedComplete);
            Expanded += new EventHandler(ServiceItem_Expanded);
            InitChildren();
        }
         
        void ServiceItem_Expanded(object sender, EventArgs e)
        {
            if (service.State != ConnectState.Connected)
            {
                service.ConnectService();
                service.OnConnectedComplete();
            }
        }

        #region Service Relevance
        public Service service;
        /// <summary>
        /// Without "net.tcp://"
        /// </summary>
        public string ServiceAddress
        {
            get { return service.ServiceAddress; }
        }

        public string ServiceUri
        {
            get { return service.ServiceUri; }
        }

        public ConnectState ServiceConnState
        {
            get { return service.State; }
            set { service.State = value; }
        }
        #endregion

        #region UI Relevance
        private List<BaseUserControl> MyChildControls = new List<BaseUserControl>();
        #endregion

        #region Event and Methods
        void service_ConnectedComplete()
        {
            if (service.State == ConnectState.Connected)
            {
                IsExpanded = true;
            }
            else
            {
                IsExpanded = false;
            }
            manageItem.IsExpanded = false;
            OnPropertyChanged("ServiceConnState");
        }

        public void Reconnect()
        {
            service.ConnectService();
            service.OnConnectedComplete();
            //重新装载
            foreach (var item in MyChildControls)
            {
                item.LoadData(this);
            }
        }

        public void Disconnect()
        {
            service.Disconnect();
            service.OnConnectedComplete(); 
            //重新装载
            foreach (var item in MyChildControls)
            {
                //item.LoadData(this);
                item.CloseServiceProxy();
            }
        }

        protected void InitChildren()
        {
            BaseUserControl currObject = null;

            currObject = new MonitorUC();
            MyChildControls.Add(currObject);
            Children.Add(new TreeNode() { Text = "在线用户的监控", Tag = currObject, RootNode = this });

            currObject = new HostInfoUC();
            MyChildControls.Add(currObject);
            Children.Add(new TreeNode() { Text = "系统连接情况监控", Tag = currObject, RootNode = this });

            currObject = new LogManagerUC();
            MyChildControls.Add(currObject);
            Children.Add(new TreeNode() { Text = "日志管理", Tag = currObject, RootNode = this });

            #region AddManageItem
            manageItem = new TreeNode() { Text = "系统管理", Tag = new object(), RootNode = this };

            currObject = new UpdaterUC(UploadFileType.ServicePacket);
            MyChildControls.Add(currObject);
            manageItem.Children.Add(new TreeNode() { Text = "服务程序升级", Tag = currObject, RootNode = this });

            currObject = new UpdaterUC(UploadFileType.ClientPacket);
            MyChildControls.Add(currObject);
            manageItem.Children.Add(new TreeNode() { Text = "客户端补丁上传", Tag = currObject, RootNode = this });

            currObject = new ModifyPasswordUC();
            MyChildControls.Add(currObject);
            manageItem.Children.Add(new TreeNode() { Text = "修改密码", Tag = currObject, RootNode = this });

            manageItem.Expanded += new EventHandler(manageItem_Expanded);
            Children.Add(manageItem);
            #endregion
        }

        void manageItem_Expanded(object sender, EventArgs e)
        {
            if (!service.IsAdminRole)
            {
                if (new Dialogs.SingelInputBox(ServiceUri).ShowDialog().Value)
                {
                    service.IsAdminRole = true;
                    manageItem.IsExpanded = true;
                }
                else
                    manageItem.IsExpanded = false;
            }
        }
        #endregion
    }
    public enum ConnectState { Connected, Disconnected, Failed }

    [Serializable]
    public class Service
    {
        protected ManagePresenter presenter;
        public event Action ConnectedComplete;
        public void OnConnectedComplete()
        {
            if (ConnectedComplete != null)
                ConnectedComplete();
        }
        public string NickName { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceAddress { get; set; }

        public string ServiceUri
        {
            get { return "net.tcp://" + ServiceAddress; }
        }

        public ConnectState State{ get; set; }

        public bool IsAdminRole { get; set; }

        public string IdentityName { get; set; }

        public Service(string identityname) : this()
        {
            IdentityName = identityname;
        }

        public Service()
        {
            ServiceAddress = "127.0.0.1:8888";
            IsAdminRole = false;
            State = ConnectState.Disconnected;
            presenter = new ManagePresenter();
            presenter.ServiceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(ServiceProxy_ErrorMessage);
        }

        void ServiceProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            State = ConnectState.Failed;
            OnConnectedComplete();
        }

        public void ConnectService()
        {
            presenter.ServiceProxy.ServiceBaseUri = ServiceUri;
            presenter.RequestManageConnect(result =>
            {
                if (result)
                {
                    State = ConnectState.Connected;
                }
                else
                {
                    State = ConnectState.Failed;
                }
                OnConnectedComplete();
            }, IdentityName);
        }

        public void Disconnect()
        {
            State = ConnectState.Disconnected;
            IsAdminRole = false;
            OnConnectedComplete();
        }
    }
}
