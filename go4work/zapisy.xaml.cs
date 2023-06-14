using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Logika interakcji dla klasy zapisy.xaml
    /// </summary>
    public partial class zapisy : Page
    {
        //! na razie się zastanowić wgl nad tym rozwiązaniem
        // lista zapisanych ofert z poprzedniego ładowania
        // jeśli ta lista jest pusta oznacza to że trzeba ją załadować z bazy danych
        // można ręcznie odświeżyć listę klikając przycisk "odśwież"
        // private static List<work_offer> CachedOffers = new List<work_offer>();
        // może trzeba zrobić dodatkową zmienną WasChached żeby nie ładować ofert za każdym razem jeśli nie ma żadnych ofert??

        public zapisy()
        {
            InitializeComponent();

            LoadHotels();
            LoadOffers();
        }

        #region Obsługa przycisków ------------

        /// <summary>
        /// obsługuje guzik szukaj - wyszukuje oferty zgodne z ustawionymy filtrami
        /// </summary>
        private void SearchOffers(object sender, RoutedEventArgs e)
        {
            LoadOffers();
        }

        /// <summary>
        /// obsługuje guzik "Wyczyść filtry" - czyści filtry i odświeża listę ofert
        /// </summary>
        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            // ustawiamy tylko filtry na stan początkowy
            HotelList.SelectedIndex = 0;
            DateFilter.SelectedDate = null;

            // załadowanie od nowa ofert korzysta z wartości z kontrolek więc samo powinno załadować bez filtrów
            LoadOffers();
        }

        /// <summary>
        /// obsługuje guzik "Zapisane Oferty" - przenosi do strony zapisanych ofert
        /// </summary>
        private void GoToTakenOffers(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("taken.xaml", UriKind.Relative));
        }

        /// <summary>
        /// obsługa guzików "Zapisz" - zapisuje użytkownika na ofertę pracy
        /// </summary>
        private void Register(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            string command = $@"insert into taken_offers (offer_id, employee_id) values ({button.Tag}, '{App.logged_user_id}');
                                update work_offers set taken = 1 where id = {button.Tag};";

            SqlCommand sql_command = new SqlCommand(command, App.connection);
            sql_command.ExecuteNonQuery();

            MessageBox.Show("Zapisano na ofertę!");

            LoadOffers();
        }

        #endregion

        #region Funkcje wewnętrzne ---------

        /// <summary>
        /// ładuje oferty i odświeża listę - korzysta z kontrolek filtrów
        /// </summary>
        private void LoadOffers()
        {
            // zanim załadujemy nowe oferty wyczyść listę - zaczynamy od zera
            OfferList.Items.Clear();

            string today = DateTime.Now.ToString("yyyy-MM-dd"); // dzisiejsza data w formacie akceptowanym przez bazę danych
            string command = $"select work_offers.id, hotels.name as 'hotel_name', date, hours, salary from work_offers left join hotels on work_offers.hotel_id=hotels.id where date >= '{today}' and taken = 0";

            // jeśli jakiekolwiek wyszukiwanie jest aktywne to dodajemy je do zapytania sql
            if (HotelList.SelectedItem != null && (HotelList.SelectedItem as ComboBoxItem).Tag.ToString() != "*")
            {
                command += $" and hotel_id={Convert.ToInt32((HotelList.SelectedItem as ComboBoxItem).Tag)}";
            }

            if (DateFilter.SelectedDate != null)
            {
                command += $" and date='{DateFilter.SelectedDate.Value.ToString("yyyy-MM-dd")}'"; // w takim formacie daty przyjmuje baza danych
            }

            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            while (reader.Read())
            {
                // wiersz bazy danych wygląda tak tak: id, hotel_name, data, godziny, wynagrodzenie
                OfferList.Items.Add(new work_offer()
                {
                    id = reader["id"].ToString() ?? work_offer.DEFAULT_ID,
                    hotel_name = reader["hotel_name"].ToString() ?? work_offer.DEFAULT_HOTEL_NAME,
                    date = reader["date"].ToString() ?? work_offer.DEFAULT_DATE,
                    hours = reader["hours"].ToString() ?? work_offer.DEFAULT_HOURS,
                    salary = reader["salary"].ToString() ?? work_offer.DEFAULT_SALARY
                });
            }
            reader.Close();
        }

        /// <summary>
        /// ładuje hotele do filtra hoteli XD
        /// </summary>
        private void LoadHotels()
        {
            // teoretycznie można dorobić żeby przechowywało stare zazanczenie pomiędzy odświeżeniami

            string command = $"select id, name from hotels;";
            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            HotelList.Items.Clear(); // najpierw trzeba wyczyścić wszystkie elementy z listy
            HotelList.Items.Add(new ComboBoxItem() { Tag = "*", Content = "Wszystkie hotele", IsSelected = true }); // dodajemy opcję "wszystkie" na początek

            while (reader.Read())
            {
                // dopiero potem dodajemy od nowa
                HotelList.Items.Add(new ComboBoxItem() { Tag = reader.GetInt32(0), Content = reader.GetString(1) });
            }

            reader.Close();
        }

        #endregion
    }
}