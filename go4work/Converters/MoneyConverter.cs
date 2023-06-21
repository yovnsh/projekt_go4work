using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace go4work.Converters
{
    /// <summary>
    /// zamienia liczbę całkowitą na format pieniężny
    /// </summary>
    public class MoneyConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return $"{value}.00 zł";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().Replace(".00 zł", "");
        }
    }
}
