using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace go4work.Converters
{
    /// <summary>
    /// zwraca tylko krótką datę z całej daty
    /// </summary>
    public class ShortDateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime) value;
            return date.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string date = (string) value;
            return DateTime.Parse(date);
        }
    }
}
