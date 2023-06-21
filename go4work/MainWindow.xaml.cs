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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //private zapisy _zapisy = new zapisy(); // żeby przyspieszyć ładowanie

        public int CurrentTab { get; set; } = 0;
        public List<Button> Tabs { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Tabs = new List<Button>() { Tab1, Tab2 }; // lista zakładek
            Tabs[CurrentTab].IsEnabled = false; // włączamy aktywną zakładkę
        }


        /// <summary>
        /// guziki zakładek - zmienia aktualną zakładkę
        /// </summary>
        private void ChangeTab(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            Tabs[CurrentTab].IsEnabled = true; // włączamy poprzednią zakładkę
            CurrentTab = (button.Parent as Grid).Children.IndexOf(button); // ustawiamy nową zakładkę
            Tabs[CurrentTab].IsEnabled = false; // wyłączamy nową zakładkę

            yanosik.Source = new Uri(Tabs[CurrentTab].Tag.ToString(), UriKind.Relative); // ustawiamy nową zakładkę
        }
    }
}
