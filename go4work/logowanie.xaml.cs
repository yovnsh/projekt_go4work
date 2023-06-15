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
    /// Logika interakcji dla klasy logowanie.xaml
    /// </summary>
    public partial class logowanie : Page
    {

        public logowanie()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Segoe UI");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: uprościć funkcję

            string command = $"select password from users where pesel='{str_pesel.Text}';";
            SQLiteCommand sql_command = new SQLiteCommand(command, App.connection);
            SQLiteDataReader reader = sql_command.ExecuteReader();

            if (reader.Read() && str_haslo.Text == reader["password"].ToString())
            {
                App.logged_user_id = str_pesel.Text;

                MessageBox.Show("zalogowano");
                reader.Close();

                this.NavigationService.Navigate(new Uri("zapisy.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("błędnie wpisany pesel lub hasło");
                reader.Close();
            }
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("rejestracja.xaml", UriKind.Relative));
        }
    }
}