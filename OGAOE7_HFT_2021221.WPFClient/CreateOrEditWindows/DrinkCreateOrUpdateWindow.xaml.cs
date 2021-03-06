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
    /// Interaction logic for DrinkCreateOrUpdateWindow.xaml
    /// </summary>
    public partial class DrinkCreateOrUpdateWindow : Window
    {
        public DrinkCreateOrUpdateWindow()
        {
            InitializeComponent();
            Drink = new Drink();

            sp.DataContext = Drink;
            tb_name.SetBinding(TextBox.TextProperty, new Binding("Name"));
            tb_price.SetBinding(TextBox.TextProperty, new Binding("Price"));
            cb_promotional.SetBinding(CheckBox.IsCheckedProperty, new Binding("Promotional"));
        }
        public DrinkCreateOrUpdateWindow(Drink selectedDrink)
        {
            InitializeComponent();
            Drink = new Drink()
            {
                Id = selectedDrink.Id,
                Name = selectedDrink.Name,
                Price = selectedDrink.Price,
                Promotional = selectedDrink.Promotional,
            };

            sp.DataContext = Drink;
            tb_name.SetBinding(TextBox.TextProperty, new Binding("Name"));
            tb_price.SetBinding(TextBox.TextProperty, new Binding("Price"));
            cb_promotional.SetBinding(CheckBox.IsCheckedProperty, new Binding("Promotional"));
        }

        public Drink Drink { get; internal set; }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}