using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    /// Logika interakcji dla klasy DodajHotele.xaml
    /// </summary>
    public partial class DodajHotele : Page
    {
        public DodajHotele()
        {
            InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void AddHotel(object sender, RoutedEventArgs e)
        {
            try
            {
                App.db.Hotels.Add(new Models.Hotel { Name = str_name.Text });
                App.db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd dodawania hotelu: ten hotel prawdopodbnie istnieje");
                Debug.WriteLine($"Err dodawanie hotelu: {ex.Message}");
                return;
            }

            MessageBox.Show($"Dodano hotel {str_name.Text}");
        }
    }
}
