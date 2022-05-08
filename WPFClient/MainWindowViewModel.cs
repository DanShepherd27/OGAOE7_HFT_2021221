using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OGAOE7_HFT_2021221.WPFClient
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private Pizza selectedPizza;
        private Drink selectedDrink;
        private PromoOrder selectedOrder;

        public RestCollection<Pizza> Pizzas { get; set; }
        public RestCollection<Drink> Drinks { get; set; }
        public RestCollection<PromoOrder> Orders { get; set; }


        public Pizza SelectedPizza
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
                    OnPropertyChanged(); //egyelőre ez nem javította meg az updatet
                    (UpdatePizzaCommand as RelayCommand).NotifyCanExecuteChanged();
                    (DeletePizzaCommand as RelayCommand).NotifyCanExecuteChanged();
                }

            }
        }
        public Drink SelectedDrink
        {
            get => selectedDrink;
            set
            {
                SetProperty(ref selectedDrink, value);
                (UpdateDrinkCommand as RelayCommand).NotifyCanExecuteChanged();
                (DeleteDrinkCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        public PromoOrder SelectedOrder
        {
            get => selectedOrder;
            set
            {
                SetProperty(ref selectedOrder, value);
                (UpdateOrderCommand as RelayCommand).NotifyCanExecuteChanged();
                (DeleteOrderCommand as RelayCommand).NotifyCanExecuteChanged();
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

        public MainWindowViewModel()
        {
            Pizzas = new RestCollection<Pizza>("http://localhost:26548/", "pizza", "hub");
            Drinks = new RestCollection<Drink>("http://localhost:26548/", "drink", "hub");
            Orders = new RestCollection<PromoOrder>("http://localhost:26548/", "promoorder", "hub");

            CreatePizzaCommand = new RelayCommand(() =>
            {
                PizzaCreateOrUpdateWindow pizzaCreateWindow = new ();
                pizzaCreateWindow.ShowDialog();
                Pizzas.Add(pizzaCreateWindow.Pizza);
            });
            UpdatePizzaCommand = new RelayCommand(() =>
            {
                PizzaCreateOrUpdateWindow pizzaUpdateWindow = new(SelectedPizza);
                pizzaUpdateWindow.ShowDialog();
                Pizzas.Update(pizzaUpdateWindow.Pizza);
            },
            () => { return SelectedPizza != null; });
            DeletePizzaCommand = new RelayCommand(() =>
            {
                Pizzas.Delete(SelectedPizza.Id);
            },
            () => { return SelectedPizza != null; });

            CreateDrinkCommand = new RelayCommand(() =>
            {
                DrinkCreateOrUpdateWindow drinkCreateWindow = new();
                drinkCreateWindow.ShowDialog();
                Drinks.Add(drinkCreateWindow.Drink);
            });
            UpdateDrinkCommand = new RelayCommand(() =>
            {
                DrinkCreateOrUpdateWindow drinkUpdateWindow = new(SelectedDrink);
                drinkUpdateWindow.ShowDialog();
                Drinks.Update(drinkUpdateWindow.Drink);
            },
            () => { return SelectedDrink != null; });
            DeleteDrinkCommand = new RelayCommand(() =>
            {
                Drinks.Delete(SelectedDrink.Id);
            },
            () => { return SelectedDrink != null; });

            CreateOrderCommand = new RelayCommand(() =>
            {
                OrderCreateOrUpdateWindow orderCreateWindow = new();
                orderCreateWindow.ShowDialog();
                Orders.Add(orderCreateWindow.Order);
            });
            UpdateOrderCommand = new RelayCommand(() =>
            {
                OrderCreateOrUpdateWindow orderUpdateWindow = new(SelectedOrder);
                orderUpdateWindow.ShowDialog();
                Orders.Update(orderUpdateWindow.Order);
            },
            () => { return SelectedOrder != null; });
            DeleteOrderCommand = new RelayCommand(() =>
            {
                Orders.Delete(SelectedOrder.Id);
            },
            () => { return SelectedOrder != null; });
        }




    }
}
