using System;

namespace PWMIS.Common
{
    /// <summary>
    ///     参数结构
    /// </summary>
    /// <remarks></remarks>
    public struct ParamMapType
    {
        public string ParamName;
        public TypeCode TypeCode;
    }

    public struct Point
    {
        public Point(int a, int b)
        {
            A = a;
            B = b;
        }

        public int A;
        public int B;
    }
}