using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Basic;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 会话容器
    /// </summary>
    public class SessionContainer
    {
        private Dictionary<string, IServiceSession> dictSession;

        #region SessionContainer 的单例实现
        private static readonly object _syncLock = new object();//线程同步锁；
        private static SessionContainer _instance;
        private SessionContainer()
        {
            dictSession = new Dictionary<string, IServiceSession>();
        }
        /// <summary>
        /// 返回 SessionContainer 的唯一实例；
        /// </summary>
        public static SessionContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SessionContainer();
                        }
                    }
                }
                return _instance;
            }
        }



        #endregion

        /// <summary>
        /// 获取会话对象
        /// </summary>
        /// <param name="sessionId">会话标示</param>
        /// <returns></returns>
        public IServiceSession GetSession(string sessionId)
        {
            if (dictSession.Keys.Contains(sessionId))
            {
                return dictSession[sessionId];
            }
            else
            {
                lock (_syncLock)
                {
                    if (dictSession.Keys.Contains(sessionId))
                    {
                        return dictSession[sessionId];
                    }
                    else
                    {
                        IServiceSession session = new ServiceSession(sessionId);//考虑使用IOC的构造函数注入
                        dictSession.Add(sessionId, session);
                        return session;
                    }
                }
            }
        }

    }
}
