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
    /// Logika interakcji dla klasy taken.xaml
    /// </summary>
    public partial class taken : Page
    {
        /// <summary>
        /// liczba ofert wyświetlanych na stronie
        /// </summary>
        public const int ITEMS_PER_PAGE = 10;

        public taken()
        {
            InitializeComponent();

            RegisteredOffers.ItemsPerPage = ITEMS_PER_PAGE;
            RegisteredOffers.OfferReloader += OffersChangePage;

            RegisteredOffers.ButtonText = "Wypisz się";
            RegisteredOffers.ButtonAction += UnRegister;

            LoadPage(0);
        }

        /// <summary>
        /// przycisk wyrejestrowania z oferty
        /// </summary>
        private void UnRegister(object? sender, EventArgs args)
        {
            // usuwa ofertę z listy zarejestrowanych i zmienia jej status na wolny
            // TODO: sprawdzić czy da się lepiej to zrobić
            try
            {
                App.db.JobOffers.Remove(App.db.JobOffers.Find((sender as Button).Tag));
                App.db.JobOffers.Find((sender as Button).Tag).WasAccepted = false;
                App.db.SaveChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show("błąd wyrejestrowania");
                Debug.WriteLine($"UnRegister: {e.Message}");
            }

            MessageBox.Show("wyrejestrowano!");

            LoadPage(RegisteredOffers.CurrentPage); // ładujemy aktualną stronę od nowa

        }

        private void OffersChangePage(object? sender, EventArgs e)
        {
            LoadPage((e as niewiem.ReloadEventArgs).RequestedPage);
        }

        /// <summary>
        /// pobiera liczbę stron
        /// </summary>
        private int GetPages()
        {
            var query = from offer in App.db.AcceptedOffers
                        where offer.UserPesel == App.logged_user_id
                        select offer;

            int result = query.Count() / ITEMS_PER_PAGE; // liczba elementów / liczba elementów na stronę = strony
            if (query.Count() % ITEMS_PER_PAGE != 0) // jeśli jest jakaś reszta z dzielenia to dodajemy jeszcze jedną stronę
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
            var query = from offer in App.db.AcceptedOffers
                        where offer.UserPesel == App.logged_user_id
                        select offer;

            var result = query.Skip(i*ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToList();

            foreach (var item in result)
            {
                try
                {
                    RegisteredOffers.Items.Add(item.JobOffer);
                }
                catch(Exception e)
                {
                    Debug.WriteLine($"Błąd wyświetlania oferty: {e.Message}");
                }
            }
        }

        /// <summary>
        /// obsługuje przycisk powrotu do poprzedniej strony
        /// </summary>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("zapisy.xaml", UriKind.Relative));
        }

        /// <summary>
        /// obsługuje przycisk przejścia do strony dodawania oferty
        /// </summary>
        private void CreateOffer(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("DodajOferty.xaml", UriKind.Relative));
        }

        private void RegisteredOffers_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RegisteredOffers_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
