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
    /// Interaction logic for OrderUpdateWindow.xaml
    /// </summary>
    public partial class OrderCreateOrUpdateWindow : Window
    {
        public OrderCreateOrUpdateWindow(RestCollection<Pizza> pizzas, RestCollection<Drink> drinks)
        {
            InitializeComponent();
            Order = new PromoOrder();

            cb_pizzas.ItemsSource = pizzas;
            cb_pizzas.SelectedItem = pizzas.FirstOrDefault();
            cb_drinks.ItemsSource = drinks;
            cb_drinks.SelectedItem = drinks.FirstOrDefault();

            sp.DataContext = Order;
            dp_timeoforder.SetBinding(DatePicker.SelectedDateProperty, new Binding("TimeOfOrder"));
            sl_discount.SetBinding(Slider.ValueProperty, new Binding("DiscountPercentage"));

        }

        public OrderCreateOrUpdateWindow(PromoOrder selectedOrder, RestCollection<Pizza> pizzas, RestCollection<Drink> drinks)
        {
            InitializeComponent();
            Order = new PromoOrder()
            {
                Id = selectedOrder.Id,
                DiscountPercentage = selectedOrder.DiscountPercentage,
                TimeOfOrder = selectedOrder.TimeOfOrder,
                DrinkId = selectedOrder.DrinkId,
                PizzaId = selectedOrder.PizzaId,

            };

            cb_pizzas.ItemsSource = pizzas;
            cb_drinks.ItemsSource = drinks;

            sp.DataContext = Order;
            dp_timeoforder.SetBinding(DatePicker.SelectedDateProperty, new Binding("TimeOfOrder"));
            sl_discount.SetBinding(Slider.ValueProperty, new Binding("DiscountPercentage"));
            cb_pizzas.SelectedItem = pizzas.FirstOrDefault(x => x.Id == selectedOrder.PizzaId);
            cb_drinks.SelectedItem = drinks.FirstOrDefault(x => x.Id == selectedOrder.DrinkId);

        }

        public PromoOrder Order { get; internal set; }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            Order.PizzaId = cb_pizzas.SelectedIndex + 1;
            Order.DrinkId = cb_drinks.SelectedIndex + 1;
            this.DialogResult = true;
        }
    }
}
