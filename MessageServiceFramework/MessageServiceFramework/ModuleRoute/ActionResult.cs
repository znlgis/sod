using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 跨模块调用结果接口，只有实现了该接口的方法，才可以跨模块调用
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// 调用是否成功
        /// </summary>
        bool Succeed { get; }

        /// <summary>
        /// 方法调用的结果
        /// </summary>
        object ObjectResult { get; set; }
    }

    /// <summary>
    /// 将结果作为对象返回，然后转换结果再使用，例如作为动态对象使用
    /// </summary>
    public class ActionResult : IActionResult
    {
        /// <summary>
        /// 方法调用的结果
        /// </summary>
        public object ObjectResult { get; set; }
        /// <summary>
        /// 结果是否是成功的，如果失败，当前类型是ErrorAction
        /// </summary>
        public bool Succeed { get;protected internal set; }
    }

    /// <summary>
    /// 将结果作为强类型返回，并直接使用它。注意，Error属性为空才表示调用成功
    /// </summary>
    /// <typeparam name="T">跨模块调用的方法结果类型</typeparam>
    public class ActionResult<T> : IActionResult
    {
        /// <summary>
        /// 方法调用的结果
        /// </summary>
        public T Result { get; set; }

        public object ObjectResult {
            get {
                return (object)Result;
            }
            set {
                throw new Exception("请修改成调用 Result 属性并赋值");
            }
        }
        /// <summary>
        /// 错误方法调用对象
        /// </summary>
        public ErrorAction Error { get; protected internal set; }

        /// <summary>
        /// 结果是否是成功的，如果失败，当前类型是ErrorAction
        /// </summary>
        public bool Succeed { get; protected internal set; }
    }
}
