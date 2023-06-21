using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace go4work.Validators
{
    /// <summary>
    /// sprawdza poprawność podanego peselu
    /// </summary>
    public class PeselValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!(value is string pesel))
            {
                return new ValidationResult(false, "Zawartość pola tekstowego musi byc ciągiem znaków");
            }
            else if (!pesel.All(char.IsDigit))
            {
                return new ValidationResult(false, "Pesel może zawierać tylko cyfry");
            }
            else if (pesel.Length != 11)
            {
                return new ValidationResult(false, "Pesel musi mieć 11 znaków");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
