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

namespace OGAOE7_HFT_2021221.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SwitchToPizzaTable_Button_Click(this, new RoutedEventArgs());
        }

        private void SwitchToPizzaTable_Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SelectedPizza = null;

            //set ItemsSource to Pizzas
            Binding listBoxToPizzasBinding = new Binding("Pizzas");
            listBoxToPizzasBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            lb.SetBinding(ListBox.ItemsSourceProperty, listBoxToPizzasBinding);

            //set SelectedItem to SelectedPizza
            lb.SetBinding(ListBox.SelectedItemProperty, new Binding("SelectedPizza"));

            //reassign button commands
            btn_create.SetBinding(Button.CommandProperty, new Binding("CreatePizzaCommand"));
            btn_create.Content = "Create Pizza";
            btn_update.SetBinding(Button.CommandProperty, new Binding("UpdatePizzaCommand"));
            btn_update.Content = "Update Pizza";
            btn_delete.SetBinding(Button.CommandProperty, new Binding("DeletePizzaCommand"));
            btn_delete.Content = "Delete Pizza";

            //set up the datatemplate
            DataTemplate dt = new DataTemplate();

            //set up a stack panel
            FrameworkElementFactory pizzaItemFactory = new FrameworkElementFactory(typeof(StackPanel), "pizzaItemFactory");
            pizzaItemFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //set up a label for id
            FrameworkElementFactory idLabelFactory = new FrameworkElementFactory(typeof(Label));
            idLabelFactory.SetBinding(Label.ContentProperty, new Binding("Id"));
            pizzaItemFactory.AppendChild(idLabelFactory);

            //set up a label for name
            FrameworkElementFactory nameLabelFactory = new FrameworkElementFactory(typeof(Label));
            nameLabelFactory.SetBinding(Label.ContentProperty, new Binding("Name"));
            pizzaItemFactory.AppendChild(nameLabelFactory);

            //set up a label for price
            FrameworkElementFactory priceLabelFactory = new FrameworkElementFactory(typeof(Label));
            priceLabelFactory.SetBinding(Label.ContentProperty, new Binding("Price"));
            pizzaItemFactory.AppendChild(priceLabelFactory);

            //set up a label for promotional
            FrameworkElementFactory promotionalLabelFactory = new FrameworkElementFactory(typeof(Label));
            promotionalLabelFactory.SetBinding(Label.ContentProperty, new Binding("Promotional"));
            pizzaItemFactory.AppendChild(promotionalLabelFactory);

            //set the visual tree of the data template
            dt.VisualTree = pizzaItemFactory;

            //set the item template to be our shiny new data template
            lb.ItemTemplate = dt;
        }

        private void SwitchToDrinkTable_Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SelectedDrink = null;

            //set ItemsSource to Drinks
            lb.SetBinding(ListBox.ItemsSourceProperty, new Binding("Drinks"));
            //set SelectedItem to SelectedDrink
            lb.SetBinding(ListBox.SelectedItemProperty, new Binding("SelectedDrink"));

            //reassign button commands
            btn_create.SetBinding(Button.CommandProperty, new Binding("CreateDrinkCommand"));
            btn_create.Content = "Create Drink";
            btn_update.SetBinding(Button.CommandProperty, new Binding("UpdateDrinkCommand"));
            btn_update.Content = "Update Drink";
            btn_delete.SetBinding(Button.CommandProperty, new Binding("DeleteDrinkCommand"));
            btn_delete.Content = "Delete Drink";

            //set up the datatemplate
            DataTemplate dt = new DataTemplate();

            //set up a stack panel
            FrameworkElementFactory drinkItemFactory = new FrameworkElementFactory(typeof(StackPanel), "drinkItemFactory");
            drinkItemFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //set up a label for id
            FrameworkElementFactory idLabelFactory = new FrameworkElementFactory(typeof(Label));
            idLabelFactory.SetBinding(Label.ContentProperty, new Binding("Id"));
            drinkItemFactory.AppendChild(idLabelFactory);

            //set up a label for name
            FrameworkElementFactory nameLabelFactory = new FrameworkElementFactory(typeof(Label));
            nameLabelFactory.SetBinding(Label.ContentProperty, new Binding("Name"));
            drinkItemFactory.AppendChild(nameLabelFactory);

            //set up a label for price
            FrameworkElementFactory priceLabelFactory = new FrameworkElementFactory(typeof(Label));
            priceLabelFactory.SetBinding(Label.ContentProperty, new Binding("Price"));
            drinkItemFactory.AppendChild(priceLabelFactory);

            //set up a label for promotional
            FrameworkElementFactory promotionalLabelFactory = new FrameworkElementFactory(typeof(Label));
            promotionalLabelFactory.SetBinding(Label.ContentProperty, new Binding("Promotional"));
            drinkItemFactory.AppendChild(promotionalLabelFactory);

            //set the visual tree of the data template
            dt.VisualTree = drinkItemFactory;

            //set the item template to be our shiny new data template
            lb.ItemTemplate = dt;
        }

        private void SwitchToOrderTable_Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SelectedOrder = null;

            //set ItemsSource to Orders
            lb.SetBinding(ListBox.ItemsSourceProperty, new Binding("Orders"));
            //set SelectedItem to SelectedOrder
            lb.SetBinding(ListBox.SelectedItemProperty, new Binding("SelectedOrder"));

            //reassign button commands
            btn_create.SetBinding(Button.CommandProperty, new Binding("CreateOrderCommand"));
            btn_create.Content = "Create Order";
            btn_update.SetBinding(Button.CommandProperty, new Binding("UpdateOrderCommand"));
            btn_update.Content = "Update Order";
            btn_delete.SetBinding(Button.CommandProperty, new Binding("DeleteOrderCommand"));
            btn_delete.Content = "Delete Order";

            //set up the datatemplate
            DataTemplate dt = new DataTemplate();

            //set up a stack panel
            FrameworkElementFactory orderItemFactory = new FrameworkElementFactory(typeof(StackPanel), "orderItemFactory");
            orderItemFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //set up a label for id
            FrameworkElementFactory idLabelFactory = new FrameworkElementFactory(typeof(Label));
            idLabelFactory.SetBinding(Label.ContentProperty, new Binding("Id"));
            orderItemFactory.AppendChild(idLabelFactory);

            //set up a label for the time of order
            FrameworkElementFactory timeLabelFactory = new FrameworkElementFactory(typeof(Label));
            timeLabelFactory.SetValue(Label.ContentStringFormatProperty, "Time: {0}");
            timeLabelFactory.SetBinding(Label.ContentProperty, new Binding("TimeOfOrder"));
            orderItemFactory.AppendChild(timeLabelFactory);

            //set up a label for the selected pizza
            FrameworkElementFactory pizzaLabelFactory =new FrameworkElementFactory(typeof (Label));
            pizzaLabelFactory.SetValue(Label.ContentStringFormatProperty, "PizzaId: {0}");
            pizzaLabelFactory.SetBinding(Label.ContentProperty, new Binding("PizzaId"));
            orderItemFactory.AppendChild(pizzaLabelFactory);

            //set up a label for the selected drink
            FrameworkElementFactory drinkLabelFactory = new FrameworkElementFactory(typeof (Label));
            drinkLabelFactory.SetValue(Label.ContentStringFormatProperty, "DrinkId: {0}");
            drinkLabelFactory.SetBinding(Label.ContentProperty, new Binding("DrinkId"));
            orderItemFactory.AppendChild(drinkLabelFactory);

            //set up a label for the dicount percentage
            FrameworkElementFactory discountLabelFactory = new FrameworkElementFactory(typeof(Label));
            discountLabelFactory.SetValue(Label.ContentStringFormatProperty, "Discount: {0}%");
            discountLabelFactory.SetBinding(Label.ContentProperty, new Binding("DiscountPercentage"));
            orderItemFactory.AppendChild(discountLabelFactory);

            //set the visual tree of the data template
            dt.VisualTree = orderItemFactory;

            //set the item template to be our shiny new data template
            lb.ItemTemplate = dt;

        }
    }
}
