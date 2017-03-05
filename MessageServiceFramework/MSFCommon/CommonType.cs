using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.EnterpriseFramework.Common
{
    /// <summary>
    /// 执行自定义方法的委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="arg"></param>
    /// <returns></returns>
    public delegate TResult MyFunc<T, TResult>(
    T arg
    );

    public delegate TResult MyFunc<TResult>();


    /// <summary>
    /// 服务端消息的数据类型
    /// </summary>
    public enum DataType
    {
        Text,
        Html,
        Xml,
        Json,
        Binary,
        DateTime
    }
}

