using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows.Mvvm
{
    /// <summary>
    /// MVVM 命令处理接口
    /// </summary>
    public interface IMvvmCommand
    {
        /// <summary>
        /// 执行前的处理，如果返回False，则不会真正执行命令方法
        /// </summary>
        /// <param name="para">命令参数</param>
        /// <returns></returns>
        bool BeforExecute(object para);
        /// <summary>
        /// 执行命令方法
        /// </summary>
        /// <param name="para">命令参数</param>
        void Execute(object para);
        /// <summary>
        /// 命令执行后的操作
        /// </summary>
        void AfterExecute();
    }
}
