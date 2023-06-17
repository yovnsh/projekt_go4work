using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using go4work.Contexts;

namespace go4work
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// baza danych - dostępna dla całej aplikacji
        /// </summary>
        public static JobContext db = new JobContext();
        public static string logged_user_id = "-1";

        protected override void OnExit(ExitEventArgs e)
        {
            db.Dispose();

            base.OnExit(e);
        }
    }
}