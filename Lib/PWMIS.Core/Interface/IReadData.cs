using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PWMIS.Common
{
    /// <summary>
    /// 读取数据接口，以用户自定义的方式，来决定如何读取来自数据源的数据
    /// </summary>
    public interface IReadData
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="reader">数据阅读器</param>
        /// <param name="fieldCount">本次字段数量</param>
        /// <param name="fieldNames">本次字段的名称数组</param>
        void ReadData(IDataReader reader, int fieldCount, string[] fieldNames);
    }
}
