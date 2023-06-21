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
    /// Logika interakcji dla klasy archived.xaml
    /// </summary>
    public partial class archived : Page
    {
        /// <summary>
        /// liczba ofert wyświetlanych na stronie
        /// </summary>
        public const int ITEMS_PER_PAGE = 3;

        /// <summary>
        /// numer pierwszej strony
        /// </summary>
        public const int FIRST_PAGE = 0;

        public archived()
        {
            InitializeComponent();

            ArchivedOffers.ItemsPerPage = ITEMS_PER_PAGE;
            ArchivedOffers.OfferReloader += OffersChangePage;

            LoadPage(FIRST_PAGE);
        }

        /// <summary>
        /// załadowywuje odpowiednią stronę
        /// </summary>
        private void OffersChangePage(object? sender, EventArgs args)
        {
            LoadPage((sender as niewiem.ReloadEventArgs).RequestedPage);
        }

        private int GetPages()
        {
            var query = from offer in App.db.AcceptedOffers.Include(x => x.JobOffer)
                        where offer.JobOffer.Date < DateTime.Now && offer.UserPesel == App.logged_user.Pesel
                        select offer;

            int count = query.Count();

            int result = count / ITEMS_PER_PAGE; // liczba elementów / liczba elementów na stronę = strony
            if (count % ITEMS_PER_PAGE != 0) // jeśli jest jakaś reszta z dzielenia to dodajemy jeszcze jedną stronę
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
            ArchivedOffers.Items.Clear();
            ArchivedOffers.PageCount = GetPages();

            // jeśli nie ma co ładować to nie ładujemy
            if (ArchivedOffers.PageCount == 0)
            {
                return;
            }

            try
            {
                var query = from offer in App.db.AcceptedOffers.Include(x => x.JobOffer)
                            where offer.JobOffer.Date < DateTime.Now && offer.UserPesel == App.logged_user.Pesel
                            select offer;

                foreach (var item in query)
                {
                    try
                    {
                        ArchivedOffers.Items.Add(item.JobOffer);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Błąd wyświetlania oferty: {e.Message}");
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("błąd ładowania strony");
                Debug.WriteLine($"LoadPage: {err.Message}");
            }
        }
    }
}
