using Sod.Pwmis.com.Interface.Infrastructure;
using PWMIS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Repository
{
    public abstract class UowDbContext : DbContext, IUowManager
    {
        private string? currentTransName = "";
        private IGMT? GMTServer;
        public UowDbContext(string connName, IGMT? gmtServer=null) : base(connName)
        {
            ID = Guid.NewGuid();
            GMTServer = gmtServer;
        }

        public Guid ID { get; private set; }
        public bool IsGlobalTrans { get; set; }

        private void WriteTransInfo(string? logText = null)
        {
            if (!string.IsNullOrEmpty(currentTransName))
            {
                if (logText == null)
                    logText = "事务名称：" + currentTransName;
                Console.WriteLine(logText);
                base.CurrentDataBase.Logger.WriteLog(logText);
            }
        }

        public void BeginTransaction(string? transName = "")
        {
            currentTransName = transName;
            Console.WriteLine("开始事务。。。。" + transName);
            base.CurrentDataBase.BeginTransaction();
            WriteTransInfo();
            if (!string.IsNullOrEmpty(transName) && GMTServer != null)
            {
                this.IsGlobalTrans = GMTServer.BeginGlobalTran(transName);
            }
        }

        public bool Commit()
        {
            WriteTransInfo("开始提交事务...");
            try
            {
                if (this.IsGlobalTrans)
                {
                    var result = GMTServer?.PrepareCommit(1000 * 60);
                    if (result != GMT_COMMIT_RESULT.COMMIT)
                        throw new Exception("GMT XA Prepare Result:" + result);
                }
                base.CurrentDataBase.Commit();
                WriteTransInfo("事务提交成功!");
                return true;
            }
            catch (Exception ex)
            {
                WriteTransInfo("事务提交异常，错误信息：" + ex.Message);
                this.Rollback();
            }
            return false;
        }

        public void Rollback()
        {
            WriteTransInfo("回滚事务");
            base.CurrentDataBase.Rollback();
            if (this.IsGlobalTrans)
            {
                GMTServer?.Rallback();
            }
        }
    }
}
