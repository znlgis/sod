using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Client;
using System.IO;
using PWMIS.EnterpriseFramework.Service.Client.Model;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 内存缓存提供者类
    /// </summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private ObjectCache _mcache;
        private ObjectCache MCache
        {
            get
            {
                if (_mcache == null)
                    _mcache = MemoryCache.Default;
                return _mcache;
            }
        }
        public MemoryCacheProvider()
        {
        }

        #region ICacheProvider 成员

        public void Insert<T>(string key, T data)
        {
            MCache.Add(key, data, null);
        }

        public void Insert<T>(string key, T data, CacheItemPolicy settings)
        {
            MCache.Add(key, data, settings);
        }

        public T Get<T>(string key)
        {
            object result = MCache[key];
            if (result == null)
                return default(T);//T 有可能是值类型
            else
                return (T)MCache[key];
        }

        public T Get<T>(string cacheKey, Func<T> getData)
        {
            T tdata = Get<T>(cacheKey);
            if (tdata == null)
            {
                tdata = getData();
                Insert(cacheKey, tdata);
            }
            return tdata;
        }

        public void Remove(string key)
        {
            MCache.Remove(key);
        }

        public void Clear()
        {
            _mcache = MemoryCache.Default;
        }

        public T Get<T>(string cacheKey, Func<T> getData, CacheItemPolicy settings)
        {
            T tdata = Get<T>(cacheKey);
            if (tdata == null || tdata.Equals(default(T)))
            {
                tdata = getData();
                Insert(cacheKey, tdata, settings);
            }
            return tdata;
        }

        #endregion
    }

    /// <summary>
    /// 缓存服务器提供者,注意单个实例在多线程下可能会引起问题
    /// 注：当缓存中不存在该对象时，要返回null值并交由使用者处理
    ///        不要抛出异常，异常情况应该是说服务本身出了问题，例如：虚拟机宕机时
    ///        请注意处理下该情况。
    /// </summary>
    public class CacheServerProvider : ICacheProvider
    {
        const string LogDirectory = ".\\Log\\";
        public static FileSystemWatcher fsw = new FileSystemWatcher();
        public static List<ServiceRegModel> SRMList { get; set; }
        public List<ServiceRegModel> srmList
        {
            get { return CacheServerProvider.SRMList; }
            set { CacheServerProvider.SRMList = value; }
        }
        public Proxy ServiceProxy = new Proxy();
        IReadCacheStrategy CacheStrategy;

        static CacheServerProvider()
        {
            SRMList = new ServiceRegFile().Load(System.Configuration.ConfigurationManager.AppSettings["CacheConfigFile"]);
            fsw.Path = Environment.CurrentDirectory;
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Filter = Path.GetFileName(System.Configuration.ConfigurationManager.AppSettings["CacheConfigFile"]);
            fsw.IncludeSubdirectories = false;
            fsw.EnableRaisingEvents = true;
            fsw.Changed += new FileSystemEventHandler((s, e) =>
                SRMList = new ServiceRegFile().Load(System.Configuration.ConfigurationManager.AppSettings["CacheConfigFile"])
                );
        }

        public CacheServerProvider()
        {
            //ServiceProxy.ServiceUri = System.Configuration.ConfigurationManager.AppSettings["CacheServiceUri"];
            ServiceProxy.UseConnectionPool = true;
            ServiceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(ServiceProxy_ErrorMessage);
            CacheStrategy = new EffectiveStrategy(this);
        }

        protected void SaveCacheErrorMessage(string message)
        {
            try
            {
                string text = string.Format("\r\n[{0}] {1}\r\n", DateTime.Now.ToString(), message);
                System.IO.File.AppendAllText(LogDirectory + "CacheError.txt", text);
            }
            catch
            {

            }
        }

        void ServiceProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            SaveCacheErrorMessage("Cache ServiceProxy Error:" + e.MessageText);
        }

        public void Insert<T>(string key, T data)
        {
            this.Insert<T>(key, data, null);
        }

        public void Insert<T>(string key, T data, CacheItemPolicy settings)
        {
            if (SRMList != null && SRMList.Count > 0)
            {
                ServiceRequest request = new ServiceRequest();
                request.ServiceName = "CacheServer";
                request.MethodName = "SetObject";
                request.Parameters = settings == null ? new object[] { key, data } : new object[] { key, data, settings };

                foreach (var srm in SRMList)
                {
                    ServiceProxy.ServiceBaseUri = srm.GetUri();
                    try
                    {
                        ServiceProxy.RequestService<bool>(request, Common.DataType.Json, b => { });

                        //ServiceProxy.Connect();
                        //MessageConverter<bool> converter = ServiceProxy.GetServiceMessage<bool>(request, Common.DataType.Json);
                        //ServiceProxy.Close();
                        //if (!converter.Scceed)
                        //{
                        //    //当其中一个缓存服务宕机时不应该被终止，在此应该写日志
                        //    SaveCacheErrorMessage(string.Format("\r\n;插入缓存数据失败，位置1，Host:{0},Key:{1},ErrorMessage:{2}", srm.GetUri(), key, converter.ErrorMessage));
                        //}
                    }
                    catch (Exception ex)
                    {
                        //当其中一个缓存服务宕机时不应该被终止，在此应该写日志
                        SaveCacheErrorMessage(string.Format("\r\n;插入缓存数据失败，位置2，Host:{0},Key:{1},ErrorMessage:{2}", srm.GetUri(), key, ex.Message));
                    }
                }
            }
            else
                throw new Exception("缓存服务器列表为空");

            //ServiceRequest request = new ServiceRequest();
            //request.ServiceName = "CacheServer";
            //request.MethodName = "SetObject";
            //request.Parameters = settings == null ? new object[] { key, data } : new object[] { key, data, settings };

            //ServiceProxy.Connect();
            //MessageConverter<bool> converter = ServiceProxy.GetServiceMessage<bool>(request, Common.DataType.Json);
            // ServiceProxy.Close();
            //if (!converter.Scceed)
            //    throw new Exception("缓存插入失败。");
        }

        public T Get<T>(string key)
        {
            try
            {
                return CacheStrategy.Get<T>(key);
            }
            catch (Exception ex)
            {
                SaveCacheErrorMessage("GetCache Error:" + ex.Message);
            }
            return default(T);
        }

        public T Get<T>(string cacheKey, Func<T> getData)
        {
            T tdata = this.Get<T>(cacheKey);
            if (tdata == null)
            {
                tdata = getData();
                Insert(cacheKey, tdata);
            }
            return tdata;
        }

        public void Remove(string key)
        {
            if (SRMList != null)
            {
                ServiceRequest request = new ServiceRequest();
                request.ServiceName = "CacheServer";
                request.MethodName = "RemoveObject";
                request.Parameters = new object[] { key };

                foreach (var srm in SRMList)
                {
                    ServiceProxy.ServiceBaseUri = srm.GetUri();
                    try
                    {
                        ServiceProxy.RequestService<bool>(request, Common.DataType.Json, b => { });

                        //ServiceProxy.Connect();
                        //MessageConverter<bool> converter = ServiceProxy.GetServiceMessage<bool>(request, Common.DataType.Json);
                        //ServiceProxy.Close();
                        //if (!converter.Scceed)
                        //{
                        //    //当其中一个缓存服务宕机时不应该被终止，在此应该写日志
                        //}
                    }
                    catch
                    {
                        //当其中一个缓存服务宕机时不应该被终止，在此应该写日志
                    }
                }
            }

            //ServiceRequest request = new ServiceRequest();
            //request.ServiceName = "CacheServer";
            //request.MethodName = "RemoveObject";
            //request.Parameters = new object[] { key };

            //ServiceProxy.Connect();
            //MessageConverter<bool> converter = ServiceProxy.GetServiceMessage<bool>(request, Common.DataType.Json);
            // ServiceProxy.Close();
            //if (!converter.Scceed)
            //    throw new Exception("移除缓存失败：" + converter.ErrorMessage);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string cacheKey, Func<T> setInitData, CacheItemPolicy settings)
        {
            T tdata = this.Get<T>(cacheKey);
            if (tdata == null)
            {
                tdata = setInitData();
                Insert(cacheKey, tdata, settings);
            }
            return tdata;
        }
    }

    /*
     * 
 有三种控制缓存访问的策略：
1，主缓存策略，效率相对较高，但可能不稳定，且无法解决相对过期的问题；
2，随机有效缓存策略，采用相对过期的缓存方式，可能导致命中率越来越低；
3，并行策略：采用并行库，读取全部缓存服务器，谁最先返回，就用谁的结果给客户端，优点是速度快，缺点是服务器通信压力会比较大；

     * 
     */

    /// <summary>
    /// 读取缓存策略
    /// </summary>
    interface IReadCacheStrategy
    {
        T Get<T>(string key);
    }

    /// <summary>
    /// 主缓存策略，效率相对较高，但可能不稳定，且无法解决相对过期的问题
    /// </summary>
    public class FirstActiveStrategy : IReadCacheStrategy
    {
        CacheServerProvider CacheProvider;
        public FirstActiveStrategy(CacheServerProvider provider)
        {
            CacheProvider = provider;
        }

        public T Get<T>(string key)
        {
            if (CacheProvider.srmList != null && CacheProvider.srmList.Count > 0)
            {
                ServiceRequest request = new ServiceRequest();
                request.ServiceName = "CacheServer";
                request.MethodName = "GetObject";
                request.Parameters = new object[] { key };

                var srm = CacheProvider.srmList.Find(o => o.IsActive);
                if (srm != null)
                {
                    CacheProvider.ServiceProxy.ServiceBaseUri = srm.GetUri();
                    CacheProvider.ServiceProxy.Connect();
                    MessageConverter<T> converter = CacheProvider.ServiceProxy.GetServiceMessage<T>(request, Common.DataType.Json);
                    CacheProvider.ServiceProxy.Close();
                    if (converter.Succeed)
                    {
                        return converter.Result;
                    }
                    else
                        throw new Exception("获取缓存失败：" + converter.ErrorMessage);
                }
                else
                    throw new Exception("未找到可用的缓存服务器");
            }
            else
                throw new Exception("缓存服务器列表为空");
        }
    }

    /// <summary>
    /// 随机有效缓存策略，采用相对过期的缓存方式，可能导致命中率越来越低
    /// </summary>
    public class EffectiveStrategy : IReadCacheStrategy
    {
        //Proxy ServiceProxy = new Proxy();
        CacheServerProvider CacheProvider;
        public EffectiveStrategy(CacheServerProvider provider)
        {
            CacheProvider = provider;
        }

        public T Get<T>(string key)
        {
            if (CacheProvider.srmList != null && CacheProvider.srmList.Count > 0)
            {
                ServiceRequest request = new ServiceRequest();
                request.ServiceName = "CacheServer";
                request.MethodName = "GetObject";
                request.Parameters = new object[] { key };

                List<int> indexlist = new List<int>(CacheProvider.srmList.Count);
                for (int i = 0; i < CacheProvider.srmList.Count; i++) indexlist.Add(i);
                Random rnd = new Random();
                string errMsg = "";
                while (indexlist.Count > 0)
                {
                    int srmindex = rnd.Next(indexlist.Count);
                    var srm = CacheProvider.srmList[srmindex];
                    CacheProvider.ServiceProxy.ServiceBaseUri = srm.GetUri();
                    try
                    {
                        if (CacheProvider.ServiceProxy.Connect())
                        {
                            CacheProvider.ServiceProxy.ServiceSubscriber.TimeOut = 30000;//30秒
                            MessageConverter<T> converter = CacheProvider.ServiceProxy.GetServiceMessage<T>(request, Common.DataType.Json);
                            CacheProvider.ServiceProxy.Close();
                            if (converter != null && converter.Succeed && !(converter.Result == null || converter.Result.Equals(default(T))))
                            {
                                return converter.Result;
                            }
                            else
                            {
                                indexlist.RemoveAt(srmindex);
                                //
                                //此处应该记录日志，以分析缓存命中率
                                // string.Format("\r\n;获取缓存数据失败，Host:{0},Key:{1},ErrorMessage:{2}",srm.GetUri(), key,converter.ErrorMessage);
                            }
                        }
                        else
                        {
                            indexlist.RemoveAt(srmindex);
                            errMsg += string.Format("\r\n;获取缓存数据失败，Host:{0},Key:{1},ErrorMessage:{2}", srm.GetUri(), key, "服务器连接失败");
                        }

                    }
                    catch (Exception ex)
                    {
                        indexlist.RemoveAt(srmindex);
                        //当其中一个缓存服务宕机时不应该被终止，在此应该写日志 
                        errMsg += string.Format("\r\n;访问缓存服务器失败，Host:{0},Key:{1},ErrorMessage:{2}", srm.GetUri(), key, ex.Message);
                    }
                }
                if (errMsg != "")
                    throw new Exception("获取缓存失败:" + errMsg);
                else
                    return default(T);
            }
            else
                throw new Exception("缓存服务器列表为空");
        }
    }

    //策略3：采用并行库，读取全部缓存服务器，谁最先返回，就用谁的结果给客户端，优点是速度快，缺点是服务器通信压力会比较大
}
