using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace go4work.Validators
{
    /// <summary>
    /// sprawdza poprawność godziny
    /// </summary>
    class HourValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string HourNumber))
            {
                return new ValidationResult(false, "Zawartość pola tekstowego musi byc ciągiem znaków");
            }
            else if (!HourNumber.All(char.IsDigit))
            {
                return new ValidationResult(false, "Godzina może zawierać tylko cyfry");
            }
            else if(HourNumber.Length == 0)
            {
                return ValidationResult.ValidResult;
            }

            int HourValue = Convert.ToInt32(HourNumber);
            if (HourValue < 0 || HourValue > 23)
            {
                return new ValidationResult(false, "Godzina musi być z przedziału 0 - 23");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
