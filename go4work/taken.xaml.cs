using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public taken()
        {
            InitializeComponent();

            string command = $"select offer_id, hotels.name as 'hotel_name', work_offers.date, work_offers.hours, work_offers.salary from taken_offers left join work_offers on offer_id = work_offers.id left join hotels on work_offers.hotel_id = hotels.id where employee_id = '{App.logged_user_id}'";
            SqlCommand sql_command = new SqlCommand(command, App.connection);
            SqlDataReader reader = sql_command.ExecuteReader();

            if(reader.FieldCount == 0)
            {
                OfferList.Children.Add(new TextBlock() { Text = "Brak ofert" });
            } else
            {
                while(reader.Read())
                {
                    OfferList.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });
                    var new_row = new niewiem(){
                        Hotel = reader.GetString(1),
                        Data = reader.GetDateTime(2).ToShortDateString(),
                        Godziny = reader.GetInt32(3).ToString(),
                        Wynagrodzenie = reader.GetInt32(4).ToString(),
                        ID = reader.GetInt32(0).ToString()
                    };
                    Grid.SetRow(new_row, OfferList.RowDefinitions.Count-1);
                    OfferList.Children.Add(new_row);
                }
            }
            reader.Close();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("zapisy.xaml", UriKind.Relative));
        }
    }
}
