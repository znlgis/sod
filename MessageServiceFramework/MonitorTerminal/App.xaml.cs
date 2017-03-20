using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MonitorTerminal
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            App.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            Login l = new Login();
            if (l.ShowDialog().Value)
            {
                App.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
                base.OnStartup(e);
            }
            else
            {
                App.Current.Shutdown();
            }
        }
    }
}
