using System.Windows;
using MonitorTerminal.UserControls;
using System.Collections.ObjectModel;
using MonitorTerminal.CustomEntities;
using MonitorTerminal.Dialogs;
using System.Windows.Controls;
using System.Linq;
using TranstarAuction.Model;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;

namespace MonitorTerminal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //MonitorUC monitoruc = new MonitorUC();
        //UpdaterUC clientupdateruc = new UpdaterUC(UploadFileType.ClientPacket);//节点已经下移
        //UpdaterUC serviceupdateruc = new UpdaterUC(UploadFileType.ServicePacket);//节点已经下移
        //HostInfoUC hostinfouc = new HostInfoUC();
        //ModifyPasswordUC modifypwduc = new ModifyPasswordUC();
        //LogManagerUC loguc = new LogManagerUC();
        ServiceItem current;

        protected ObservableCollection<ServiceItem> services = new ObservableCollection<ServiceItem>();
        private TreeViewItem SelectTreeViewItem = null;
        // We keep the old text when we go into editmode
        // in case the user aborts with the escape key
        private string oldText;
        private bool IsEditMode;
        private ConfigManager configmanager = new ConfigManager();

        public MainWindow()
        {
            InitializeComponent();
            //serviceupdateruc.UpdateComplete += new System.Action<string>(serviceupdateruc_UpdateComplete);
            this.tvObjectResourceManager.ItemsSource = services;
            this.btnConnected.Click += new RoutedEventHandler(btnConnected_Click);
            this.btnReConnected.Click += new RoutedEventHandler(btnReConnected_Click);
            this.btnDisconnect.Click += new RoutedEventHandler(btnDisconnect_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.tvObjectResourceManager.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(tvObjectResourceManager_SelectedItemChanged);
            this.tvObjectResourceManager.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(TreeViewItem_Expanded));
            this.tvObjectResourceManager.AddHandler(TreeViewItem.CollapsedEvent, new RoutedEventHandler(TreeViewItem_Collapsed));
            this.tvObjectResourceManager.AddHandler(TreeViewItem.SelectedEvent, new RoutedEventHandler(TreeViewItem_Selected));
            this.tvObjectResourceManager.AddHandler(TreeViewItem.KeyDownEvent, new KeyEventHandler(TreeViewItem_KeyDown));
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEditMode)
            {
                DeleteServiceItem();
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var list = DeserializeServiceItems();
            if (list != null)
            {
                foreach (var s in list)
                {
                    var serviceitem = new ServiceItem(s);
                    serviceitem.IdentityName = Login.CurrentLoginUserName;
                    serviceitem.ServiceConnState = ConnectState.Disconnected;
                    services.Add(serviceitem);
                }
            }
        }

        DateTime clickTime;
        bool isClicked = false;    
        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isClicked)
            {
                TimeSpan span = DateTime.Now - clickTime;
                if (span.TotalMilliseconds < System.Windows.Forms.SystemInformation.DoubleClickTime)
                {
                    ReNameDoubleClick(e);
                }
                isClicked = false;
            }
            else
            {
                isClicked = true;
                clickTime = DateTime.Now;
            }          
        }

        private void ReNameDoubleClick(MouseButtonEventArgs e)
        {
            var node = this.tvObjectResourceManager.SelectedItem as ServiceItem;
            if (node != null)
            {
                var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

                if (treeViewItem != null)
                {
                    if (treeViewItem == SelectTreeViewItem)
                    {
                        SetCurrentItemInEditMode(true);
                        e.Handled = true;
                    }
                }
            }
        }

        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        private void TreeViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            var node = this.tvObjectResourceManager.SelectedItem as ServiceItem;
            if (node != null)
            {
                if (e.Key == Key.F2)
                    SetCurrentItemInEditMode(true);
                else if (e.Key == Key.Delete)
                {
                    if (!IsEditMode)
                    {
                        DeleteServiceItem();
                    }
                }
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            SetCurrentItemInEditMode(false);
        }
        private void TreeViewItem_Collapsed(object sender, RoutedEventArgs e)
        {
            SetCurrentItemInEditMode(false);
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            SetCurrentItemInEditMode(false);
            SelectTreeViewItem = e.OriginalSource as TreeViewItem;
        }

        private void SetCurrentItemInEditMode(bool EditMode)
        {
            if (EditMode != IsEditMode)
            {
                if (SelectTreeViewItem != null)
                {
                    if (EditMode)
                        SelectTreeViewItem.HeaderTemplate = (DataTemplate)FindResource("EditModeTemplate");
                    else
                        SelectTreeViewItem.HeaderTemplate = (DataTemplate)FindResource("ServiceItemTemplate");
                }
                IsEditMode = EditMode;
            }
        }

        void btnReConnected_Click(object sender, RoutedEventArgs e)
        {
            current.Reconnect();
        }

        void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect(current);
        }

        void serviceupdateruc_UpdateComplete(string msg)
        {
            CommonMethods.ShowMessage(msg);
            Disconnect(current);
        }

        private void Addconnect(Service service)
        {
            var s = services.Where(o => o.ServiceAddress == service.ServiceAddress).FirstOrDefault();
            if (s == null)
            {
                var serviceitem = new ServiceItem(service);
                serviceitem.NickName = serviceitem.Text;
                services.Add(serviceitem); 
                SerializeServiceItems();
            }
            else
                s.IsSelected = true;
        }

        private void Disconnect(ServiceItem service)
        {
            service.Disconnect();
            service.IsExpanded = false;
            //monitoruc.service = null;
            //clientupdateruc.service = null;
            //serviceupdateruc.service = null;
            //hostinfouc.service = null;
            //modifypwduc.service = null;
            this.UcContainer.Children.Clear();
        }

        private void DeleteServiceItem()
        {
            if (MessageBoxResult.Yes == MessageBox.Show("是否移除当前服务节点？", "删除提示", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                Disconnect(current);
                services.Remove(current);
                SerializeServiceItems();
            }
        }

        void btnConnected_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ServiceConnected();
            if (dialog.ShowDialog().Value)
            {
                Addconnect(dialog.service);
            }
        }

        private void UCLoadData(BaseUserControl uc)
        {
            if (current != null && current != uc.service)
            {
                uc.LoadData(current);
            }
        }

        void tvObjectResourceManager_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNode node = e.NewValue as TreeNode;
            if (node != null)
            {
                if (node.Tag == null)
                {
                    var root = node as ServiceItem;
                    if (root != null)
                        current = root;
                }
                else
                {
                    //BaseUserControl newuc = null;
                    current = node.RootNode as ServiceItem;
                    if (node.Tag is BaseUserControl)
                    {
                        BaseUserControl newuc = node.Tag as BaseUserControl;
                        this.UcContainer.Children.Clear();
                        UCLoadData(newuc);
                        this.UcContainer.Children.Add(newuc);
                    }
                    //switch ((int)node.Tag)
                    //{
                    //    case 1: newuc = monitoruc; break;
                    //    case 2: newuc = hostinfouc; break;
                    //    case 3: newuc = loguc; break;

                    //    case 103: newuc = serviceupdateruc; break;
                    //    case 104: newuc = clientupdateruc; break;
                    //    case 105: newuc = modifypwduc; break;
                    //}
                    //if (newuc != null)
                    //{
                    //    this.UcContainer.Children.Clear();
                    //    UCLoadData(newuc);
                    //    this.UcContainer.Children.Add(newuc);
                    //}
                }
            }
        }

        // Invoked when we enter edit mode.
        void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            oldText = txt.Text;
            // Give the TextBox input focus
            txt.Focus();

            txt.SelectAll();
        }

        // Invoked when we exit edit mode.
        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetCurrentItemInEditMode(false);
        }

        // Invoked when the user edits the annotation.
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetCurrentItemInEditMode(false);
                SerializeServiceItems();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                ((TextBox)e.Source).Text = oldText;
                SetCurrentItemInEditMode(false);
                e.Handled = true;
            }
        }

        private void SerializeServiceItems()
        {
            var list = from s in services select s.service;
            configmanager.SerializeServiceItems(list.ToList());
        }

        private List<Service> DeserializeServiceItems()
        {
            return configmanager.DeserializeServiceItems();
        }
    }
}
