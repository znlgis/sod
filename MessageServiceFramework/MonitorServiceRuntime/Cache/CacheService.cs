using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranstarAuction.Service.Runtime;
using System.Runtime.Caching;

namespace TranstarAuction.Service.Runtime
{
    /// <summary>
    /// 缓存服务，获取或者设置缓存，如果插入缓存且原先的存在，则会移除旧缓存，插入新缓存
    /// </summary>
    public class CacheService : IService
    {
        public bool ProcessRequest(IServiceContext context)
        {
            string key = "";
            switch(context.Request.MethodName)
            {
                case "GetObject":
                    key = context.Request.Parameters[0].ToString();
                    object obj = context.Cache.Get<object>(key);
                    context.Response.WriteJsonString(obj);
                    break;
                case "SetObject":
                    key = context.Request.Parameters[0].ToString();
                    context.Cache.Remove(key);

                    object cacheObject = context.Request.Parameters[1];
                    if (context.Request.Parameters.Length > 2)
                    {
                        CacheItemPolicy policy = context.Request.Parameters[2] as CacheItemPolicy;
                        if (policy != null)
                        {
                            if (policy.SlidingExpiration != TimeSpan.Zero)//调试的时候显示的AbsoluteExpiration 是DateTimeOffset.MaxValue
                                policy.AbsoluteExpiration = DateTimeOffset.MaxValue; //实际上不是，必须设置才可以。
                            context.Cache.Insert<object>(key, cacheObject, policy);
                        }
                        else
                        {
                            context.Cache.Insert<object>(key, cacheObject);
                        }
                    }
                    else
                    {
                        context.Cache.Insert<object>(key, cacheObject);
                    }
                    context.Response.WriteJsonString(true);
                    break;
                case "RemoveObject":
                    key = context.Request.Parameters[0].ToString();
                    context.Cache.Remove(key);
                    context.Response.WriteJsonString(true);
                    break;
            }
           
            return false;
        }

        public void CompleteRequest(IServiceContext context)
        {
            
        }


        public bool IsUnSubscribe
        {
            get { return false; }
        }
    }

    public class CacheServiceObject
    { 
    
    }
}
