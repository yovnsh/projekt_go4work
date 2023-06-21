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
    /// sprawdza numery telefonu
    /// </summary>
    class PhoneNumberValidator: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!(value is string PhoneNumber))
            {
                return new ValidationResult(false, "Zawartość pola tekstowego musi byc ciągiem znaków");
            }
            else if(!PhoneNumber.All(char.IsDigit))
            {
                return new ValidationResult(false, "Numer telefonu może zawierać tylko cyfry");
            }
            else if(PhoneNumber.Length != 9)
            {
                return new ValidationResult(false, "Numer telefonu musi mieć 9 cyfr");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
