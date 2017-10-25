using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Runtime.Principal;
using PWMIS.EnterpriseFramework.Service.Client.Model;

namespace PWMIS.EnterpriseFramework.Service.Runtime
{
    /// <summary>
    /// 基础服务接口
    /// </summary>
    public interface IService
    {

        /// <summary>
        /// 是否继续处理请求，如果不需要服务自动调用服务方法，请在当前方法中做处理，并返回False
        /// </summary>
        /// <param name="context">当前服务上下文</param>
        /// <returns>是否需要继续处理服务请求</returns>
        bool ProcessRequest(IServiceContext context);
        /// <summary>
        /// 请求的服务方法执行完成以后的操作
        /// </summary>
        /// <param name="context"></param>
        void CompleteRequest(IServiceContext context);

        /// <summary>
        /// (发布-订阅模式中)是否已经注销订阅服务
        /// </summary>
        bool IsUnSubscribe { get; }

    }

    /// <summary>
    /// 服务抽象类
    /// </summary>
    public abstract class ServiceBase : IService
    {
        /// <summary>
        /// 当前服务上下文
        /// </summary>
        public IServiceContext CurrentContext { get; set; }

        /// <summary>
        /// (发布-订阅模式中)是否已经注销订阅服务
        /// </summary>
        public bool IsUnSubscribe { get; private set; }

        /// <summary>
        /// 获取全局缓存，根据配置，可以支持分布式的缓存服务器
        /// </summary>
        public ICacheProvider GlobalCache
        {
            get
            {
                return CacheProviderFactory.GetGlobalCacheProvider();
            }
        }
        /// <summary>
        /// 注销订阅的服务，并执行其它清理资源的操作
        /// </summary>
        /// <returns></returns>
        public virtual bool UnSubscribeService()
        {
            if (this.CurrentContext.Session != null)
            {
                this.CurrentContext.Session.Clear();
            }
            this.IsUnSubscribe = true;
            return true;
        }

        /// <summary>
        /// 是否继续处理请求，如果不需要服务自动调用服务方法，请在当前方法中做处理，并返回False，如果需要自定义的处理，请重写该方法
        /// </summary>
        /// <param name="context">当前服务上下文</param>
        public virtual bool ProcessRequest(IServiceContext context)
        {
            this.CurrentContext = context;
            return true;
        }

        /// <summary>
        /// 请求的服务方法执行完成以后的操作
        /// </summary>
        /// <param name="context"></param>
        public virtual void CompleteRequest(IServiceContext context)
        {

        }
    }

    /// <summary>
    /// 服务上下文接口
    /// </summary>
    public interface IServiceContext
    {
        /// <summary>
        /// 请求服务
        /// </summary>
        ServiceRequest Request { get; set; }
        /// <summary>
        /// 响应服务
        /// </summary>
        ServiceResponse Response { get; set; }
        /// <summary>
        /// 服务关联的会话
        /// </summary>
        IServiceSession Session { get; set; }
        /// <summary>
        /// 系统缓存
        /// </summary>
        ICacheProvider Cache { get; }

        /// <summary>
        /// 服务是否必须要求依赖于会话状态
        /// </summary>
        bool SessionRequired { get; set; }
        /// <summary>
        /// 服务关联的用户对象
        /// </summary>
        ServiceIdentity User { get; set; }
        /// <summary>
        /// 服务所在的宿主
        /// </summary>
        ServiceHostInfo Host { get; set; }
        /// <summary>
        /// 指定服务是否可以并发执行,默认为并行
        /// </summary>
        bool ParallelExecute { get; set; }
        /// <summary>
        /// 每一批次的执行间隔时间，单位是毫秒，如果小于等于零，则不执行等待。默认为1秒
        /// </summary>
        int BatchInterval { get; set; }
        /// <summary>
        /// 执行的批次号
        /// </summary>
        int BatchIndex { get; set; }
        /// <summary>
        /// 回调客户端的函数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="para">参数</param>
        /// <returns>客户端返回的结果</returns>
        TResult CallBackFunction<T, TResult>(T para);

        /// <summary>
        /// 预先回调客户端的函数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="para">参数</param>
        /// <returns>客户端返回的结果</returns>
        TResult PreCallBackFunction<T, TResult>(T para);

        /// <summary>
        /// 是否向客户端发送空的结果，例如空的列表记录，或者结果为 NULL 的对象 
        /// </summary>
        bool SendEmptyResult { get; set; }
        /// <summary>
        /// 发布数据
        /// </summary>
        /// <param name="data"></param>
        void PublishData(object data);
        /// <summary>
        /// 服务发布数据的事件（框架内部使用）
        /// </summary>
        event EventHandler<ServiceEventArgs> OnPublishDataEvent;
        /// <summary>
        /// 获取发布事件源对象
        /// </summary>
        ServiceEventSource PublishEventSource { get; }
        /// <summary>
        /// 获取或者设置服务的会话模式
        /// </summary>
        SessionModel SessionModel { get; set; }
    }
}
