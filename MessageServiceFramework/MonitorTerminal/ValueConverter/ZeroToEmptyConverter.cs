using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MonitorTerminal.ValueConverter
{
    public class DefaultToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value.GetType();
            if (type == typeof(long))
            {
                if ((long)value == 0) return string.Empty;
                else return value;
            }
            else if(type == typeof(DateTime))
            {
                if ((DateTime)value == DateTime.MinValue) return string.Empty;
                else return value;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
