using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace go4work.Converters
{
    /// <summary>
    /// zamienia pierwszą literę na wielką
    /// </summary>
    public class CapitalizeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string val = value.ToString();
            if (val.Length == 0)
            {
                return val;
            }
            else
            {
                return char.ToUpper(val[0]) + val.Substring(1);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().ToLower();
        }
    }
}
