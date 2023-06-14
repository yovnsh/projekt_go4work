using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
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
        public static SQLiteConnection connection;
        public static string logged_user_id = "-1";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            connection = new SQLiteConnection("Data Source=DB.sqlite; Version=3");
            connection.Open();

            Debug.WriteLine("udało się połączyć z bazą danych");

            string users_table = "create table if not exists users (pesel varchar(11) primary key, password varchar(50));";
            string hotels_table = "create table if not exists hotels (id integer primary key autoincrement, name varchar(50));";
            string work_offers_table = "create table if not exists work_offers (id integer primary key autoincrement, hotel_id int, date datetime, hours int, salary int, taken int2, foreign key(hotel_id) references hotels(id));";
            string taken_offers_table = "create table if not exists taken (employee_id int, offer_id int, foreign key(employee_id) references users(pesel), foreign key(offer_id) references work_offers(id))";

            SQLiteCommand sql_command;

            sql_command = new SQLiteCommand(users_table, connection);
            sql_command.ExecuteNonQuery();

            Debug.WriteLine("tabela użytkowników zweryfikowana");

            sql_command = new SQLiteCommand(hotels_table, connection);
            sql_command.ExecuteNonQuery();

            Debug.WriteLine("tabela hoteli zweryfikowana");

            sql_command = new SQLiteCommand(work_offers_table, connection);
            sql_command.ExecuteNonQuery();

            Debug.WriteLine("tabela ofert pracy zweryfikowana");

            sql_command = new SQLiteCommand(taken_offers_table, connection);
            sql_command.ExecuteNonQuery();

            Debug.WriteLine("tabela zajętych ofert pracy zweryfikowana");

            Debug.WriteLine("baza danych gotowa do użycia!");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            connection.Close();
        }
    }
}