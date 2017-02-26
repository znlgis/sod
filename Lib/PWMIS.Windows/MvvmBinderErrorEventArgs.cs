using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows
{
    public class MvvmBinderErrorEventArgs:EventArgs
    {
        public string ErrorMessage { get; private set; }
        public MvvmBinderErrorEventArgs(string errMessage)
        {
            this.ErrorMessage = errMessage;
        }
    }
}
