using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace PWMIS.EnterpriseFramework.AOP
{
    public class LogAttribute : BeforeDecoratorAttribute
    {
        public override void Process(object target, MethodBase method, object[] parameters)
        {
           // Trace.WriteLine(method.Name);
        }
    }

    public class ParameterCountAttribute : BeforeDecoratorAttribute
    {
        public override void Process(object target, MethodBase method, object[] parameters)
        {
           // Trace.WriteLine(target.GetType().Name);
        }
    } 
}
