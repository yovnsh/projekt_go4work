using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logika interakcji dla klasy rejestracja.xaml
    /// </summary>
    public partial class rejestracja : Page
    {
        public rejestracja()
        {
            InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            //TODO: validacja

            try
            {
                App.db.Users.Add(new Models.User
                {
                    Pesel = str_pesel.Text,
                    Name = str_name.Text,
                    Surname = str_surname.Text,
                    Password = str_password.Text,
                    City = str_city.Text,
                    Street = str_street.Text,
                    ApartamentNumber = str_aparament_n.Text,
                    CardNumber = str_card_n.Text,
                    TelephoneNumber = str_telephone_n.Text
                });
                App.db.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd dodawania użytkownika");
                Debug.WriteLine($"SignUp: {ex.Message}");
                return;
            }

            MessageBox.Show("zarejestrowano!");
            this.NavigationService.Navigate(new Uri("logowanie.xaml", UriKind.Relative));
        }

        private void str_city_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
