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
    /// Logika interakcji dla klasy DodajOferty.xaml
    /// </summary>
    public partial class DodajOferty : Page
    {
        public DodajOferty()
        {
            InitializeComponent();

            LoadHotels(); // ładujemy wybory hoteli
        }

        /// <summary>
        /// ładuje hotele do filtra hoteli
        /// </summary>
        private void LoadHotels()
        {
            // teoretycznie można dorobić żeby przechowywało stare zazanczenie pomiędzy odświeżeniami

            var query = from hotel in App.db.Hotels
                        select new { hotel.ID, hotel.Name };

            Hotels.Items.Clear(); // najpierw trzeba wyczyścić wszystkie elementy z listy

            foreach(var hotel in query)
            {
                Hotels.Items.Add(new ComboBoxItem() { Tag = hotel.ID, Content = hotel.Name });
            }
        }

        /// <summary>
        /// obsługuje kliknięcie guzika "dodaj ofertę"
        /// tworzy nową ofertę w bazie danych na podstawie danych z formularza
        /// </summary>
        private void AddOffer(object? sender, RoutedEventArgs args)
        {
            if (Hotels.SelectedItem == null)
            {
                MessageBox.Show("Wybierz hotel");
                return;
            }

            if (Data.SelectedDate == null)
            {
                MessageBox.Show("Wybierz datę");
                return;
            }

            if (str_hours.Text == "")
            {
                MessageBox.Show("Podaj ilość godzin");
                return;
            }

            if(str_salary.Text == "")
            {
                MessageBox.Show("Podaj wynagrodzenie");
                return;
            }

            ComboBoxItem? choosen_hotel = Hotels.SelectedItem as ComboBoxItem;
            if (choosen_hotel == null)
            {
                MessageBox.Show("Błąd dodawania oferty");
                Debug.WriteLine("AddOffer: selecteditem nie jest comboboxitem???");
                return;
            }

            //TODO: walidacja danych

            try
            {
                App.db.JobOffers.Add(new Models.JobOffer()
                {
                    HotelID = Convert.ToInt32(choosen_hotel.Tag.ToString()),
                    Date = Data.SelectedDate.Value,
                    Hours = Convert.ToInt32(str_hours.Text),
                    Salary = Convert.ToInt32(str_salary.Text)
                });
            }
            catch(Exception e)
            {
                MessageBox.Show("Błąd dodawania oferty");
                Debug.WriteLine("AddOffer: " + e.Message);
                return;
            }

            MessageBox.Show("Dodano ofertę pracy");
        }

        /// <summary>
        /// obsługuje guzik "wróć"
        /// </summary>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //TODO: poprawić nawigację tak żeby pokazywała do konkretnego miejsca
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// obsługuje guzik "dodaj hotel"
        /// przechodzi do strony dodawania hoteli
        /// </summary>
        private void AddHotel(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("DodajHotele.xaml", UriKind.Relative));
        }
    }
}
