using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows
{
    /// <summary>
    /// 控件验证的类型
    /// </summary>
    public enum ValidationDataType
    {
        /// <summary>
        /// 字符串数据类型。该值被视为 System.String。
        /// </summary>
        String = 0,
       /// <summary>
        /// 32 位有符号整数数据类型。该值被视为 System.Int32。
       /// </summary>
          Integer = 1,
       /// <summary>
          ///  双精度浮点数数据类型。该值被视为 System.Double。
       /// </summary>
        Double = 2,

        /// <summary>
        /// 日期数据类型。仅允许使用数字日期。不能指定时间部分。
        /// </summary>
        Date = 3,
       /// <summary>
        /// 货币数据类型。该值被视为 System.Decimal。但仍允许使用货币和分组符号。
       /// </summary>
        Currency = 4,
    }
}
