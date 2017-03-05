using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessagePublishService;

namespace MessagePublisher
{
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
