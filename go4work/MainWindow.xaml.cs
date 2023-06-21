using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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

            bool changed_avatar = false; // czy udało się załadować avatar
            try
            {
                if (App.logged_user.AvatarPath != null)
                {
                    if(File.Exists(App.logged_user.AvatarPath))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = new Uri(App.logged_user.AvatarPath, UriKind.Relative);
                        image.EndInit();
                        
                        Avatar.Background = new ImageBrush(image);
                        changed_avatar = true;
                    } 
                    else
                    {
                        Debug.WriteLine("nie odnaleziono pliku z avatarem - usuwanie wpisów w bazie");
                        App.logged_user.AvatarPath = null;
                        App.db.SaveChanges();
                    }
                }
            }
            catch(Exception err)
            {
                Debug.WriteLine($"MainWindow Avatar: " + err.Message);
            }

            // jeśli nie załadowano żadnego to ładujemy domyślny
            if(!changed_avatar)
            {
                Avatar.Background = (ImageBrush)FindResource("default_user");
            }
            

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

        /// <summary>
        /// próbuje załadować obrazek z pliku
        /// </summary>
        private void Avatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.png;*.jpeg;*.jpg";
            openFileDialog.Title = "Wybierz swój avatar";
            if(openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // losowa liczba + rozszerzenie pliku
                    var myUniqueFileName = $@".\Avatars\{Guid.NewGuid()}{System.IO.Path.GetExtension(openFileDialog.FileName)}";

                    // kopiujemy plik do folderu z avatarami
                    if (Directory.Exists(@".\Avatars") == false)
                    {
                        Directory.CreateDirectory(@".\Avatars");
                    }
                    File.Copy(openFileDialog.FileName, myUniqueFileName);

                    string? old_file = App.logged_user.AvatarPath; // zapamiętujemy stare zdjęcie
                    App.logged_user.AvatarPath = myUniqueFileName; // ustawiamy nowe zdjęcie

                    // zakładamy nowy obrazek
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(myUniqueFileName, UriKind.Relative);
                    image.EndInit();
                    Avatar.Background = new ImageBrush(image);

                    // usuwamy stare zdjęcie
                    if(old_file != null && File.Exists(old_file))
                    {
                        File.Delete(old_file);
                    }

                    // dopiero jak się wysztko udało to zapisujemy zmiany w bazie danych
                    App.db.SaveChanges();
                }
                catch(Exception err)
                {
                    MessageBox.Show("Błąd ustawiania avatara");
                    Debug.WriteLine($"Avatar_Click: " + err.Message);
                }
            }
        }
    }
}
