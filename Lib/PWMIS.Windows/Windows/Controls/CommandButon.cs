using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace PWMIS.Windows.Windows.Controls
{
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

        public string CommandName
        {
            get;
            set;
        }

        public string ParameterObject
        {
            get;
            set;
        }

        public string ParameterProperty
        {
            get;
            set;
        }

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
      
    }
}
