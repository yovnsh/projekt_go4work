using Microsoft.EntityFrameworkCore;
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
    /// Logika interakcji dla klasy lista_ofert_admin.xaml
    /// </summary>
    public partial class lista_ofert_admin : Page
    {
        /// <summary>
        /// liczba ofert na strone
        /// </summary>
        public const int OFFERS_PER_PAGE = 3;

        /// <summary>
        /// numer pierwszej strony
        /// </summary>
        public const int FIRST_PAGE = 0;

        public lista_ofert_admin()
        {
            InitializeComponent();

            OfferList.ItemsPerPage = OFFERS_PER_PAGE;
            OfferList.OfferReloader += OffersChangePage; // ustawiamy aktualizator stron

            LoadOffers(FIRST_PAGE); // tylko pierwszą strona
        }

        /// <summary>
        /// event handler dla zmiany strony ofert
        /// </summary>
        private void OffersChangePage(object? sender, EventArgs e)
        {

            LoadOffers((e as niewiem.ReloadEventArgs).RequestedPage);
        }

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
            catch (Exception e)
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
    }
}
