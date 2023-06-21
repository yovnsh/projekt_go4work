using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private List<SuperiorTextBox> Inputs;

        public DodajOferty()
        {
            InitializeComponent();

            Inputs = new List<SuperiorTextBox>()
            {
                str_shift_start,
                str_shift_end,
                str_salary
            };

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

            Hotels.Items.Add(new ComboBoxItem() { Tag = -1, Content = "Wybierz hotel" }); // dodajemy pierwszy element (nie można wybrać)
            Hotels.SelectedIndex = 0; // ustawiamy na pierwszy element


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
            if (Hotels.SelectedItem == null || (Hotels.SelectedItem as ComboBoxItem).Tag.ToString() == "-1")
            {
                MessageBox.Show("Wybierz hotel");
                return;
            }

            if (Data.SelectedDate == null)
            {
                MessageBox.Show("Wybierz datę");
                return;
            }

            if(Inputs.Any(x => x.HasError || string.IsNullOrEmpty(x.Text)))
            {
                MessageBox.Show("Niepoprawne dane");
                return;
            }

            ComboBoxItem? choosen_hotel = Hotels.SelectedItem as ComboBoxItem;
            if (choosen_hotel == null)
            {
                MessageBox.Show("Błąd dodawania oferty");
                return;
            }

            try
            {
                App.db.JobOffers.Add(new Models.JobOffer()
                {
                    HotelID = Convert.ToInt32(choosen_hotel.Tag.ToString()),
                    Date = Data.SelectedDate.Value,
                    ShiftStart = Convert.ToInt32(str_shift_start.Text),
                    ShiftEnd = Convert.ToInt32(str_shift_end.Text),
                    Salary = Convert.ToInt32(str_salary.Text)
                });
                App.db.SaveChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show("Błąd dodawania oferty");
                Debug.WriteLine("AddOffer: " + e.Message);
                return;
            }

            MessageBox.Show("Dodano ofertę pracy");
        }
    }
}
