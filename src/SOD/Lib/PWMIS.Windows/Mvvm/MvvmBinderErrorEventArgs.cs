using System;

namespace PWMIS.Windows.Mvvm
{
    public class MvvmBinderErrorEventArgs : EventArgs
    {
        public MvvmBinderErrorEventArgs(string errMessage)
        {
            ErrorMessage = errMessage;
        }

        public string ErrorMessage { get; private set; }
    }
}