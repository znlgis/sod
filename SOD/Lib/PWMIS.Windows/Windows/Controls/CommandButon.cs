using PWMIS.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace PWMIS.Windows.Controls
{
    /// <summary>
    /// 命令按钮控件
    /// </summary>
    public partial class CommandButon :System.Windows.Forms.Button,ICommandControl
    {
        public CommandButon()
        {
            InitializeComponent();
            this.ControlEvent = "Click";
        }

        public CommandButon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.ControlEvent = "Click";
        }

        [Category("MVVM"), Description("命令接口对象名称")]
        public string CommandName
        {
            get;
            set;
        }

         [Category("MVVM"), Description("关联的参数对象")]
        public string ParameterObject
        {
            get;
            set;
        }

         [Category("MVVM"), Description("关联的参数对象的参数名")]
        public string ParameterProperty
        {
            get;
            set;
        }

         [Category("MVVM"), Description("命令关联的控件事件名称")]
        public string ControlEvent
        {
            get;
            set;
        }

        /*
        private static readonly object Event_RaiseCommand = new object();
        /// <summary>
        /// 发起命令事件
        /// </summary>
        public event EventHandler RaiseCommand
        {
            add 
            {
                base.Events.AddHandler(Event_RaiseCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(Event_RaiseCommand, value);
            }
        }
         */

         [Category("MVVM"), Description("命令接口对象所在的对象名称")]
        public string CommandObject
        {
            get;
            set;
        }

        //[Browsable(false)]
        //public IMvvmCommand Command
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
