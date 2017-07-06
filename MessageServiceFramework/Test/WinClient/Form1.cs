using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PWMIS.EnterpriseFramework.Service.Client;
using System.Diagnostics;
using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
using Model;
using MessageSubscriber;

namespace WinClient
{
    /// <summary>
    /// 方法处理委托
    /// </summary>
    public delegate void MyAction();

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 自定义控件异步调用方法
        /// </summary>
        /// <param name="ctr">控件</param>
        /// <param name="action">自定义的处理方法</param>
        public void MyInvoke(Control ctr, MyAction action)
        {
            ctr.Invoke(action);
        }

        /// <summary>
        /// 带一个参数的自定义控件异步调用方法
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="ctr">控件</param>
        /// <param name="action">自定义的处理方法</param>
        /// <param name="para">参数</param>
        public void MyInvoke<T>(Control ctr, Action<T> action, T para)
        {
            ctr.Invoke(action, para);
        }

        /// <summary>
        /// 自定义窗体异步调用方法
        /// </summary>
        /// <param name="ctr">控件</param>
        /// <param name="action">自定义的处理方法</param>
        public void MyInvoke(Form form, MyAction action)
        {
            form.Invoke(action);
        }


        /// <summary>
        /// 带一个参数的自定义窗体异步调用方法
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="ctr">控件</param>
        /// <param name="action">自定义的处理方法</param>
        /// <param name="para">参数</param>
        public void MyInvoke<T>(Form form, Action<T> action, T para)
        {
            form.Invoke(action, para);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CalcClient cc = new CalcClient(this.txtSerivceUri.Text); //"127.0.0.1", 8888
            double result = cc.GetAddResult(3, 2);

            //要请求的服务消息
            string requestMessage =string.Format( "Service://Calculator/Add/System.Int32={0}&System.Int32={1}",this.txtA.Text,this.txtB.Text);
            //测试会话，结果将在100的基础上累加
            Proxy sessionProxy = new Proxy();
            sessionProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(sessionProxy_ErrorMessage);
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            sw.Start();
            //同步方式必须显示的打开和关闭连接
            if (sessionProxy.Connect(this.txtSerivceUri.Text))
            {
                int x = sessionProxy.GetServiceMessage<int>(requestMessage, DataType.Text).Result;
                sw.Stop();
                this.lblResult.Text = "同步会话，第一次计算，结果：" + x.ToString()+"；耗时(ms)："+sw.ElapsedMilliseconds;
                MessageBox.Show(this.lblResult.Text);

                sw.Restart();
                int y = sessionProxy.GetServiceMessage<int>(requestMessage).Result;
                this.lblResult.Text = "同步会话，第二次计算，结果：" + y.ToString() + "；耗时(ms)：" + sw.ElapsedMilliseconds;
                MessageBox.Show(this.lblResult.Text);

                sessionProxy.Close();
            }
        }

        void sessionProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            MessageBox.Show("服务异常："+e.MessageText);
        }

        private async void btnSub_Click(object sender, EventArgs e)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "Calculator";
            request.MethodName = "Sub";
            request.Parameters = new object[] { int.Parse(this.txtA.Text), int.Parse(this.txtB.Text) };
            //异步方式测试
            Proxy serviceProxy = new Proxy();
            serviceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(sessionProxy_ErrorMessage);
            serviceProxy.ServiceBaseUri = this.txtSerivceUri.Text;
            //serviceProxy.RequestService<int>(request, DataType.Text, (x) =>
            //{
            //    MyInvoke(this.lblResult, () =>
            //    {
            //        this.lblResult.Text = "Result=" + x.ToString();
            //        MessageBox.Show("[异步]计算完成");
            //    });
            //});
            //改用异步方法，达到相同效果
           //var task= serviceProxy.RequestServiceAsync<int>(request);
           //this.lblResult.Text = "Result=" + task.Result;

            try
            {
                //int result = await serviceProxy.RequestServiceAsync<int>(request);
                int result = serviceProxy.RequestServiceAsync<int>(request).Result;
                this.lblResult.Text = "Result=" + result;
            }
            catch (AggregateException ex1)
            {
                //MessageBox.Show(ex1.Message);
                foreach (var item in ex1.InnerExceptions)
                {
                    string errMsg= string.Format ("异常类型：{0}{1}来自：{2}{3}异常内容：{4}", item.GetType(), Environment.NewLine,
         item.Source, Environment.NewLine, item.Message);
                    MessageBox.Show(errMsg);
                }  

            }
            catch (Exception ex)
            {
                MessageBox.Show("异步调用服务异常：" + ex.Message);
            }
         
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "Calculator";
            request.MethodName = "TestVoidMethod";
            request.Parameters = new object[] {"Message Service Framework" };
            //异步方式测试
            Proxy serviceProxy = new Proxy();
            serviceProxy.ServiceBaseUri = this.txtSerivceUri.Text;
            serviceProxy.RequestService<int>(request, (x) =>
            {
                MessageBox.Show("[异步]执行无返回值方法完成");
            });
        }

        private void btnServerTime_Click(object sender, EventArgs e)
        {
            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "TestTimeService";
            request.MethodName = "ServerTime";

            //异步方式测试
            Proxy serviceProxy = new Proxy();
            serviceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(serviceProxy_ErrorMessage);
            serviceProxy.ServiceBaseUri = this.txtSerivceUri.Text;
            int msgId= serviceProxy.Subscribe<TimeCount>(request, DataType.Json, (converter) =>
            {
                if (converter.Succeed)
                {
                    MyInvoke(this, () =>
                    {
                        this.lblResult.Text = converter.Result.Now.ToString();
                        this.txtA.Text = converter.Result.Count.ToString();
                        System.Diagnostics.Debug.WriteLine("time count:{0}",converter.Result.Count );
                        if (converter.Result.Count > 100)
                        {
                            serviceProxy.Close();
                            this.btnServerTime.Enabled = true;
                        }
                    });
                }
                else
                {
                    MessageBox.Show(converter.ErrorMessage);
                }
            });
            if (msgId < 1)
            {
                MessageBox.Show("订阅失败");
            }
            else
            {
                this.btnServerTime.Enabled = false;
            }
        }

        void serviceProxy_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            MessageBox.Show(e.MessageText);
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            //将相关的类库复制到服务器的运行目录
            string source = @"..\..\..\Service\bin\Debug\ServiceSample.dll";
            string target = @"..\..\..\ServiceHost\ServiceSample.dll";
            System.IO.File.Copy(source, target,true);
            source = @"..\..\..\Service\bin\Debug\ServiceSample.pdb";
            target = @"..\..\..\ServiceHost\ServiceSample.pdb";
            System.IO.File.Copy(source, target, true);

            string source1 = @"..\..\..\Model\bin\Debug\Model.dll";
            string target1 = @"..\..\..\ServiceHost\Model.dll";
            System.IO.File.Copy(source1, target1, true);
            source1 = @"..\..\..\Model\bin\Debug\Model.pdb";
            target1 = @"..\..\..\ServiceHost\Model.pdb";
            System.IO.File.Copy(source1, target1, true);

            Process.Start(@"..\..\..\ServiceHost\PdfNetEF.MessageServiceHost.exe");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //System.Data.SqlClient.SqlParameter para = new System.Data.SqlClient.SqlParameter("p1", "abc");
            //para.Size = 100;
            //MessageBox.Show( para.SqlDbType.ToString());
            Environment.SetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT", "yes");
          
        }

        private void btnAlarmClock_Click(object sender, EventArgs e)
        {
            //if (this.dateTimePicker1.Value < DateTime.Now)
            //{
            //    MessageBox.Show("请选择闹铃时间");
            //    return;
            //}

            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "AlarmClockService";
            request.MethodName = "SetAlarmTime";
            request.Parameters = new object[] { this.dateTimePicker1.Value };

            //异步方式测试
            Proxy serviceProxy = new Proxy();
            serviceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(serviceProxy_ErrorMessage);
            serviceProxy.ServiceBaseUri = this.txtSerivceUri.Text;
            int msgId = serviceProxy.Subscribe<DateTime>(request, DataType.Json, (converter) =>
            {
                if (converter.Succeed)
                {
                    MyInvoke(this, () =>
                    {
                        this.lblResult.Text = converter.Result.ToString();// +"/" + DateTime.Now.ToLongTimeString();
                        //this.txtA.Text = converter.Result.Count.ToString();
                        //if (converter.Result.Count > 100)
                        //{
                        //    serviceProxy.Close();
                        //    this.btnServerTime.Enabled = true;
                        //}
                    });
                }
                else
                {
                    MessageBox.Show(converter.ErrorMessage);
                }
            });
            if (msgId < 1)
            {
                MessageBox.Show("订阅失败");
            }
            else
            {
                this.btnServerTime.Enabled = false;
            }
        }

        private void btnServerText_Click(object sender, EventArgs e)
        {
            Proxy serviceProxy = new Proxy();
            serviceProxy.ErrorMessage += new EventHandler<MessageSubscriber.MessageEventArgs>(serviceProxy_ErrorMessage);
            serviceProxy.ServiceBaseUri = this.txtSerivceUri.Text;
            serviceProxy.SubscribeTextMessage("你好，MSF", serverText => {
                MyInvoke(this, () =>
                {
                    this.lblResult.Text = serverText;
                   
                });
            });
        }
    }
}
