using PWMIS.EnterpriseFramework.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AOPTestLib
{
    public interface IBizObject
    {
        /// 对对象功能的装饰采用“横切”而非传统的继承方式获得， 
        /// 中间代理对象的构造是隐式，而且是由DecoratorInjector包装的， 
        /// 从外部角度看，对象的扩展是在Meta信息部分扩展，而且有关的约 
        /// 束定义在接口而非实体类层次。 
        [Log]
        [ParameterCount]
        int GetValue();
    }

    public class BizObject : IBizObject
    {
        public int GetValue() { return 0; }
    } 
}
