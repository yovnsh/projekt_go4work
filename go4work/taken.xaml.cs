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
    /// Logika interakcji dla klasy taken.xaml
    /// </summary>
    public partial class taken : Page
    {
        public const int ITEMS_PER_PAGE = 10;

        public taken()
        {
            InitializeComponent();

            RegisteredOffers.ItemsPerPage = ITEMS_PER_PAGE;
            RegisteredOffers.OfferReloader += OffersChangePage;

            RegisteredOffers.ButtonText = "Wypisz się";
            RegisteredOffers.ButtonAction += (object? sender, EventArgs e) => MessageBox.Show("Wypisano");

            LoadPage(0);
        }

        private void OffersChangePage(object? sender, EventArgs e)
        {
            LoadPage((e as niewiem.ReloadEventArgs).RequestedPage);
        }

        // pobiera liczbę stron danych
        private int GetPages()
        {
            string command = $"select count(*) from taken_offers where employee_id = '{App.logged_user_id}'";
            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            if (!reader.Read())
            {
                Debug.WriteLine("Nie udało się pobrać liczby stron");
                return 1;
            }

            int result = reader.GetInt32(0) / ITEMS_PER_PAGE; // liczba elementów / liczba elementów na stronę = strony
            if (reader.GetInt32(0) % ITEMS_PER_PAGE != 0) // jeśli jest jakaś reszta z dzielenia to dodajemy jeszcze jedną stronę
            {
                result++;
            }

            return result;
        }

        /// <summary>
        /// ładuje porcję ofert do listy
        /// </summary>
        /// <param name="i">numer strony</param>
        private void LoadPage(int i)
        {
            // nie ma potrzeby sprawdzać ponownie liczby stron bo nie ma filtrów

            string command = $@"select work_offers.id, hotels.name as 'hotel_name', work_offers.date, work_offers.hours, work_offers.salary 
                                from taken_offers 
                                left join work_offers on offer_id = work_offers.id
                                left join hotels on work_offers.hotel_id = hotels.id
                                where employee_id = '{App.logged_user_id}'
                                order by work_offers.date
                                offset {i * ITEMS_PER_PAGE} rows fetch next {ITEMS_PER_PAGE} rows only";

            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            RegisteredOffers.Items.Clear(); // zanim zmodyfikujemy to musimy wyczyścić
            while (reader.Read())
            {
                RegisteredOffers.Items.Add(new work_offer()
                {
                    id = reader.GetInt32(0).ToString(),
                    hotel_name = reader.GetString(1),
                    date = reader.GetDateTime(2).ToShortDateString(),
                    hours = reader.GetInt32(3).ToString(),
                    salary = reader.GetInt32(4).ToString()
                });
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("zapisy.xaml", UriKind.Relative));
        }
    }
}
