using PWMIS.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Windows.Mvvm
{
    /// <summary>
    /// WindowsForms MVVM 窗体接口
    /// </summary>
    public interface IMvvmForm
    {
        /// <summary>
        /// 用于解决【线程间操作无效】的问题
        /// </summary>
        /// <param name="action">自定义的方法，在此方法内对控件或者绑定的数据对象进行修改</param>
        void FormInvoke(MyAction action);

        /// <summary>
        /// 用于解决【线程间操作无效】的问题
        /// </summary>
        /// <typeparam name="T">控件或者其它对象类型</typeparam>
        /// <param name="ctl">对象实例</param>
        /// <param name="action">要对控件或者对象执行的方法，在此方法内对控件或者绑定的数据对象进行修改</param>
        void FormInvoke<T>(T ctl, Action<T> action);
    }
}
