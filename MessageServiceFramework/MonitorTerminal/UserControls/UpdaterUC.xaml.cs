using System.Windows;
using System.Windows.Controls;
using TranstarAuction.Presenters.Presenter;
using MonitorTerminal.CustomEntities;
using System.Threading;
using TranstarAuction.Model;
using System;
using System.IO;

using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
 
using System.Linq;
using PWMIS.EnterpriseFramework.Service.Client.Model;

namespace MonitorTerminal.UserControls
{
    /// <summary>
    /// UpdaterUC.xaml 的交互逻辑
    /// </summary>
    public partial class UpdaterUC : BaseUserControl
    {
        private string CurrentDirectory = string.Empty;
        protected UpdateFilePresenter presenter = new UpdateFilePresenter();
        protected UploadFileType uploadtype;
        public event Action<string> UpdateComplete;
        public void OnUpdateComplete(string arg)
        {
            if (UpdateComplete != null)
                UpdateComplete(arg);
        }

        public UpdaterUC(UploadFileType type)
        {
            InitializeComponent();
            uploadtype = type;
            this.btnBrowser.Click += new RoutedEventHandler(btnBrowser_Click);
            this.btnUpLoad.Click += new RoutedEventHandler(btnUpLoad_Click);
            this.btnUpdateFiles.Click += new RoutedEventHandler(btnUpdateFiles_Click);
        }
        public override void CloseServiceProxy()
        {
            presenter.Close();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //Get clicked column
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                if (clickedColumn != null)
                {
                    //Get binding property of clicked column
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path;
                    SortDescriptionCollection sdc = this.lvDirectory.Items.SortDescriptions;
                    ListSortDirection sortDirection = ListSortDirection.Ascending;
                    if (sdc.Count > 0)
                    {
                        SortDescription sd = sdc[0];
                        sortDirection = (ListSortDirection)((((int)sd.Direction) + 1) % 2);
                        sdc.Clear();
                    }
                    RemoveUPNode();
                    sdc.Add(new SortDescription(bindingProperty, sortDirection));
                    AddUPNode();

                    GridViewColumnHeader headerClicked =              e.OriginalSource as GridViewColumnHeader;

                    if (sortDirection == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["ListViewHeaderTemplateDescendingSorting"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["ListViewHeaderTemplateAscendingSorting"] as DataTemplate;
                    }
                }
            }
        }

        protected RoutedEvent ItemDoubleClickEvent;

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = this.lvDirectory.SelectedItem as ServiceFileInfo;
            if (item != null)
            {
                if (item.Name.StartsWith("<DIR>"))
                {
                    //CurrentDirectory = Path.Combine(CurrentDirectory, item.Name.Remove(0, 5));
                    CurrentDirectory = item.Name.Remove(0, 5);
                    BindListView();
                }
                else
                {
                    if (item.Name.Equals(".")) //刷新
                    {
                        //CurrentDirectory = CurrentDirectory.Remove(CurrentDirectory.LastIndexOf('/'));
                        //if (CurrentDirectory.Length < (uploadtype == UploadFileType.ClientPacket ? "/WorkFolder" : "/").Length)
                        //    CurrentDirectory = uploadtype == UploadFileType.ClientPacket ? "/WorkFolder" : "/";
                        BindListView();
                    }
                    else if (item.Name.Equals("..")) //返回上级
                    {
                        //CurrentDirectory = uploadtype == UploadFileType.ClientPacket ? "/WorkFolder" : "/";
                        string[] arr = CurrentDirectory.Split('\\');
                        string newPath = "";
                        for (int i = 0; i < arr.Length - 1; i++)
                            if(arr[i]!="")
                                newPath +="\\"+ arr[i];

                        CurrentDirectory = newPath;
                        BindListView();
                    }
                }
            }
            //this.RaiseEvent(new RoutedEventArgs(ItemDoubleClickEvent, this.lvDirectory.SelectedItem));
        }

        void btnUpdateFiles_Click(object sender, RoutedEventArgs e)
        {//Update
            if (uploadtype == UploadFileType.ServicePacket)
            {
                var confirm = System.Windows.Forms.MessageBox.Show("Note:this operation will restart the service!\r\nAre you confirm?", "Information", System.Windows.Forms.MessageBoxButtons.OKCancel);
                if (confirm != System.Windows.Forms.DialogResult.OK) return;
            }
            else
            {
                var confirm = System.Windows.Forms.MessageBox.Show("Note:this operation will update cliect packets!\r\nAre you confirm?", "Information", System.Windows.Forms.MessageBoxButtons.OKCancel);
                if (confirm != System.Windows.Forms.DialogResult.OK) return;
            }
            this.btnUpLoad.IsEnabled = false;
            this.btnUpdateFiles.IsEnabled = false;
            presenter.ServiceProxy.ServiceBaseUri = service.ServiceUri;
            presenter.RequestUpdate(uploadtype);
            if (uploadtype == UploadFileType.ServicePacket)
            {
                //OnUpdateComplete("Service Update Complete!");
                System.Windows.Forms.MessageBox.Show("Service Update Complete!");
                this.service.Disconnect();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Client Packets Update Complete!");

                CurrentDirectory = uploadtype == UploadFileType.ClientPacket ? "/WorkFolder" : "/";
                BindListView();
            }
            this.btnUpLoad.IsEnabled = true;
            this.btnUpdateFiles.IsEnabled = true;
        }

        private bool VerifyDirectory()
        {
            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please select a directory!");
                return false;
            }
            try
            {
                if (!Directory.Exists(this.txtPath.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Please select a correct directory!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            } 
        }

        void btnUpLoad_Click(object sender, RoutedEventArgs e)
        {//Uploading
            if (!VerifyDirectory()) return;
            var confirm = System.Windows.Forms.MessageBox.Show("Are you confirm upload these files in selected directory?", "Information", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if (confirm != System.Windows.Forms.DialogResult.OK) return;
            this.btnUpLoad.IsEnabled = false;
            this.btnUpdateFiles.IsEnabled = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                presenter.ServiceProxy.ServiceBaseUri = service.ServiceUri;
                presenter.CallbackUploadFiles(obj.ToString(),
                    uploadtype,
                    g =>
                    {
                        if (g.State == UploadStateArg.UploadState.Uploading)
                            Dispatcher.BeginInvoke(new Action<string>(s => this.lblProgress.Content = s), g.ProgressFile);
                        else
                            Dispatcher.BeginInvoke(new Action<string>(s =>
                            {
                                this.btnUpLoad.IsEnabled = true;
                                this.btnUpdateFiles.IsEnabled = true;
                                this.lblProgress.Content = s;
                            }), g.msg);
                    });
            }), this.txtPath.Text);
        }

        void btnBrowser_Click(object sender, RoutedEventArgs e)
        {//Select Files Directory
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog(); 
            System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.txtPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentDirectory = uploadtype == UploadFileType.ClientPacket ? "/WorkFolder" : "/";
            BindListView();
        }

        protected void BindListView()
        {
            ManagePresenter presenter = new ManagePresenter();
            presenter.ServiceProxy.ServiceBaseUri = service.ServiceUri;
            List<ServiceFileInfo> files = presenter.GetDirectoryFiles(CurrentDirectory);
            //将文件列表，绑定到界面上，并可以点击目录查看文件列表
            if (files != null)
            {
                this.lvDirectory.ItemsSource = new BindingList<ServiceFileInfo>(files);
                AddUPNode();
            }
        }

        private void AddUPNode()
        {
            var collect = this.lvDirectory.ItemsSource as BindingList<ServiceFileInfo>;
            if (collect != null)
            {
                collect.Insert(0, new ServiceFileInfo() { Name = ".." });
                collect.Insert(0, new ServiceFileInfo() { Name = "." }); 
                //ICollectionView dataView =       CollectionViewSource.GetDefaultView(this.lvDirectory.ItemsSource);
                //dataView.Refresh();
            }
        }
        private void RemoveUPNode()
        {
            var collect = this.lvDirectory.ItemsSource as BindingList<ServiceFileInfo>;
            if (collect != null)
            {
                collect.Remove(collect.FirstOrDefault(o => o.Name.Equals(".")));
                collect.Remove(collect.FirstOrDefault(o => o.Name.Equals("..")));
            }
        }
    }
}
