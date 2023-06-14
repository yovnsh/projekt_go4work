using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
            string command = $"insert into hotels values ('{str_name.Text}')";
            SQLiteCommand sql_command = new SQLiteCommand(command, App.connection);
            sql_command.ExecuteNonQuery();

            MessageBox.Show("Dodano hotel");
        }
    }
}
