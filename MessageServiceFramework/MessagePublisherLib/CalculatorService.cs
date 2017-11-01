using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagePublishService;

namespace MessagePublisher
{
    /// <summary>
    ///  简易计算器服务端，仅供测试WCF通信
    /// </summary>
    public class CalculatorService : ICalculator
    {

        #region ICalculator 成员

        public double Add(double x, double y)
        {
            return x + y;
        }

        #endregion
    }
}
