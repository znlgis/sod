using PWMIS.Common;
using PWMIS.Core;
using PWMIS.DataForms.Adapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PWMIS.Windows.Mvvm
{
    /// <summary>
    /// MVVM 窗体基类，如果需要在SOD Windows数据窗体上实现MVVM效果，请继承本类。
    /// </summary>
    public partial class MvvmForm : Form
    {
        public delegate void CommandMethod();
        public delegate void CommandMethod<T>(T para);

        //private Dictionary<object, Delegate> dictCommand; //CommandMethod
        /// <summary>
        /// MVVM窗体绑定处理过程发生异常的事件
        /// </summary>
        public event EventHandler MvvmBinderErrorEvent;

        private void RaiseBinderError(object sender, Exception ex)
        {
            if (MvvmBinderErrorEvent != null)
                MvvmBinderErrorEvent(sender, new MvvmBinderErrorEventArgs(ex.Message ));
            else
                throw new NotSupportedException("MVVM Binder Exception,because not handler the MvvmBinderErrorEvent.see inner exception.", ex);
        }
       

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MvvmForm()
        {
            InitializeComponent();
            //dictCommand = new Dictionary<object, Delegate>();
        }

        /// <summary>
        /// 绑定未处理的数据控件，如果有控件未处理这里将抛出异常
        /// </summary>
        /// <param name="control">数据控件</param>
        /// <param name="dataSource">数据源对象</param>
        /// <param name="dataMember">要绑定的成员名称</param>
        protected virtual void BindDataControl(IDataControl control, object dataSource,string dataMember)
        {
            throw new NotImplementedException("请重写此方法处理当前控件。注意不要再调用此基类方法。");
        }

        /// <summary>
        /// 对数据控件实现双向绑定
        /// </summary>
        /// <param name="controls">要搜索数据控件的窗体控件集合</param>
        public void BindDataControls(Control.ControlCollection controls)
        {
            var dataControls = MyWinForm.GetIBControls(controls);
            foreach (IDataControl control in dataControls)
            {
                //control.LinkObject 这里都是 "DataContext"
                object dataSource = GetInstanceByMemberName(control.LinkObject);
                if (control is TextBox)
                {
                    ((TextBox)control).DataBindings.Add("Text", dataSource, control.LinkProperty);
                }else  if (control is Label)
                {
                    ((Label)control).DataBindings.Add("Text", dataSource, control.LinkProperty);
                }else  if (control is ListBox)
                {
                    ((ListBox)control).DataBindings.Add("SelectedValue", dataSource, control.LinkProperty, false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).DataBindings.Add("Value", dataSource, control.LinkProperty, false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else
                {
                    //自定义处理控件类型
                    BindDataControl(control, dataSource, control.LinkProperty);
                }
            }
        }
       
        /// <summary>
        /// 绑定ViewModel的命令到窗体的按钮等控件上
        /// </summary>
        /// <param name="control">ButtonBase 按钮等执行命令调用的控件</param>
        /// <param name="command">要执行的命令委托方法</param>
        public void BindCommandControls(Control control,CommandMethod command)
        {
            if (control is ButtonBase)
            {
                //dictCommand.Add(control, command);
                ((ButtonBase)control).Click += (sender, e) =>
                {
                    //((CommandMethod)dictCommand[sender])(); 
                    CommandEventMethod(sender, e, command);
                };
            }
        }

        /// <summary>
        /// 绑定ViewModel的命令到窗体的任意控件上
        /// </summary>
        /// <param name="control">窗体控件</param>
        /// <param name="controlEvent">控件事件名称</param>
        /// <param name="command">命令方法</param>
        public void BindCommandControls(Control control,string controlEvent, CommandMethod command)
        {
            EventHandler hander = new EventHandler(
                   (object sender, EventArgs e) =>
                   {
                       CommandEventMethod(sender, e, command);
                   });

            Type ctrType = control.GetType();
            ctrType.GetEvent(controlEvent).AddEventHandler(control, hander);
        }

        /// <summary>
        /// (自动)绑定命令按钮的ControlEvent 到关联的命令对象
        /// </summary>
        /// <param name="control"></param>
        public void BindCommandControls(ICommandControl control)
        {
            object dataSource = GetInstanceByMemberName(control.CommandObject);
            string[] propNames = control.CommandName.Split('.');
            object obj = GetPropertyValue(dataSource, propNames);
            IMvvmCommand command = obj as IMvvmCommand;

            BindCommandControls(control ,control.ControlEvent, command);
        }

        /// <summary>
        /// 绑定控件的事件到命令接口对象
        /// </summary>
        /// <param name="control">窗体控件</param>
        /// <param name="controlEvent">控件的事件</param>
        /// <param name="command">要绑定的命令接口对象</param>
        public void BindCommandControls(object control, string controlEvent, IMvvmCommand command)
        {
            EventHandler hander = new EventHandler(
                    (object sender, EventArgs e) =>
                    {
                        object paraValue = null;
                        if (control is ICommandControl)
                        {
                            try
                            {
                                ICommandControl cmdCtr = control as ICommandControl;
                                object paraSource = GetInstanceByMemberName(cmdCtr.ParameterObject);
                                string[] paraPropNames = cmdCtr.ParameterProperty.Split('.');
                                paraValue = GetPropertyValue(paraSource, paraPropNames);
                            }
                            catch (Exception ex)
                            {
                                RaiseBinderError(control, ex);
                                return;
                            }
                           
                        }

                        if (command.BeforExecute(paraValue))
                        {
                            try
                            {
                                command.Execute(paraValue);
                            }
                            catch (Exception ex)
                            {
                                RaiseBinderError(control, ex);
                            }
                            finally
                            {
                                command.AfterExecute();
                            }
                        }
                    });

            Type ctrType = control.GetType();
            ctrType.GetEvent(controlEvent).AddEventHandler(control, hander);
        }

        /// <summary>
        /// 绑定一个命令控件到一个有参数的命令方法上
        /// </summary>
        /// <typeparam name="T">命令方法的参数类型</typeparam>
        /// <param name="control">命令控件</param>
        /// <param name="command">带参数的命令方法</param>
        public void BindCommandControls<T>(ICommandControl control, CommandMethod<T> command)
        {
            //dictCommand.Add(control, command);
            EventHandler hander = new EventHandler(
                (object sender, EventArgs e) => {
                    CommandEventMethod<T>(sender, e,command);
                });

            Type ctrType = control.GetType();
            ctrType.GetEvent(control.ControlEvent).AddEventHandler(control, hander);
           
        }

        private void CommandEventMethod(object sender, EventArgs e, CommandMethod command)
        {
            try
            {
                command();
            }
            catch (Exception ex)
            {
                RaiseBinderError(sender, ex);
            }
        }
        private void CommandEventMethod<T>(object sender, EventArgs e, CommandMethod<T> command)
        {
            ICommandControl cmdCtr = sender as ICommandControl;
            try
            {
                //这里不处理命令控件关联的命令对象
                object dataSource = GetInstanceByMemberName(cmdCtr.ParameterObject);
                T paraValue = GetCommandParameterValue<T>(dataSource, cmdCtr.ParameterProperty);
                //CommandMethod<T> method = (CommandMethod<T>)dictCommand[sender];
                //method(paraValue);
                command(paraValue);
            }
            catch (Exception ex)
            {
                RaiseBinderError(sender, ex);
            }
           
        }


        /// <summary>
        /// 根据成员名称获取成员的实例
        /// </summary>
        /// <param name="memberName">MVVM窗体上的数据成员属性名</param>
        /// <returns></returns>
        protected virtual object GetInstanceByMemberName(string memberName)
        {
           var obj= this.GetType().InvokeMember(memberName, 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.GetField |  
                System.Reflection.BindingFlags.GetProperty |
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Public , 
                null, this, null);
           if (obj == null)
               throw new Exception("当前窗体未找到名称为 " + memberName + " 的实例成员，请检查数据绑定控件的 LinkObject 属性。");

           return obj;
            
        }

        /// <summary>
        /// 从源对象调用多层次属性对象，例如 user.Order.ID;
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="dataSource">源对象</param>
        /// <param name="propertyName">带层次的属性对象</param>
        /// <returns></returns>
        private T GetCommandParameterValue<T>(object dataSource, string propertyName)
        {
            string[] propNames = propertyName.Split('.');
            object obj= GetPropertyValue(dataSource, propNames);
            //处理结果转换错误
            return CommonUtil.ChangeType<T>(obj);
        }

        private object GetPropertyValue(object dataSource, string[] propNames)
        {
            Type t = dataSource.GetType();
            if (propNames.Length > 1)
            {
                FieldInfo fi = t.GetField(propNames[0], BindingFlags.Public | BindingFlags.Instance);
                if (fi != null)
                {
                    object fieldObj = fi.GetValue(dataSource);
                    string[] newPropNames = new string[propNames.Length - 1];
                    propNames.CopyTo(newPropNames, 1);
                    return GetPropertyValue(fieldObj, newPropNames);
                }
                PropertyInfo pi = t.GetProperty(propNames[0], BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    object propObj = pi.GetValue(dataSource, null);
                    string[] newPropNames = new string[propNames.Length - 1];
                    propNames.CopyTo(newPropNames, 1);
                    return GetPropertyValue(propObj, newPropNames);
                }
            }
            else
            {
                FieldInfo fi = t.GetField(propNames[0], BindingFlags.Public | BindingFlags.Instance);
                if (fi != null)
                {
                    object fieldObj = fi.GetValue(dataSource);
                    return fieldObj;
                }
                 PropertyInfo pi = t.GetProperty(propNames[0], BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    object propObj = pi.GetValue(dataSource, null);
                    return propObj;
                }
            }
            //统一抛出错误事件
            throw new Exception("在对象" + t.Name + " 中没有找到名为 " + propNames[0] + " 的字段或者属性！");
        }

        private void MvvmForm_Load(object sender, EventArgs e)
        {
            var ibControls = MyWinForm.GetIBControls(this.Controls);
            ControlCollection coll = new ControlCollection(this);
            foreach(IDataControl ctr in ibControls)
                coll.Add(ctr as Control);
            BindDataControls(coll);
        }
    }
}
