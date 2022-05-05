using OGAOE7_HFT_2021221.Models;
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
using System.Windows.Shapes;

namespace OGAOE7_HFT_2021221.WPFClient
{
    /// <summary>
    /// Interaction logic for PizzaCreateOrUpdateWindow.xaml
    /// </summary>
    public partial class PizzaCreateOrUpdateWindow : Window
    {
        public PizzaCreateOrUpdateWindow()
        {
            InitializeComponent();
        }

        public PizzaCreateOrUpdateWindow(Pizza selectedPizza) : this()
        {
            Pizza = selectedPizza;
            sp.DataContext = Pizza;

            tb_name.SetBinding(TextBox.TextProperty, new Binding("Name"));
            tb_price.SetBinding(TextBox.TextProperty, new Binding("Price"));
        }

        public Pizza? Pizza { get; private set; }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
