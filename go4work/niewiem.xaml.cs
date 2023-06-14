using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// niewiem.xaml reprezentuje widok ofert pracy
    /// 
    /// składa się z listy ofert pracy (można ustawić limit wyświetlanych ofert)
    /// na dnie posiada także przyciski do nawigacji po stronach które uruchamiają odpowiednią funkcję przeładowania
    /// 
    /// </summary>
    public partial class niewiem : UserControl
    {
        /// <summary>
        /// zbiór wszystkich wyświetlonych ofert
        /// zmodyfikowanie tej kolekcji powoduje automatyczną zmianę wyświetlonych ofert
        /// </summary>
        public ObservableCollection<work_offer> Items { get; set; } = new ObservableCollection<work_offer>();

        public niewiem()
        {
            InitializeComponent();

            this.DataContext = this;
        }
    }
}