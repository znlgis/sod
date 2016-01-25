using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Common
{
    /// <summary>
    /// 参数结构
    /// </summary>
    /// <remarks></remarks>
    public struct ParamMapType
    {
        public string ParamName;
        public System.TypeCode TypeCode;
    }

    public struct Point
    {
        public Point(int a, int b)
        {
            this.A = a;
            this.B = b;
        }
        public int A;
        public int B;
    }
}
