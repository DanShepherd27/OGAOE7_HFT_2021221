using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OGAOE7_HFT_2021221.WPFClient
{
    public class MainWindowViewModel : ObservableRecipient
    {

        private Pizza? selectedPizza;
        private Drink? selectedDrink;
        private PromoOrder? selectedOrder;

        public RestCollection<Pizza> Pizzas { get; set; }
        public RestCollection<Drink> Drinks { get; set; }
        public RestCollection<PromoOrder> Orders { get; set; }


        public Pizza? SelectedPizza
        {
            get => selectedPizza;
            set
            {
                if (value != null)
                {
                    selectedPizza = new Pizza()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Price = value.Price
                    };
                }
                else selectedPizza = null;
                OnPropertyChanged();
                //SetProperty(ref selectedPizza, value);
                ((RelayCommand)UpdatePizzaCommand).NotifyCanExecuteChanged();
                ((RelayCommand)DeletePizzaCommand).NotifyCanExecuteChanged();
            }
        }
        public Drink? SelectedDrink
        {
            get => selectedDrink;
            set
            {
                if (value != null)
                {
                    selectedDrink = new Drink()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Price = value.Price,
                        Promotional = value.Promotional,
                    };
                }
                else selectedDrink = null;
                OnPropertyChanged();
                //SetProperty(ref selectedDrink, value);
                ((RelayCommand)UpdateDrinkCommand).NotifyCanExecuteChanged();
                ((RelayCommand)DeleteDrinkCommand).NotifyCanExecuteChanged();
            }
        }
        public PromoOrder? SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if (value != null)
                {
                    selectedOrder = new PromoOrder()
                    {
                        Id = value.Id,
                        DiscountPercentage = value.DiscountPercentage,
                        TimeOfOrder = value.TimeOfOrder,
                        DrinkId = value.DrinkId,
                        PizzaId = value.PizzaId,
                    };
                }
                else selectedOrder = null;
                OnPropertyChanged();
                //SetProperty(ref selectedOrder, value);
                ((RelayCommand)UpdateOrderCommand).NotifyCanExecuteChanged();
                ((RelayCommand)DeleteOrderCommand).NotifyCanExecuteChanged();
            }
        }

        #region PizzaCommands
        public ICommand CreatePizzaCommand { get; set; }
        public ICommand UpdatePizzaCommand { get; set; }
        public ICommand DeletePizzaCommand { get; set; }
        #endregion

        #region DrinkCommands
        public ICommand CreateDrinkCommand { get; set; }
        public ICommand UpdateDrinkCommand { get; set; }
        public ICommand DeleteDrinkCommand { get; set; }
        #endregion

        #region OrderCommands
        public ICommand CreateOrderCommand { get; set; }
        public ICommand UpdateOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        #endregion

        //public static bool IsInDesignMode
        //{
        //    get
        //    {
        //        var prop = DesignerProperties.IsInDesignModeProperty;
        //        return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
        //    }
        //}

        public MainWindowViewModel()
        {
            Pizzas = new RestCollection<Pizza>("http://localhost:26548/", "pizza", "hub");
            Drinks = new RestCollection<Drink>("http://localhost:26548/", "drink", "hub");
            Orders = new RestCollection<PromoOrder>("http://localhost:26548/", "promoorder", "hub");

            CreatePizzaCommand = new RelayCommand(() =>
            {
                PizzaCreateOrUpdateWindow pizzaCreateWindow = new();
                pizzaCreateWindow.ShowDialog();
                if (pizzaCreateWindow.DialogResult == true)
                    Pizzas.Add(pizzaCreateWindow.Pizza);
            });
            UpdatePizzaCommand = new RelayCommand(() =>
            {
                if (SelectedPizza != null)
                {
                    PizzaCreateOrUpdateWindow pizzaUpdateWindow = new(SelectedPizza);
                    pizzaUpdateWindow.ShowDialog();
                    if (pizzaUpdateWindow.DialogResult == true)
                        Pizzas.Update(pizzaUpdateWindow.Pizza);
                }
            },
            () => { return SelectedPizza != null; });
            DeletePizzaCommand = new RelayCommand(() =>
            {
                if (SelectedPizza != null)
                {
                    Pizzas.Delete(SelectedPizza.Id, "pizza/id");
                    
                }
            },
            () => { return SelectedPizza != null; });

            CreateDrinkCommand = new RelayCommand(() =>
            {
                DrinkCreateOrUpdateWindow drinkCreateWindow = new();
                drinkCreateWindow.ShowDialog();
                if (drinkCreateWindow.DialogResult == true)
                    Drinks.Add(drinkCreateWindow.Drink);
            });
            UpdateDrinkCommand = new RelayCommand(() =>
            {
                if (SelectedDrink != null)
                {
                    DrinkCreateOrUpdateWindow drinkUpdateWindow = new(SelectedDrink);
                    drinkUpdateWindow.ShowDialog();
                    if (drinkUpdateWindow.DialogResult == true)
                        Drinks.Update(drinkUpdateWindow.Drink);
                }
            },
            () => { return SelectedDrink != null; });
            DeleteDrinkCommand = new RelayCommand(() =>
            {
                if (SelectedDrink != null)
                {
                    Drinks.Delete(SelectedDrink.Id, "drink/id");
                }
            },
            () => { return SelectedDrink != null; });

            CreateOrderCommand = new RelayCommand(() =>
            {
                OrderCreateOrUpdateWindow orderCreateWindow = new(Pizzas, Drinks);
                orderCreateWindow.ShowDialog();
                if (orderCreateWindow.DialogResult == true)
                    Orders.Add(orderCreateWindow.Order);
            });
            UpdateOrderCommand = new RelayCommand(() =>
            {
                if (SelectedOrder != null)
                {
                    OrderCreateOrUpdateWindow orderUpdateWindow = new(SelectedOrder, Pizzas, Drinks);
                    orderUpdateWindow.ShowDialog();
                    if (orderUpdateWindow.DialogResult == true)
                        Orders.Update(orderUpdateWindow.Order);
                }
            },
            () => { return SelectedOrder != null; });
            DeleteOrderCommand = new RelayCommand(() =>
            {
                if (SelectedOrder != null)
                {
                    Orders.Delete(SelectedOrder.Id, "promoorder/id");
                }
            },
            () => { return SelectedOrder != null; });
        }
    }
}
