using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace go4work
{
    /// <summary>
    /// Logika interakcji dla klasy SuperiorTextBox.xaml
    /// </summary>
    public partial class SuperiorTextBox : UserControl
    {
        public bool HasError { get => Validation.GetHasError(input); }

        /// <summary>
        /// tekst wyświetlany kiedy nie został podany żaden tekst
        /// </summary>
        public string Placeholder { get; set; }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SuperiorTextBox), new PropertyMetadata(""));
        /// <summary>
        /// tekst wpisany w pole tekstowe
        /// jako dependency property bo potrzebujemy go w bindingu
        /// </summary>
        public string Text { 
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ValidatorProperty = DependencyProperty.Register("Validator", typeof(ValidationRule), typeof(SuperiorTextBox), new PropertyMetadata(null, UpdateValidators));
        /// <summary>
        /// validator, który będzie sprawdzał porawność wprowadzanych danych
        /// </summary>
        public ValidationRule? Validator { 
            get => (ValidationRule?)GetValue(ValidatorProperty);
            set => SetValue(ValidatorProperty, value);
        }

        /// <summary>
        /// aktualizuje walidatory - używany jako onchange w ValidatorProperty
        /// </summary>
        private static void UpdateValidators(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = obj as SuperiorTextBox;
            if(control == null)
            {
                return;
            }

            var binding = BindingOperations.GetBinding(control.input, TextBox.TextProperty);
            binding.ValidationRules.Clear();
            binding.ValidationRules.Add(args.NewValue as ValidationRule);
        }

        public SuperiorTextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
