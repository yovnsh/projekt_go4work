using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// niewiem.xaml reprezentuje widok ofert pracy
    /// 
    /// składa się z listy ofert pracy (można ustawić limit wyświetlanych ofert)
    /// na dnie posiada takżę przyciski do nawigacji po stronach które uruchamiają odpowiednią funkcję przeładowania
    /// </summary>
    public partial class niewiem : UserControl
    {
        #region Zmienne ----------------------------
        /// <summary>
        /// zbiór wszystkich wyświetlonych ofert
        /// zmodyfikowanie tej kolekcji powoduje automatyczną zmianę wyświetlonych ofert
        /// jeśli będzie przechowywać więcej elementów niż ItemsPerPage to nie zostaną one wyświetlone
        /// </summary>
        public LimitedObservableCollection<work_offer> Items { get; set; } = new LimitedObservableCollection<work_offer>(DEFAULT_ITEMS_PER_PAGE);

        /// <summary>
        /// domyślna liczba ofert wyświetlanych na stronie
        /// </summary>
        public const int DEFAULT_ITEMS_PER_PAGE = 10;

        /// <summary>
        /// liczba wyświetlanaych ofert na stronie (można modyfikować w dowolnej chwili)
        /// </summary>
        public int ItemsPerPage
        {
            get => Items.Capacity;
            set => Items.Capacity = value;
        }

        /// <summary>
        /// numer aktualnej strony (zaznaczona strona w nawigacji na dole)
        /// zmiana powoduje wywołanie żądania przeładowania ofert
        /// </summary>
        public int CurrentPage
        {
            get => _current_page;
            set
            {
                // zabezpieczenie przed nieprawidłowymi danymi
                if (value < 0 || value >= PageCount)
                {
                    Debug.WriteLine("Próba ustawienia nieprawidłowej strony");
                    return;
                }

                // wywołujemy żądanie przeładowania ofert
                var args = new ReloadEventArgs()
                {
                    RequestedPage = value
                };
                OfferReloader?.Invoke(this, args);

                Debug.WriteLine("Strona: " + value);

                // ustawiamy aktualną stronę
                _current_page = value;

                GenerateNavigation();
            }
        }
        private int _current_page;

        /// <summary>
        /// liczba wszystkich stron (od tego zależy nawigacja na dole)
        /// jeśli jesteśmy na dalszej stronie niż chcemy ustawić to możliwie konieczne będzie przeładowanie ofert
        /// </summary>
        public int PageCount
        {
            get => _page_count;
            set
            {
                // jeśli nic się nie zmieniło to nie ma sensu nic robić
                if (value == _page_count)
                {
                    return;
                }

                // jeśli liczba stron jest zerowa lub ujemna to pokazujemy tekst brak ofert 
                if (value < 1)
                {
                    NoItems.Visibility = Visibility.Visible; // w tym jednym wypadku pokazujemy co tam mamy
                    Navigation.Visibility = Visibility.Collapsed; // chowamy nawigację skoro i tak nie ma stron
                    _page_count = 0;
                    return;
                }
                NoItems.Visibility = Visibility.Collapsed; // nomralnie zawsze chowamy komunikat o braku ofert

                // jeśli nie jest potrzebne wgl stronicowanie to chowamy nawigację
                if (value == 1)
                {
                    Navigation.Visibility = Visibility.Collapsed;
                }
                // wpw pokazujemy nawigację
                else
                {
                    Navigation.Visibility = Visibility.Visible;
                }

                // jeśli jesteśmy na stronie dalejszej niż jest to ustawiamy na ostatnią
                // TODO: albo na 0
                if (CurrentPage >= value)
                {
                    CurrentPage = value - 1;
                }
                _page_count = value;

                GenerateNavigation(); // po zmianie ilości stron trzeba od nowa wygenerować nawigację
            }
        }
        private int _page_count = 0;

        /// <summary>
        /// funkcja która powinna być wywoływana gdy chcemy przeładować oferty
        /// </summary>
        public event EventHandler<EventArgs>? OfferReloader;

        /// <summary>
        /// klasa argumentów dla OfferReloader
        /// </summary>
        public class ReloadEventArgs : EventArgs
        {
            public int RequestedPage { get; set; }
        }

        /// <summary>
        /// tekst przekazywany do przycisków
        /// </summary>
        public string ButtonText { get; set; } = "hehe button";

        /// <summary>
        /// akcja wykonywana przy kliknięciu dowolnego przycisku
        /// (każdy przycisk ma właściwość Tag)
        /// </summary>
        public event EventHandler<EventArgs>? ButtonAction;

        #endregion

        public niewiem()
        {
            InitializeComponent();

            this.DataContext = this;

            //GenerateNavigation();
        }

        public niewiem(int limit, int n_pages = 1) : this()
        {
            ItemsPerPage = limit;
            PageCount = n_pages;
        }

        /// <summary>
        /// generuje guziki na dole naweigacji
        /// wyjątkowo nieprzyjemny kod kurde bela
        /// </summary>
        private void GenerateNavigation()
        {
            PageList.Children.Clear();

            // jeśli jest mniej niż 5 stron to wypisujemy wszystkie
            if (PageCount <= 5)
            {
                for (int n = 0; n < PageCount; n++)
                {
                    var btn = new Button()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5, 5, 5, 5),
                        Content = (n + 1).ToString(), // numer strony
                        Tag = n.ToString(),         // też numer strony tylko od 0 numerowany
                    };
                    btn.Click += NavigateToPage; // obsługa klikania na strony

                    PageList.Children.Add(btn);
                }
            }
            else
            {
                // robimy układ guzików 1 2 3 4 ... l lub 1 ... n-1 n n+1 ... l

                var btn1 = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5, 5, 5, 5),
                    Content = "1",
                    Tag = "0"
                };
                btn1.Click += NavigateToPage;

                var btn_last = new Button()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5, 5, 5, 5),
                    Content = PageCount.ToString(),
                    Tag = (PageCount - 1).ToString()
                };
                btn_last.Click += NavigateToPage;

                // --- 1 guzik ---
                PageList.Children.Add(btn1);

                int start_index; // od jakiej liczby zaczynamy potem dodawac trzy guziki - przydatne później

                if (CurrentPage > 2)
                {
                    // jeśli jest jakaś znaczącza przerwa to wstawiamy trzykropek
                    PageList.Children.Add(new TextBlock() // trzykropek po guziku
                    {
                        Text = "...",
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    // jeśli odległość jest większa niż 2 to robimy (1 ... n-1 n n+1 ... last) czyli zaczynamy od n-1 aż po n+1
                    start_index = CurrentPage - 1;

                }
                else
                {
                    // jeśli odległość jest mniejsza niż 2 to pokazujemy 1 2 3 4 ... last - czyli wypisujemy 1 i 3 następne liczby począwszy od 2 (1 liczone od 0)
                    start_index = 1;
                }

                // --- 3 następne guziki ---
                int end_index = start_index + 3; // na którym numerku mamy zakończyć

                if (end_index >= PageCount) // jeśli jesteśmy na ostatniej stronie to nie możemy wypisać 3 następnych guzików
                {
                    start_index = PageCount - 4; // cofamy indeks do tyłu o 1
                    end_index = PageCount - 1; // więc wypisujemy do przedostatniego (bo ostatni jeszcze sam sie wstawi)
                }

                for (int n = start_index; n < end_index; n++)
                {
                    var btn = new Button()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Padding = new Thickness(5, 5, 5, 5),
                        Content = (n + 1).ToString(), // numer strony
                        Tag = n.ToString(),           // też numer strony tylko od 0 numerowany
                    };
                    btn.Click += NavigateToPage; // obsługa klikania na strony

                    PageList.Children.Add(btn);
                }

                if (CurrentPage < PageCount - 3)
                {
                    // jeśli jest jakaś znaczącza przerwa to wstawiamy trzykropek
                    PageList.Children.Add(new TextBlock()
                    {
                        Text = "...",
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                }

                // --- ostatni guzik ---
                PageList.Children.Add(btn_last); // ostatnia strona
            }

            UpdateDisabledButtons(); // musimy zadbać o aktywność przycisków
        }


        // wyłącza lub włącza przyciski nawigacji
        private void UpdateDisabledButtons()
        {
            // włączamy wszystkie pozostałe poza aktualną
            foreach (UIElement btn in PageList.Children)
            {
                if (btn is Button && Convert.ToInt32((btn as Button).Tag.ToString()) != CurrentPage)
                    btn.IsEnabled = true;
                else
                    btn.IsEnabled = false;

            }
        }

        /// <summary>
        /// obsługuje klinięcie guzika strony z numerkiem
        /// </summary>
        private void NavigateToPage(object sender, RoutedEventArgs e)
        {

            CurrentPage = Convert.ToInt32((sender as Button).Tag.ToString());
        }

        private void ButtonClick(object? sender, RoutedEventArgs e)
        {
            ButtonAction?.Invoke(sender, e);
        }
    }
}