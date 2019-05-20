
using PWMIS.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncGui
{
    class MainViewModel
    {
        private int waiteTimes = 0;
        public FileSyncInfoEntity FileSyncInfo { get; set; }
        public bool IsStarting { get; private set; }

        public StartSyncCommand StartSync { get; private set; }
        /// <summary>
        /// 开始同步事件
        /// </summary>
        public event EventHandler OnStartSync;

        FilesSyncModel Model;

        public IMvvmForm View;

        public System.Collections.IList UploadFiles { get; set; }

        private int times = 0;

        public MainViewModel()
        {
            string serverHost = string.Format("{0}:{1}",
              Program.ServerIP,
              Program.ServerPort);

            this.FileSyncInfo = new FileSyncGui.FileSyncInfoEntity();
            this.FileSyncInfo.IsAutoSync = Program.IsAutoSync ;//配置参数
            this.FileSyncInfo.MiniServerHost = serverHost;
            //this.FileSyncInfo.BizServerHost
            //this.FileSyncInfo.CurrentSyncInfo
            //this.FileSyncInfo.LastSyncTime
            //this.FileSyncInfo.MiniServerSyncFolder = "";
            this.FileSyncInfo.ServiceStartTime = DateTime.Now;
            this.FileSyncInfo.SyncCount = 0;//自服务启动以来，同步的次数
            this.FileSyncInfo.SyncInterval = Program.SyncInterval;//分钟，配置参数
            this.waiteTimes = this.FileSyncInfo.SyncInterval * 60;

            this.StartSync = new FileSyncGui.StartSyncCommand(this);
            this.Model = new FileSyncGui.FilesSyncModel();
            this.Model.OnFileUploading += Model_OnFileUploading;
            this.Model.OnUploadError += Model_OnUploadError;
            this.Model.OnUploadFinish += Model_OnUploadFinish;
        }

        private void Init()
        {
            //var serviceInfoTask = Model.ServiceInfo(serverHost);
            //serviceInfoTask.ConfigureAwait(false);
            //serviceInfoTask.Wait();
            //var serviceInfo = serviceInfoTask.Result;
            View.FormInvoke(() => {
                try
                {
                    var serviceInfo = Model.ServiceInfo(this.FileSyncInfo.MiniServerHost);
                    this.FileSyncInfo.BizServerHost = serviceInfo.Find(p => p.Key == "BizServer").Value;
                    this.FileSyncInfo.MiniServerSyncFolder = serviceInfo.Find(p => p.Key == "UploadPath").Value;
                }
                catch
                {
                    this.FileSyncInfo.CurrentSyncInfo = this.FileSyncInfo.MiniServerHost+ " 不可访问！";
                }
                
            });
            
        }

        private void Model_OnUploadFinish(object sender, FilesUploadResultEventArg e)
        {
            string msg = string.Format("上传结束，成功：{0},总文件数量：{1}，消息：{2}",
               e.Result.Success,
                e.Result.FilesCount,
                e.Result.Message);
            View.FormInvoke(() =>
            {
                this.FileSyncInfo.CurrentSyncInfo = msg;
                this.FileSyncInfo.LastSyncTime = DateTime.Now;
                this.FileSyncInfo.SyncCount++;
                this.FileSyncInfo.FileProgress = 100;
                this.FileSyncInfo.FolderProgress = 100;
            });
        }

        private void Model_OnUploadError(object sender, UploadErrorEventArg e)
        {
            View.FormInvoke(() => {
                this.FileSyncInfo.CurrentSyncInfo = e.ErrorMessage;
            });
           
        }

        private void Model_OnFileUploading(object sender, FileUploadEventArg e)
        {
            View.FormInvoke(() =>
            {
                this.FileSyncInfo.FileProgress = e.StateArg.ProcessValue;
                this.FileSyncInfo.FolderProgress = e.StateArg.TotalProcessValue;
                string msg = string.Format("正在上传文件【{0}】，状态【{1}】:{2}",
                    e.StateArg.ProgressFile, e.StateArg.State, e.StateArg.Message);
                this.FileSyncInfo.CurrentSyncInfo = msg;
                if (e.StateArg.State == Common.Service.DTO.UploadState.Success)
                {
                    if (this.FileSyncInfo.SyncCount > 100 && UploadFiles.Count>200)
                    {
                        UploadFiles.Clear();
                    }
                    UploadFiles.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" "+  e.StateArg.ProgressFile);
                }
                
            });
        }

        /// <summary>
        /// 设置定时间隔
        /// </summary>
        /// <param name="sec">秒</param>
        public int SetTimerTick(int sec)
        {
            //if (times == 0)
            //    this.Init();
            if (IsStarting || this.FileSyncInfo.IsAutoSync)
            {
                times += sec;
                if (times >= this.waiteTimes)
                {
                    StartTask();
                    times = 0;
                }
                return this.waiteTimes - times;
            }
            return 0;
        }

        /// <summary>
        /// 启动同步任务
        /// </summary>
        public void StartTask()
        {
            IsStarting = true;
            //调用同步服务
            if (!Model.Working)
            {
                Model.Sync(this.FileSyncInfo.MiniServerHost);

                if (OnStartSync != null)
                    OnStartSync(this, new EventArgs());
            }
        }
    }

    class StartSyncCommand : IMvvmCommand
    {
        MainViewModel ViewModel;

        public StartSyncCommand(MainViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public void AfterExecute()
        {
            //throw new NotImplementedException();
        }

        public bool BeforExecute(object para)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void Execute(object para)
        {
            ViewModel.FileSyncInfo.ServiceStartTime = DateTime.Now;
            ViewModel.FileSyncInfo.SyncCount += 1;
            ViewModel.StartTask();
        }
    }
}
