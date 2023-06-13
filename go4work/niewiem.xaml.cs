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
    /// Logika interakcji dla klasy niewiem.xaml
    /// </summary>
    public partial class niewiem : UserControl
    {
        public string Hotel { get; set; } = "-- hotel --";
        public string Data { get; set; } = "-- data --";
        public string Godziny { get; set; } = "-- godziny --";
        public string Wynagrodzenie { get; set; } = "-- wynagrodzenie --";
        public string ID { get; set; } = "-1";

        public niewiem()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            string command = $@"insert into taken_offers (offer_id, employee_id) values ({this.ID}, '{App.logged_user_id}');
                                update work_offers set taken = 1 where id = {this.ID};";

            SqlCommand sql_command = new SqlCommand(command, App.connection);
            sql_command.ExecuteNonQuery();

            MessageBox.Show("Zapisano na ofertę!");
            //TODO: odświeżyć listę ofert
        }
    }
}