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
    /// Logika interakcji dla klasy DodajOferty.xaml
    /// </summary>
    public partial class DodajOferty : Page
    {
        public DodajOferty()
        {
            InitializeComponent();

            LoadHotels(); // ładujemy wybory hotteli
        }

        /// <summary>
        /// ładuje hotele do filtra hoteli XD
        /// </summary>
        private void LoadHotels()
        {
            // teoretycznie można dorobić żeby przechowywało stare zazanczenie pomiędzy odświeżeniami

            string command = $"select id, name from hotels;";
            SQLiteCommand sql_command = new SQLiteCommand(command, App.connection);
            SQLiteDataReader reader = sql_command.ExecuteReader();

            Hotels.Items.Clear(); // najpierw trzeba wyczyścić wszystkie elementy z listy

            while (reader.Read())
            {
                // dopiero potem dodajemy od nowa
                Hotels.Items.Add(new ComboBoxItem() { Tag = reader.GetInt32(0), Content = reader.GetString(1) });
            }

            reader.Close();
        }

        private void AddOffer(object? sender, RoutedEventArgs args)
        {
            string command = $@"insert into work_offers (hotel_id, date, hours, salary, taken) 
                                values ({(Hotels.SelectedItem as ComboBoxItem).Tag}, '{Data.SelectedDate.Value.ToString("yyyy-MM-dd")}', {str_hours.Text}, {str_salary.Text}, 0)";
            SQLiteCommand sql_command = new SQLiteCommand(command, App.connection);
            sql_command.ExecuteNonQuery();

            MessageBox.Show("Dodano ofertę pracy");
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void AddHotel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("DodajHotele.xaml", UriKind.Relative));
        }
    }
}
