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
    class CardNumberValidator: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!(value is string CardNumber))
            {
                return new ValidationResult(false, "Zawartość pola tekstowego musi byc ciągiem znaków");
            }
            else if(!CardNumber.All(char.IsDigit))
            {
                return new ValidationResult(false, "Numer karty może zawierać tylko cyfry");
            }
            else if(CardNumber.Length != 16)
            {
                return new ValidationResult(false, "Numer karty musi mieć 16 cyfr");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
