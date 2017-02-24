using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows
{
    /// <summary>
    /// 命令接口控件
    /// </summary>
    public interface ICommandControl
    {
        /// <summary>
        /// 要执行命令的方法名称
        /// </summary>
        string CommandName { get; set; }
        /// <summary>
        /// 关联的参数对象
        /// </summary>
        string ParameterObject{get;set;}
        /// <summary>
        /// 关联的参数对象的参数名
        /// </summary>
        string ParameterProperty { get; set; }
        /// <summary>
        /// 命令关联的控件事件名称
        /// </summary>
        string ControlEvent { get; set; }
    }
}
