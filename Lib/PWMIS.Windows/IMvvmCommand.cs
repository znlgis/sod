using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows
{
    /// <summary>
    /// MVVM 命令处理接口
    /// </summary>
    public interface IMvvmCommand
    {
        bool BeforExecute();
        void Execute();
        void AfterExecute();
    }
}
