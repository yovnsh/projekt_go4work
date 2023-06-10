using System;
using System.Collections.Generic;
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
        public string Dlugosc { get; set; } = "-- długość zmiany --"; // nie pamiętam co tam było w ostatnim

        public niewiem()
        {
            InitializeComponent();

            this.DataContext = this;
        }
    }
}