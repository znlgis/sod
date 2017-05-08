using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 没有返回值调用的结果
    /// </summary>
    public class EmptyAction : ActionResult
    {
        public EmptyAction()
        {
            base.Succeed = true;
        }
    }
}
