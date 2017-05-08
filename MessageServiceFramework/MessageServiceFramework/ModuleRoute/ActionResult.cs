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
    
    }

    /// <summary>
    /// 将结果作为对象返回，然后转换结果再使用，例如作为动态对象使用
    /// </summary>
    public class ActionResult : IActionResult
    {
        public object ObjectResult { get; set; }
    }

    /// <summary>
    /// 将结果作为强类型返回，并直接使用它。注意，Error属性为空才表示调用成功
    /// </summary>
    /// <typeparam name="T">跨模块调用的方法结果类型</typeparam>
    public class ActionResult<T> : IActionResult
    {
        public T Result { get; set; }

        public ErrorAction Error { get; protected internal set; }
    }
}
