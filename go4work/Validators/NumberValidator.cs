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
    /// sprawdza poprawność numeru karty kredytowej
    /// </summary>
    class NumberValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string Number))
            {
                return new ValidationResult(false, "Zawartość pola tekstowego musi byc ciągiem znaków");
            }
            else if (!Number.All(char.IsDigit))
            {
                return new ValidationResult(false, "Tylko cyfry są dopuszczalne");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
