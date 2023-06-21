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
using go4work.Models;
using Microsoft.EntityFrameworkCore;

namespace go4work
{
    /// <summary>
    /// Logika interakcji dla klasy zapisy.xaml
    /// </summary>
    public partial class zapisy : Page
    {
        /// <summary>
        /// liczba ofert na strone
        /// </summary>
        public const int OFFERS_PER_PAGE = 3;

        /// <summary>
        /// numer pierwszej strony
        /// </summary>
        public const int FIRST_PAGE = 0;

        public zapisy()
        {
            InitializeComponent();

            OfferList.ItemsPerPage = OFFERS_PER_PAGE;
            OfferList.OfferReloader += OffersChangePage; // ustawiamy aktualizator stron

            OfferList.ButtonText = "Zapisz się"; // ustawiamy tekst na guziku
            OfferList.ButtonAction += Register;

            //LoadHotels(); // ładujemy hotele do filtru hoteli
            LoadOffers(FIRST_PAGE); // tylko pierwszą strona
        }

        /// <summary>
        /// event handler dla zmiany strony ofert
        /// </summary>
        private void OffersChangePage(object? sender, EventArgs e)
        {

            LoadOffers((e as niewiem.ReloadEventArgs).RequestedPage);
        }

        #region Obsługa przycisków ------------

        /// <summary>
        /// obsługuje guzik szukaj - wyszukuje oferty zgodne z ustawionymy filtrami
        /// </summary>
        private void SearchOffers(object sender, RoutedEventArgs e)
        {
            LoadOffers(FIRST_PAGE); // ładujemy od początku co powoduje zastosowanie filtrów
        }

        /// <summary>
        /// obsługa guzików "Zapisz" - zapisuje użytkownika na ofertę pracy
        /// </summary>
        private void Register(object? sender, EventArgs e)
        {
            try
            {
                App.db.AcceptedOffers.Add(new AcceptedOffer() {
                    UserPesel = App.logged_user.Pesel,
                    JobOfferID = Convert.ToInt32((sender as Button).Tag)
                });
                App.db.JobOffers.Find((sender as Button).Tag).WasAccepted = true;
                App.db.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("błąd zapisu na ofertę");
                Debug.WriteLine("zapisy:Register(): " + ex.Message);
            }

            MessageBox.Show("Zapisano na ofertę!");
            LoadOffers(OfferList.CurrentPage); // ładujemy ponownie aktualną stronę
        }

        #endregion

        #region Funkcje wewnętrzne ---------

        /// <summary>
        /// ładuje oferty i odświeża listę - korzysta z kontrolek filtrów
        /// </summary>
        private void LoadOffers(int page)
        {
            // zanim załadujemy nowe oferty wyczyść listę - zaczynamy od zera
            OfferList.Items.Clear();
            OfferList.PageCount = GetPages(); // aktualizujemy liczbę stron

            try
            {
                var query = from work_offer in App.db.JobOffers
                            where work_offer.WasAccepted == false
                            select work_offer;


                //query = ApplyFilters(query); // aplikujemy filtry

                var results = query.Skip(page * OFFERS_PER_PAGE).Take(OFFERS_PER_PAGE).Include("Hotel");
                OfferList.Items.Clear();
                foreach (var result in results)
                {
                    OfferList.Items.Add(result);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("błąd ładownania ofert");
                Debug.WriteLine("zapisy:LoadOffers(): " + e.Message);
            }
        }

        /// <summary>
        /// sprawdza ile jest stron wyników w aktualnych filtrach
        /// </summary>
        private int GetPages()
        {
            var query = from work_offer in App.db.JobOffers
                        where work_offer.WasAccepted == false
                        select work_offer;

            //query = ApplyFilters(query); // aplikujemy filtry

            int calculated_pages = query.Count() / OFFERS_PER_PAGE; // pamiętajmy że to dzielenie bez reszty
            if (query.Count() % OFFERS_PER_PAGE != 0) calculated_pages++; // jeśli jest reszta to trzeba dodać jeszcze jedną stronę

            return calculated_pages;
        }

        #endregion
    }
}