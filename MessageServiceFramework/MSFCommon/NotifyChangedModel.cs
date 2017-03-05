using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PWMIS.EnterpriseFramework.Common
{
    public class NotifyChangedModel : INotifyPropertyChanged
    {
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
