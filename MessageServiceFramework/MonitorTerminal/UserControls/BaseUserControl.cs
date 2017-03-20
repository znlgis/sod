using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using MonitorTerminal.CustomEntities;

namespace MonitorTerminal.UserControls
{
    public abstract class BaseUserControl : UserControl
    {
        public ServiceItem service { get; set; }
        public virtual void LoadData(ServiceItem servicenode)
        {
            service = servicenode;
        }

        public abstract void CloseServiceProxy();
    }
}
