using System.ComponentModel;
using System.Windows.Forms;

namespace PWMIS.Windows.Controls
{
    /// <summary>
    ///     命令按钮控件
    /// </summary>
    public partial class CommandButon : Button, ICommandControl
    {
        public CommandButon()
        {
            InitializeComponent();
            ControlEvent = "Click";
        }

        public CommandButon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            ControlEvent = "Click";
        }

        [Category("MVVM")]
        [Description("命令接口对象名称")]
        public string CommandName { get; set; }

        [Category("MVVM")]
        [Description("当前窗体上提供给命令按钮执行命令的参数关联的参数对象")]
        public string ParameterObject { get; set; }

        [Category("MVVM")]
        [Description("关联的参数对象的属性名，执行命令方法的时候将使用它的值作为参数值")]
        public string ParameterProperty { get; set; }

        [Category("MVVM")]
        [Description("命令关联的控件事件名称")]
        public string ControlEvent { get; set; }

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

        [Category("MVVM")]
        [Description("命令接口对象所在的对象名称")]
        public string CommandObject { get; set; }

        //[Browsable(false)]
        //public IMvvmCommand Command
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}