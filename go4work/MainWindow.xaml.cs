﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=GABI;Initial Catalog=go4work;Trusted_Connection=True;");
            connection.Open();

            string command = $"select * from users where pesel='{str_pesel.Text}';";
            //string command = "select * from users";
            SqlCommand sql_command = new SqlCommand(command,connection);
            SqlDataReader reader = sql_command.ExecuteReader();
            //reader.Read();

            if (reader.Read()&&str_haslo.Text == reader["password"].ToString())
            {
                MessageBox.Show("tez kocham kotki");
            }
            else
            {
                MessageBox.Show("wole pieski");
            }
            connection.Close();
        }
    }
}
