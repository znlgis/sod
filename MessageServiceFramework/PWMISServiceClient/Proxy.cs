/*
 * New  12.28
 */
using System;
using System.Collections.Generic;
using System.Text;
using MessageSubscriber;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
using System.Threading;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.Service.Client
{
    /// <summary>
    /// 服务代理类 2012.5.3 整理
    /// </summary>
    public class Proxy : IDisposable
    {
        #region 私有对象和方法
        private System.Threading.Timer heartBeatTimer = null;

        private void RaiseSubscriberError(object sender, MessageEventArgs e)
        {
            if (this.ErrorMessage != null)
                this.ErrorMessage(sender, e);
        }

        private void ServiceSubscriber_HeartBeatError(object sender, MessageEventArgs e)
        {
            if (heartBeatTimer != null)
            {
                heartBeatTimer.Dispose();
                heartBeatTimer = null;
            }
            RaiseSubscriberError(sender, e);
        }

        

        /// <summary>
        /// 检查连接是否可用，如果不可用，则重新打开连接
        /// </summary>
        private void CheckConnect()
        {
            if (ServiceSubscriber == null || ServiceSubscriber.Closed)
            {
                //throw new InvalidOperationException("未打开连接或者连接已经关闭，请先调用Connect 方法");
                RaiseSubscriberError(this, new MessageEventArgs("未打开连接或者连接已经关闭，程序将重新连接服务"));
                this.Close();
                this.Connect();
            }
        }

        private void ProcessRemoteMessage<T>(string remoteMsg, DataType resultDataType, Connection conn, Action<T> action)
        {
            if (conn != null)
                conn.Close();
            string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
            if (errMsg != string.Empty)
            {
                RaiseSubscriberError(this, new MessageEventArgs(errMsg));
            }
            else
            {
                MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                if (convert.Succeed)
                {
                    if (action != null)
                        action(convert.Result);
                }
                else
                {
                    RaiseSubscriberError(convert, new MessageEventArgs(convert.ErrorMessage));
                }
            }
        }

        #endregion

        #region 公开的事件和属性
        /// <summary>
        /// 处理服务的异常信息
        /// </summary>
        public event EventHandler<MessageEventArgs> ErrorMessage;
        /// <summary>
        /// 获取服务订阅者
        /// </summary>
        public Subscriber ServiceSubscriber { get; private set; }
        /// <summary>
        /// 服务的基础地址
        /// </summary>
        public string ServiceBaseUri { get; set; }

        #endregion

        #region 同步 请求-响应

        /// <summary>
        /// 以同步的方式（阻塞），获取服务执行的结果消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <param name="resultDataType">消息的数据类型</param>
        /// <returns>消息转换器对象，如果不成功，返回空值</returns>
        public MessageConverter<T> GetServiceMessage<T>(string reqSrvUrl, DataType resultDataType)
        {
            CheckConnect();
            //ServiceSubscriber.SendMessage(reqSrvUrl);

            string remoteMsg = ServiceSubscriber.RequestMessage(reqSrvUrl);
            string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
            if (errMsg != string.Empty)
            {
                RaiseSubscriberError(this, new MessageEventArgs(errMsg));
                return null;
            }
            else
            {
                MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                return convert;
            }
        }

        /// <summary>
        /// 以同步的方式（阻塞），获取服务执行的结果消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <returns>消息转换器对象，如果不成功，返回空值</returns>
        public MessageConverter<T> GetServiceMessage<T>(string reqSrvUrl)
        {
            DataType resultDataType = MessageConverter<T>.GetResponseDataType();
            return GetServiceMessage<T>(reqSrvUrl, resultDataType);
        }

       

        /// <summary>
        /// 以同步的方式（阻塞），获取服务执行的结果消息。如果连接未打开，会尝试自动打开。
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="request">消息请求对象</param>
        /// <param name="resultDataType">消息的数据类型</param>
        /// <returns>消息转换器对象</returns>
        public MessageConverter<T> GetServiceMessage<T>(ServiceRequest request, DataType resultDataType)
        {
            request.RequestModel = RequestModel.GetService;
            return GetServiceMessage<T>(request.ServiceUrl, resultDataType);
        }

        #endregion

        #region 异步 请求-响应 的方法
        /// <summary>
        /// 请求远程服务并异步执行自定义的操作
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        public void RequestService<T>(string reqSrvUrl, DataType resultDataType, Action<T> action)
        {
            ////原来的方式
            //Subscriber sub = new Subscriber(ServiceUri);
            //sub.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceSubscriber_ErrorMessage);
            //sub.Subscribe();
            //if (!sub.Closed)
            //{
            //    sub.SendMessage(reqSrvUrl, null, (remoteMsg) =>
            //    {
            //        MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
            //        sub.Dispose();
            //        if (action != null)
            //            action(convert.Result);
            //    });
            //}
            //else
            //{
            //    sub.Dispose();
            //}

            //注意：由于本方法被频繁调用，不能这样使用连接池
            //使用连接池
            //if (this.Connect())
            //{
            //    this.ServiceSubscriber.SendMessage(reqSrvUrl, null, (remoteMsg) =>
            //    {
            //        MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
            //        this.Close();
            //        if (action != null)
            //            action(convert.Result);
            //    });
            //}
            //else
            //{
            //    this.Close();
            //}

            //新的使用连接池的方式
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            conn.ErrorMessage += new EventHandler<MessageEventArgs>(RaiseSubscriberError);
            if (conn.Open())
            {
                conn.RequestService(reqSrvUrl, null, (remoteMsg) =>
                {
                    ProcessRemoteMessage(remoteMsg, resultDataType, conn, action);
                });
            }
            else
            {
                conn.Close();
            }
        }

       

  
        /// <summary>
        /// 请求一个服务，然后异步处理结果。注意如果没有订阅异常处理事件（ErrorMessage），将在Task 上抛出异常。
        /// </summary>
        /// <typeparam name="T">服务方法的结果类型</typeparam>
        /// <param name="reqSrvUrl">服务的地址</param>
        /// <param name="resultDataType">服务返回的数据类型</param>
        /// <returns>服务结果</returns>
        public Task<T> RequestServiceAsync<T>(string reqSrvUrl, DataType resultDataType) 
        {
            var tcs = new TaskCompletionSource<T>();
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            conn.ErrorMessage += new EventHandler<MessageEventArgs>(
                (object sender, MessageEventArgs e) => {
                    if (this.ErrorMessage != null)
                    {
                        this.ErrorMessage(sender, new MessageEventArgs(e.MessageText));
                        tcs.SetCanceled();
                    }
                    else
                        tcs.SetException(new Exception(e.MessageText));
                }
                );
          

            if (conn.Open())
            {
                conn.RequestService(reqSrvUrl, null, (remoteMsg) =>
                {
                    conn.Close();
                    string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
                    if (errMsg != string.Empty)
                    {
                        if (this.ErrorMessage != null)
                        {
                            this.ErrorMessage(this, new MessageEventArgs(errMsg));
                            tcs.SetCanceled();
                        }
                        else
                        {
                            //即使抛出了异常，也必须设置任务线程的异常，否则异步方法没法返回
                            tcs.SetException(new Exception(errMsg));
                        }
                    }
                    else
                    {
                        MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                        if (convert.Succeed)
                        {
                            tcs.SetResult(convert.Result);
                        }
                        else
                        {
                            ////尝试转换Task<T>
                            ////类似：{"Result":3,"Id":6,"Exception":null,"Status":5,"IsCanceled":false,"IsCompleted":true,"CreationOptions":0,"AsyncState":null,"IsFaulted":false}
                            //string[] keyvalues= remoteMsg.TrimStart('{').TrimEnd('}').Split(',');
                            //if (keyvalues.Length > 5 && keyvalues[0].StartsWith("\"Result\":"))
                            //{
                            //    string msgData = keyvalues[0].Split(':')[1];

                            //}
                            //else
                            //{ 
                            
                            //}
                            //MessageConverter<Task<T>> convert2 = new MessageConverter<Task<T>>(remoteMsg, DataType.Json);
                            //if (convert2.Succeed)
                            //{
                            //    T value = convert2.Result.Result;
                            //    tcs.SetResult(value);
                            //}
                            //else
                            //{
                                
                            //}

                            //
                            errMsg = "resultDataType 指定错误:" + convert.ErrorMessage;
                            if (this.ErrorMessage != null)
                            {
                                this.ErrorMessage(this, new MessageEventArgs(errMsg));
                                tcs.SetCanceled();
                            }
                            else
                            {
                                //即使抛出了异常，也必须设置任务线程的异常，否则异步方法没法返回
                                tcs.SetException(new Exception(errMsg));
                            }
                        }
                    }

                });
            }
            else
            {
                conn.Close();
                tcs.SetCanceled();
            }
            return tcs.Task;
        }

        /// <summary>
        /// 请求一个服务，然后异步处理结果。注意如果没有订阅异常处理事件（ErrorMessage），将在Task 上抛出异常。
        /// </summary>
        /// <typeparam name="T">服务方法的结果类型</typeparam>
        /// <param name="request">"服务请求"对象</param>
        /// <returns>异步任务对象</returns>
        public Task<T> RequestServiceAsync<T>(ServiceRequest request)
        {
            string reqSrvUrl = request.ServiceUrl;
            DataType resultDataType = MessageConverter<T>.GetResponseDataType();
            return RequestServiceAsync<T>(reqSrvUrl, resultDataType);
        }

        /// <summary>
        /// 请求一个服务，然后异步处理结果，并运行服务端处理过程中回调客户端的函数。注意如果没有订阅异常处理事件（ErrorMessage），将在Task 上抛出异常。
        /// </summary>
        /// <typeparam name="T">服务方法的结果类型</typeparam>
        /// <typeparam name="TFunPara">要执行服务中间调用的回调函数的参数类型</typeparam>
        /// <typeparam name="TFunResult">要执行服务中间调用的回调函数的结果类型</typeparam>
        /// <param name="request">"服务请求"对象</param>
        /// <param name="function">中间过程客户端回调函数</param>
        /// <returns>异步任务对象</returns>
        public Task<T> RequestServiceAsync<T, TFunPara, TFunResult>(ServiceRequest request, MyFunc<TFunPara, TFunResult> function)
        {
            string reqSrvUrl = request.ServiceUrl;
            DataType resultDataType = MessageConverter<T>.GetResponseDataType();
            return RequestServiceAsync<T, TFunPara, TFunResult>(reqSrvUrl, resultDataType, function);
        }

        /// <summary>
        /// 请求远程服务并异步执行自定义的操作，在得到最终结果前，允许服务端回调客户端指定的函数，作为服务端计算需要的中间结果
        /// </summary>
        /// <typeparam name="T">最终执行的回调方法参数类型</typeparam>
        /// <typeparam name="TFunPara">要执行服务中间调用的回调函数的参数类型</typeparam>
        /// <typeparam name="TFunResult">要执行服务中间调用的回调函数的结果类型</typeparam>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        /// <param name="function">作为服务端计算需要回调客户端提供的中间结果函数</param>
        public void RequestService<T, TFunPara, TFunResult>(string reqSrvUrl, DataType resultDataType, Action<T> action, MyFunc<TFunPara, TFunResult> function)
        {
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            if (conn.Open())
            {
                conn.RequestService(reqSrvUrl, null, (remoteMsg) =>
                {
                    conn.Close();
                    MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                    if (convert.Succeed)
                    {
                        if (action != null)
                            action(convert.Result);
                    }
                    else
                    {
                        string errMsg = "参数 resultDataType 类型跟方法的返回类型不匹配，内部错误信息：" + convert.ErrorMessage;
                        RaiseSubscriberError(convert, new MessageEventArgs(errMsg));
                    }
                },
                para =>
                {
                    MessageConverter<TFunPara> convert = new MessageConverter<TFunPara>(para);
                    TFunResult result = function(convert.Result);

                    MessageConverter<TFunResult> convertFunResult = new MessageConverter<TFunResult>();
                    string strResult = convertFunResult.Serialize(result);
                    //检查转换是否成功,convertFunResult.MessageText 可以获取结果的原始值
                    return strResult;
                }

                );
            }
            else
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 异步请求服务，并允许服务在执行过程中，回调客户端函数
        /// </summary>
        /// <typeparam name="T">服务的结果返回类型</typeparam>
        /// <typeparam name="TFunPara">回调函数的参数类型</typeparam>
        /// <typeparam name="TFunResult">回调函数的结果类型</typeparam>
        /// <param name="reqSrvUrl">请求服务的URL地址</param>
        /// <param name="resultDataType">结果数据类型</param>
        /// <param name="function">回调函数</param>
        /// <returns>异步任务对象</returns>
        public Task<T> RequestServiceAsync<T, TFunPara, TFunResult>(string reqSrvUrl, DataType resultDataType, MyFunc<TFunPara, TFunResult> function)
        {
            var tcs = new TaskCompletionSource<T>();
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            conn.ErrorMessage += new EventHandler<MessageEventArgs>(
                (object sender, MessageEventArgs e) =>
                {
                    if (this.ErrorMessage != null)
                    {
                        this.ErrorMessage(sender, new MessageEventArgs(e.MessageText));
                        tcs.SetCanceled();
                    }
                    else
                        tcs.SetException(new Exception(e.MessageText));
                }
                );


            if (conn.Open())
            {
                conn.RequestService(reqSrvUrl, null, (remoteMsg) =>
                {
                    conn.Close();
                    string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
                    if (errMsg != string.Empty)
                    {
                        if (this.ErrorMessage != null)
                        {
                            this.ErrorMessage(this, new MessageEventArgs(errMsg));
                            tcs.SetCanceled();
                        }
                        else
                        {
                            //即使抛出了异常，也必须设置任务线程的异常，否则异步方法没法返回
                            tcs.SetException(new Exception(errMsg));
                        }
                    }
                    else
                    {
                        MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);

                        tcs.SetResult(convert.Result);
                    }

                },
                para =>
                {
                    MessageConverter<TFunPara> convert = new MessageConverter<TFunPara>(para);
                    TFunResult result = function(convert.Result);

                    MessageConverter<TFunResult> convertFunResult = new MessageConverter<TFunResult>();
                    string strResult = convertFunResult.Serialize(result);
                    //检查转换是否成功,convertFunResult.MessageText 可以获取结果的原始值
                    return strResult;
                }

                );
            }
            else
            {
                conn.Close();
                tcs.SetCanceled();
            }
            return tcs.Task;
        }


        /// <summary>
        /// 请求远程服务并异步执行自定义的操作，在进行服务方法的操作过程中，允许预先进行一部分计算，例如先返回一些客户端需要的结果，
        /// 然后执行过程中，服务端再回调回调客户端指定的函数，作为服务端计算需要的中间结果
        /// </summary>
        /// <typeparam name="T">最终执行的回调方法参数类型</typeparam>
        /// <typeparam name="TFunPara">要执行服务中间调用的会掉函数的参数类型</typepar，am>
        /// <typeparam name="TFunResult">要执行服务中间调用的会掉函数的结果类型</typeparam>
        /// <typeparam name="TPreFunPara">作为服务端计算需前的结果函数的参数类型</typeparam>
        /// <typeparam name="TPreFunResult">作为服务端计算需前的结果函数的类型</typeparam>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        /// <param name="function">作为服务端计算需要的中间结果函数</param>
        /// <param name="preFunction">作为服务端计算需前的结果函数</param>
        public void RequestService<T, TFunPara, TFunResult, TPreFunPara, TPreFunResult>(
            string reqSrvUrl,
            DataType resultDataType,
            Action<T> action,
            MyFunc<TFunPara, TFunResult> function,
            MyFunc<TPreFunPara, TPreFunResult> preFunction)
        {
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            if (conn.Open())
            {
                conn.RequestService(reqSrvUrl, null, (remoteMsg) =>
                {
                    //MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                    //conn.Close();
                    //if (action != null)
                    //    action(convert.Result);
                    ProcessRemoteMessage<T>(remoteMsg, resultDataType, conn, action);
                },
                para =>
                {
                    MessageConverter<TFunPara> convert = new MessageConverter<TFunPara>(para);
                    TFunResult result = function(convert.Result);

                    MessageConverter<TFunResult> convertFunResult = new MessageConverter<TFunResult>();
                    string strResult = convertFunResult.Serialize(result);
                    //检查转换是否成功,convertFunResult.MessageText 可以获取结果的原始值
                    return strResult;
                },
                prePara =>
                {
                    MessageConverter<TPreFunPara> convert = new MessageConverter<TPreFunPara>(prePara);
                    TPreFunResult result = preFunction(convert.Result);

                    MessageConverter<TPreFunResult> convertFunResult = new MessageConverter<TPreFunResult>();
                    string strResult = convertFunResult.Serialize(result);
                    //检查转换是否成功,convertFunResult.MessageText 可以获取结果的原始值
                    return strResult;
                }
                );
            }
            else
            {
                conn.Close();
            }

        }

        /// <summary>
        /// 请求远程服务并且处理返回消息
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="request">"服务请求"对象</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理的回调方法</param>
        public void RequestService<T>(ServiceRequest request, DataType resultDataType, Action<T> action)
        {
            string reqSrvUrl = request.ServiceUrl;
            RequestService<T>(reqSrvUrl, resultDataType, action);
        }

        /// <summary>
        /// 请求远程服务并且处理返回消息
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="request">"服务请求"对象</param>
        /// <param name="action">自定义的处理方法</param>
        public void RequestService<T>(ServiceRequest request, Action<T> action)
        {
            string reqSrvUrl = request.ServiceUrl;
            DataType resultDataType = MessageConverter<T>.GetResponseDataType();
            RequestService<T>(reqSrvUrl, resultDataType, action);
        }

        /*
         * 简化版本，实际详细的，请参考上面的RequestServiceAsync ，有更多的异常信息封装
         * RequestServiceAsync 改写之前，方法原型是：
            RequestServiceAsync<T>(ServiceRequest request, DataType resultDataType,Action<T> action)
            {....}
         * 
         *  修改时间：2016.12.9
         * 
        /// <summary>
        /// 异步请求远程服务，结果将在线程池中取一个线程异步处理。
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="request">"服务请求"对象</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        public Task<T> RequestServiceAsync<T>(ServiceRequest request, DataType resultDataType)
        {
            string reqSrvUrl = request.ServiceUrl;
            var tcs = new TaskCompletionSource<T>();
            try
            {
               RequestService<T>(reqSrvUrl, resultDataType, t=> tcs.SetResult(t));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

       

        //http://stackoverflow.com/questions/16830601/convert-actiont-callback-to-an-await
        //var result = await AsAsync<string>(MethodWithCallback);

        Task<T> AsAsync<T>(Action<Action<T>> target)
        {
            var tcs = new TaskCompletionSource<T>();
            try
            {
                target(t => tcs.SetResult(t));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

         *
         */

        /// <summary>
        /// 在发布订阅模式下，以同一个连接，请求服务并执行指定的处理方法
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="sub">正在使用的订阅对象</param>
        /// <param name="reqSrvUrl">服务地址</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        public void RequestService<T>(Subscriber sub, string reqSrvUrl, DataType resultDataType, Action<T> action)
        {
            //if (sub == null || sub.Closed)
            //    throw new InvalidOperationException("未打开连接或者连接已经关闭，请先调用Connect 方法");
            CheckConnect();
            if (sub != null)
            {
                sub.SendMessage(reqSrvUrl, null, (remoteMsg) =>
                {
                    //MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                    //if (action != null)
                    //    action(convert.Result);
                    // 上面代码已经优化
                    ProcessRemoteMessage(remoteMsg, resultDataType, null, action);
                });
            }
        }

        /// <summary>
        /// 在发布订阅模式下，以同一个连接，请求服务并执行指定的处理方法
        /// </summary>
        /// <typeparam name="T">要处理的对象类型</typeparam>
        /// <param name="sub">正在使用的订阅对象</param>
        /// <param name="request">服务请求对象</param>
        /// <param name="resultDataType">返回消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        public void RequestService<T>(Subscriber sub, ServiceRequest request, DataType resultDataType, Action<T> action)
        {
            RequestService<T>(sub, request.ServiceUrl, resultDataType, action);
        }

        /// <summary>
        /// 在发布订阅模式下，以同一个连接，请求远程服务并异步执行自定义的操作
        /// </summary>
        /// <typeparam name="T">父类对象</typeparam>
        /// <typeparam name="T2">结果要转换的兼容类对象</typeparam>
        /// <param name="request"></param>
        /// <param name="resultDataType"></param>
        /// <param name="action"></param>
        public void RequestService<T, T2>(ServiceRequest request, DataType resultDataType, Action<T2> action)
        {
            ////原有的方式
            //Subscriber sub = new Subscriber(ServiceUri);
            //sub.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceSubscriber_ErrorMessage);
            //sub.Subscribe();
            //if (!sub.Closed)
            //{
            //    sub.SendMessage(request.ServiceUrl, typeof(T), (remoteMsg) =>
            //    {
            //        MessageConverter<T2> convert = new MessageConverter<T2>(remoteMsg, resultDataType);
            //        sub.Dispose();

            //        if (action != null)
            //            action(convert.Result);
            //    });
            //}
            //else
            //{
            //    sub.Dispose();
            //}

            // //使用连接池，下面的方式有问题
            //if (this.Connect())
            //{
            //    this.ServiceSubscriber.SendMessage(request.ServiceUrl, typeof(T), (remoteMsg) =>
            //    {
            //        MessageConverter<T2> convert = new MessageConverter<T2>(remoteMsg, resultDataType);
            //        this.Close();

            //        if (action != null)
            //            action(convert.Result);
            //    });
            //}
            //else
            //{
            //    this.Close();
            //}

            //新的使用连接池的方式
            Connection conn = new Connection(this.ServiceBaseUri, this.UseConnectionPool);
            conn.ErrorMessage += new EventHandler<MessageEventArgs>(RaiseSubscriberError);
            if (conn.Open())
            {
                conn.RequestService(request.ServiceUrl, typeof(T), (remoteMsg) =>
                {
                    //string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
                    //if (errMsg != string.Empty)
                    //{
                    //    RaiseSubscriberError(this, new MessageEventArgs(errMsg));
                    //}
                    //else
                    //{
                    //    MessageConverter<T2> convert = new MessageConverter<T2>(remoteMsg, resultDataType);
                    //    conn.Close();

                    //    if (action != null)
                    //        action(convert.Result);
                    //}
                    // 上面代码已经优化
                    ProcessRemoteMessage<T2>(remoteMsg, resultDataType, conn, action);
                });
            }
            else
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 请求远程服务并异步执行自定义的操作
        /// </summary>
        /// <typeparam name="T">父类对象</typeparam>
        /// <typeparam name="T2">结果要转换的兼容类对象</typeparam>
        /// <param name="sub">正在使用的订阅对象</param>
        /// <param name="request"></param>
        /// <param name="resultDataType"></param>
        /// <param name="action"></param>
        public void RequestService<T, T2>(Subscriber sub, ServiceRequest request, DataType resultDataType, Action<T2> action)
        {
            if (sub == null || sub.Closed)
            {
                RaiseSubscriberError(this, new MessageEventArgs("未打开连接或者连接已经关闭，请先调用Connect 方法"));
            }
            else
            {
                sub.SendMessage(request.ServiceUrl, typeof(T), (remoteMsg) =>
                {
                    //MessageConverter<T2> convert = new MessageConverter<T2>(remoteMsg, resultDataType);
                    //if (action != null)
                    //    action(convert.Result);

                    // 上面代码已经优化
                    ProcessRemoteMessage<T2>(remoteMsg, resultDataType, null, action);

                });
            }
        }

       

        /// <summary>
        /// 以当前连接（如未打开则打开），异步请求并处理数据
        /// </summary>
        /// <typeparam name="T">请求的数据类型</typeparam>
        /// <param name="request">服务请求对象</param>
        /// <param name="resultDataType">数据类型</param>
        /// <param name="action">要异步执行的委托方法</param>
        public void RequestServiceCurrentSubscribe<T>(ServiceRequest request, DataType resultDataType, Action<T> action)
        {
            //if (sub == null || sub.Closed)
            //    throw new InvalidOperationException("未打开连接或者连接已经关闭，请先调用Connect 方法");
            CheckConnect();
            ServiceSubscriber.SendMessage(request.ServiceUrl, null, (remoteMsg) =>
            {
                //MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                //if (action != null)
                //    action(convert.Result);
                // 上面代码已经优化
                ProcessRemoteMessage<T>(remoteMsg, resultDataType, null, action);
            });
        }

        #endregion

        #region 发布-订阅 方法
        /// <summary>
        /// 发起消息订阅
        /// </summary>
        ///<param name="reqSrvUrl">服务的地址，必须是Publish模式</param>
        ///<param name="resultType">服务返回的对象类型</param>
        ///<param name="action">自定义的消息处理方法</param>
        ///<returns>消息标识</returns>
        private int Subscribe(string reqSrvUrl, Type resultType, Action<string> action)
        {
            if (this.Connect())
            {
                //subServiceList.Add(new SubscribeArgs() { ReqSrvUrl = reqSrvUrl, ResultType = resultType, CallBackAction = action });
                return ServiceSubscriber.SendMessage(reqSrvUrl, resultType, action);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 发起消息订阅
        /// </summary>
        /// <param name="request">服务请求对象</param>
        /// <param name="action">自定义的消息处理方法</param>
        /// <returns>消息标识,如果为0，则订阅未成功</returns>
        public int Subscribe(ServiceRequest request, Action<string> action)
        {
            request.RequestModel = RequestModel.Publish;
            return Subscribe(request.ServiceUrl, request.ResultType, action);
        }

        /// <summary>
        /// 发起订阅，并处理来自服务器的消息（订阅的数据类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">服务请求对象</param>
        /// <param name="resultDataType">结果的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        /// <returns>消息标识,如果为0，则订阅未成功</returns>
        public int Subscribe<T>(ServiceRequest request, DataType resultDataType, Action<MessageConverter<T>> action)
        {
            request.ResultType = typeof(T);
            return Subscribe(request, remoteMsg =>
            {
                string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
                if (errMsg != string.Empty)
                {
                    RaiseSubscriberError(this, new MessageEventArgs(errMsg));
                }
                else
                {
                    MessageConverter<T> convert = new MessageConverter<T>(remoteMsg, resultDataType);
                    if (action != null)
                        action(convert);
                }
            });
        }

        /// <summary>
        /// 发起订阅，并处理来自服务器的消息（订阅的数据类型）
        /// </summary>
        /// <typeparam name="T">服务返回的结果对象类型，仅供服务器端使用</typeparam>
        /// <typeparam name="T2">服务结果真正要转换成最终使用的对象类型</typeparam>
        /// <param name="request">服务请求对象</param>
        /// <param name="resultDataType">消息的数据类型</param>
        /// <param name="action">自定义的处理方法</param>
        /// <returns>消息标识,如果为0，则订阅未成功</returns>
        public int Subscribe<T, T2>(ServiceRequest request, DataType resultDataType, Action<MessageConverter<T2>> action)
        {
            request.ResultType = typeof(T);
            return Subscribe(request, remoteMsg =>
            {
                string errMsg = ServiceConst.GetServiceErrorMessage(remoteMsg);
                if (errMsg != string.Empty)
                {
                    RaiseSubscriberError(this, new MessageEventArgs(errMsg));
                }
                else
                {
                    MessageConverter<T2> convert = new MessageConverter<T2>(remoteMsg, resultDataType);
                    if (action != null)
                        action(convert);
                }
            });
        }

        /// <summary>
        /// 订阅文本消息
        /// </summary>
        /// <param name="message">给服务器的初始消息</param>
        /// <param name="action">对服务器发布的消息的自定义处理</param>
        /// <returns>订阅是否成功，大于0，成功，其它为消息标识，表示成功</returns>
        public int SubscribeTextMessage(string message, Action<string> action)
        {
            return Subscribe("hello:" + message, typeof(string), serverMsg =>
            {

                if (action != null)
                    action(serverMsg);
            });
        }

        /// <summary>
        /// 在发布-订阅模式下，向发布端发送文本消息
        /// </summary>
        /// <param name="message"></param>
        public void SendTextMessage(string message)
        {
            //if (sub == null || sub.Closed)
            //    throw new InvalidOperationException("未打开连接或者连接已经关闭，请先调用Connect 方法");
            CheckConnect();
            ServiceSubscriber.SendTextMessage(message);
        }

        /// <summary>
        /// 发送命令消息
        /// </summary>
        /// <param name="cmdString">命令字符串</param>
        public void SendCommandMessage(string cmdString)
        {
            if (string.IsNullOrEmpty(cmdString))
                return;
            string strTemp = "[CMD]" + cmdString;
            SendTextMessage(strTemp);
        }

        /// <summary>
        /// 发送命令信息，并订阅命令的执行结果
        /// </summary>
        /// <param name="cmdString">命令字符串</param>
        /// <param name="action">要处理的回调方法</param>
        /// <returns>消息编号</returns>
        public int SubscribeCommandMessage(string cmdString, Action<string> action)
        {
            if (string.IsNullOrEmpty(cmdString))
                return 0;
            string strTemp = "[CMD]" + cmdString;

            return Subscribe(strTemp, null, serverMsg =>
            {
                if (action != null)
                    action(serverMsg);
            });
        }

        /// <summary>
        /// 根据消息编号，取消当前连接订阅的服务方法，但不会关闭当前连接
        /// </summary>
        /// <param name="messageId"></param>
        public void DeSubscribe(int messageId)
        {
            //CheckConnect();
            if (ServiceSubscriber == null || ServiceSubscriber.Closed)
            {
                return;
            }
            else
            {
                if (messageId < 1)
                {
                    RaiseSubscriberError(this, new MessageEventArgs("消息编号不能小于1,Number:" + messageId));
                }
                else
                {
                    ServiceSubscriber.SendTextMessage(messageId, "[CMD]DeSubscribeService");
                    ServiceSubscriber.RemoveMessage(messageId);
                }
            }

        }

        #endregion

        #region 打开和关闭服务连接
        /// <summary>
        /// 是否使用连接池，在订阅模式下，不必设置该属性
        /// </summary>
        public bool UseConnectionPool { get; set; }

        /// <summary>
        /// 连接服务
        /// </summary>
        /// <param name="serviceUri">服务的基础地址</param>
        /// <returns></returns>
        public bool Connect(string serviceUri)
        {
            //if (ServiceSubscriber == null || ServiceSubscriber.Closed)
            //{
            //    ServiceSubscriber = UseConnectionPool? 
            //        PublishServicePool.Instance.GetServiceChannel(serviceUri): 
            //        new Subscriber(serviceUri);

            //    ServiceSubscriber.ErrorMessage += new EventHandler<MessageEventArgs>(ServiceSubscriber_ErrorMessage);
            //    if( ServiceSubscriber.Closed)
            //        ServiceSubscriber.Subscribe();//尝试打开
            //}
            //return !ServiceSubscriber.Closed;

            //新代码
            if (ServiceSubscriber == null || ServiceSubscriber.Closed)
            {
                Connection conn = new Connection(serviceUri, this.UseConnectionPool);
                conn.ErrorMessage += new EventHandler<MessageEventArgs>(RaiseSubscriberError);
                if (conn.Open())
                {
                    ServiceSubscriber = conn.ServiceSubscriber;
                    this.ServiceBaseUri = serviceUri;
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            return !ServiceSubscriber.Closed;
        }

        /// <summary>
        /// 连接服务
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            return Connect(this.ServiceBaseUri);
        }

        /// <summary>
        /// 取消订阅，关闭连接
        /// </summary>
        public void Close()
        {
            if (ServiceSubscriber != null)
            {
                if (UseConnectionPool)
                {
                    //将连接放回连接池
                    PublishServicePool.Instance.BackPool(ServiceSubscriber);
                }
                else
                {
                    if (!ServiceSubscriber.Closed)
                        ServiceSubscriber.Close(1);
                }
            }
        }

        #endregion

        #region 其它公开的方法

        /// <summary>
        /// (在打开连接后)开启心跳监测，如果发现服务器与客户端网络异常，会触发异常事件
        /// </summary>
        public void StartCheckHeartBeat()
        {
            if (ServiceSubscriber != null && !ServiceSubscriber.Closed)
            {
                ServiceSubscriber.HeartBeatError += new EventHandler<MessageEventArgs>(ServiceSubscriber_HeartBeatError);
                heartBeatTimer = new Timer(ServiceSubscriber.CheckHeartBeatCallBack(), null, 1000, 10000);
            }
        }

        /// <summary>
        /// 设置服务访问代理的基地址（ServiceBaseUri属性）
        /// </summary>
        /// <param name="host">远程主机名，可以是一个IP地址</param>
        /// <param name="port">连接远程主机的端口号</param>
        public void CreateServiceBaseUri(string host, string port)
        {
            ServiceBaseUri = string.Format("net.tcp://{0}:{1}", host,port);
        }
        #endregion

        public void Dispose()
        {
            this.Close();
        }
    }

    public class RequestServiceArgs<T>
    {
        public string ReqSrvUrl { get; set; }
        public DataType ResultDataType { get; set; }
        public Action<T> ActionT { get; set; }
    }
}
