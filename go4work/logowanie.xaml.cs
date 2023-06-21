using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        /// <summary>
        /// obsługuje guzik logowania (sprawdza czy użytkownik istnieje w bazie danych i hasło się zgadza)
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from user in App.db.Users
                        where user.Pesel == str_pesel.Text && user.Password == str_haslo.Text
                        select user;

            try
            {
                var result = query.Single();

                App.logged_user = result;

                GenerateSession(); // tworzy sesję użytkownika

                Window LoginWindow = Window.GetWindow(this);
                Window MainWindow = new MainWindow();
                MainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                MainWindow.Owner = LoginWindow;
                MainWindow.Show();

                App.Current.MainWindow = MainWindow;
                MainWindow.Owner = null;
                
                LoginWindow.Close();
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("błędnie wpisany pesel lub hasło");
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas logowania");
                Debug.WriteLine("Błąd logowania: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// obsługuje guzik rejestracji (przenosi do strony rejestracji)
        /// </summary>
        private void SignUp(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("rejestracja.xaml", UriKind.Relative));
        }

        /// <summary>
        /// generowanie sesji i zapisanie jej do pliku
        /// </summary>
        private void GenerateSession()
        {
            string sessid = Guid.NewGuid().ToString(); // tworzy nowy identyfikator sesji
            // dodajemy sesje do bazy
            App.db.Sessions.Add(new Models.Session
            {
                ID = sessid,
                UserPesel = App.logged_user.Pesel
            });

            // tworzymy plik z identyfikatorem sesji
            using (var writer = File.CreateText(@".\session.dat"))
            {
                writer.WriteLine(sessid);
            }

            // zapisujemy zmiany
            App.db.SaveChanges();
        }
    }
}