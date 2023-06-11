using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace go4work
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SqlConnection connection;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            connection = new SqlConnection("Data Source=GABI;Initial Catalog=go4work;Trusted_Connection=True;");
            connection.Open();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            connection.Close();
        }
    }
}