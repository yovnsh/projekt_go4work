using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
            string command = $@"insert into users (pesel, name, surname, password, city, street, apartament_number, card_number, telephone_number)
                                values ('{str_pesel.Text}', '{str_name.Text}', '{str_surname.Text}', '{str_password.Text}', '{str_city.Text}', '{str_street.Text}',
                                        '{str_aparament_n.Text}', '{str_card_n.Text}', '{str_telephone_n.Text}');";
            SQLiteCommand SQLiteCommand = new SQLiteCommand(command, App.connection);
            //SQLiteDataReader reader = SQLiteCommand.ExecuteReader();
            SQLiteCommand.ExecuteNonQuery();

            MessageBox.Show("zarejestrowano!");
            this.NavigationService.Navigate(new Uri("logowanie.xaml", UriKind.Relative));
        }

        private void str_city_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
