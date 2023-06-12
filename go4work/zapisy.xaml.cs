using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        // lista zapisanych ofert z poprzedniego ładowania
        // jeśli ta lista jest pusta oznacza to że trzeba ją załadować z bazy danych
        // można ręcznie odświeżyć listę klikając przycisk "odśwież"
        private static List<work_offer> CachedOffers = new List<work_offer>();
        // może trzeba zrobić dodatkową zmienną WasChached żeby nie ładować ofert za każdym razem jeśli nie ma żadnych ofert??

        public zapisy()
        {
            InitializeComponent();

            // ręcznie ładujemy hotele za każdym razem
            // może też wygodniej by było przechowywać je w pamięci?
            // A PRZEDE WSZYSTKIM W ODDZIELNEJ FUNKCJI
            string command = $"select id, name from hotels;";
            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();
            while (reader.Read())
            {
                HotelList.Items.Add(new ComboBoxItem() { Tag = reader.GetInt32(0), Content = reader.GetString(1) });
            }
            reader.Close();

            // jeśli nie ma załadowanych ofert to je załaduj
            if (CachedOffers.Count == 0)
            {
                LoadOffers();
            }
            UpdateOfferList(); // wyświetla załadowane oferty
        }

        // obsługuje przycisk szukaj
        private void SearchOffers(object sender, RoutedEventArgs e)
        {
            LoadOffers();
            UpdateOfferList();
        }

        // czyści filtry wyszukiwania
        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            HotelList.SelectedIndex = 0;
            DateFilter.SelectedDate = null;
            LoadOffers();
            UpdateOfferList();
        }

        // ładuje oferty z bazy danych i akutalizuje listę oraz zmienną CachedOffers
        // może w przyszłości uda się zrobić to asynchronicznie?
        private void LoadOffers()
        {
            // zanim załadujemy nowe oferty wyczyść listę - zaczynamy od zera
            CachedOffers.Clear();

            string command = $"select hotels.name as 'hotel_name', date, hours, salary from work_offers left join hotels on work_offers.hotel_id=hotels.id";

            bool search_active = false; // czy where już zostało wcześniej dodane żeby się nie powtarzało

            // jeśli jakiekolwiek wyszukiwanie jest aktywne to dodajemy je do zapytania sql
            if (HotelList.SelectedItem != null && (HotelList.SelectedItem as ComboBoxItem).Tag.ToString() != "*")
            {
                command += $" where hotel_id={Convert.ToInt32((HotelList.SelectedItem as ComboBoxItem).Tag)}";
                search_active = true; // informujemy że już jest jakiś warunek
            }

            if (DateFilter.SelectedDate != null)
            {
                if (!search_active)
                {
                    command += " where ";
                    search_active = true;
                }
                else
                {
                    command += " and ";
                }

                command += $"date='{DateFilter.SelectedDate.Value.ToString("yyyy-MM-dd")}'"; // w takim formacie daty przyjmuje baza danych
            }

            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            while (reader.Read())
            {
                // wiersz bazy danych wygląda tak tak: hotel_name, data, godziny, wynagrodzenie
                CachedOffers.Add(new work_offer(reader.GetString(0), reader.GetDateTime(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            reader.Close();
        }

        // wyświetla na stronie oferty z listy CachedOffers
        private void UpdateOfferList()
        {
            Offers.Children.Clear(); // usuwamy zawartość całej listy na stronie
            Offers.RowDefinitions.Clear(); // usuwamy wszystkie wiersze

            // jeśli nie ma ofert to wyświetl komunikat i przerwij działanie
            if (CachedOffers.Count == 0)
            {
                Offers.Children.Add(new TextBlock() { Text = "Brak ofert", TextAlignment = TextAlignment.Center });
                return;
            }

            // w przeciwnym wypadku wypisujemy wszystkie oferty
            foreach (work_offer offer in CachedOffers)
            {
                // tworzymy nowy wiersz dla każdej oferty
                Offers.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });

                // konstrukcja danych oferty
                var new_offer = new niewiem()
                {
                    Hotel = offer.hotel_name,
                    Data = offer.date.ToShortDateString(),
                    Godziny = offer.hours.ToString(),
                    Wynagrodzenie = offer.salary.ToString()
                };

                // przesunięcie oferty na odpowiedni wiersz
                Grid.SetRow(new_offer, Offers.RowDefinitions.Count - 1);

                // wstawienie oferty na stronę
                Offers.Children.Add(new_offer);
            }
        }
    }
}