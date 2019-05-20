using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileSyncGui
{
    class FileSyncInfoEntity:EntityBase
    {
        public DateTime LastSyncTime
        {
            get { return getProperty<DateTime>("LastSyncTime"); }
            set { setProperty("LastSyncTime", value); }
        }
        public DateTime ServiceStartTime
        {
            get { return getProperty<DateTime>("ServiceStartTime"); }
            set { setProperty("ServiceStartTime", value); }
        }

        public string MiniServerHost
        {
            get { return getProperty<string>("MiniServerHost"); }
            set { setProperty("MiniServerHost", value); }
        }

        public string BizServerHost
        {
            get { return getProperty<string>("BizServerHost"); }
            set { setProperty("BizServerHost", value); }
        }

        public string MiniServerSyncFolder
        {
            get { return getProperty<string>("MiniServerSyncFolder"); }
            set { setProperty("MiniServerSyncFolder", value); }
        }

        public string CurrentSyncInfo
        {
            get { return getProperty<string>("CurrentSyncInfo"); }
            set { setProperty("CurrentSyncInfo", value); }
        }

        public int SyncCount
        {
            get { return getProperty<int>("SyncCount"); }
            set { setProperty("SyncCount", value); }
        }

        public bool IsAutoSync
        {
            get { return getProperty<bool>("IsAutoSync"); }
            set { setProperty("IsAutoSync", value); }
        }

        public int SyncInterval
        {
            get { return getProperty<int>("SyncInterval"); }
            set { setProperty("SyncInterval", value); }
        }

        public int FileProgress
        {
            get { return getProperty<int>("FileProgress"); }
            set { setProperty("FileProgress", value); }
        }

        public int FolderProgress
        {
            get { return getProperty<int>("FolderProgress"); }
            set { setProperty("FolderProgress", value); }
        }
    }
}
