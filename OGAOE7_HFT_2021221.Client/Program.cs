using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Repository;
using OGAOE7_HFT_2021221.Logic;
using System;
using OGAOE7_HFT_2021221.Models;
using ConsoleTools;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using BetterConsoleTables;
using System.Globalization;

namespace OGAOE7_HFT_2021221.Client
{
    class Program
    {

        static void Main(string[] args)
        {
            int discount = 20;
            Console.WriteLine("Waiting for connection with the server...");
            RestService rest = new RestService("http://localhost:26548");
            Console.Clear();
            Console.WriteLine("Connection established!");
            Console.Clear();
            MainMenu(rest, discount).Show();
        }

        private static ConsoleMenu MainMenu(RestService rest, int discount)
        {
            return new ConsoleMenu()
            .Add("Use as Admin", () => AdminMenu(rest, discount).Show())
            .Add("Use as Guest", () => GuestMenu(rest, discount).Show())
            .Add("EXIT", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.Title = "Welcome to Ristorante Italiano!\n";
                config.WriteHeaderAction = () => Console.WriteLine("Please choose one of the following options:");
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }

        private static ConsoleMenu AdminMenu(RestService rest, int discount)
        {
            return new ConsoleMenu()
            .Add("View all orders", () =>
            {
                Console.WriteLine("History of orders:");
                OrdersToConsole(rest, rest.Get<PromoOrder>("order"));
            }) //PromoOrder - ReadAll
            .Add("View menu", () => MenuToConsole(rest)) //Pizza, Drink - ReadAll
            .Add("Search for an order", () =>
            {
                Console.WriteLine("Order ID: ");
                List<PromoOrder> orders = rest.Get<PromoOrder>($"order/{Console.ReadLine()}");
                Console.WriteLine("Result:");
                OrdersToConsole(rest, orders);
            })
            .Add("Add new pizza", () => CreatePizza(rest, discount))
            .Add("Add new drink", () => CreateDrink(rest, discount))
            .Add("Update an order", () => UpdateOrder(rest, discount)) //PromoOrder - Update
            .Add("Update a pizza", () => UpdatePizza(rest, discount)) //Pizza - Update
            .Add("Update a drink", () => UpdateDrink(rest, discount)) //Drink - Update
            .Add("Delete an order", () => DeleteOrder(rest, discount)) //PromoOrder - Delete
            .Add("Delete a pizza", () => DeletePizza(rest, discount)) //Pizza - Delete
            .Add("Delete a drink", () => DeleteDrink(rest, discount)) //Drink - Delete
            .Add("Other options", () => NonCrudMenu(rest, discount).Show()) //non-crud operations
            .Add("GO BACK", () => MainMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.Title = "Login as Admin successful!\nYou can now manage the orders...";
                config.WriteHeaderAction = () => Console.WriteLine("Pick an option:");
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }

        private static ConsoleMenu NonCrudMenu(RestService rest, int discount)
        {
            return new ConsoleMenu()
                .Add("Most popular pizza with a certain drink", () =>
                {
                    Console.WriteLine("Please select only from promotional items!");
                    Console.ReadLine();
                    Drink drink = DrinkSelector(rest);
                    Pizza response = rest.Post<Drink, Pizza>(drink, "noncrud/mostpopularpizzawithacertaindrink").First();
                    Console.WriteLine($"{response.Name} is the most popular pizza with {drink.Name}");
                    Console.ReadLine();
                })
                .Add("Most popular drink with a certain pizza", () =>
                {
                    Console.WriteLine("Please select only from promotional items!");
                    Console.ReadLine();
                    Pizza pizza = PizzaSelector(rest);
                    Drink response = rest.Post<Pizza, Drink>(pizza, "noncrud/mostpopulardrinkwithacertainpizza").First();
                    Console.WriteLine($"{response.Name} is the most popular drink with {pizza.Name}");
                    Console.ReadLine();
                })
                .Add("Pizza stats for today", () =>
                {
                    Console.WriteLine("Pizza stats for today:");
                    DateTime today = DateTime.Now;
                    List<string> stats = rest.Post<DateTime, string>(today, "noncrud/pizzastatsfortoday");
                    Table table = new Table("Pizza name", "Quantity")
                    .AddRows(stats.Select(x => new string[2] { x.Split("\t")[0], x.Split("\t")[1] }));
                    Console.Write(table.ToString());
                    Console.ReadLine();

                })
                .Add("Drink revenue over a given period of time", () =>
                {
                    Console.WriteLine("Please specify a start date in YYYY:MM:DD format:");
                    string startString = Console.ReadLine();
                    DateTime start = new DateTime();
                    DateTime.TryParseExact(startString, "yyyy:MM:dd", null, DateTimeStyles.None, out start);

                    Console.WriteLine("Please specify an end date in YYYY:MM:DD format:");
                    string endString = Console.ReadLine();
                    DateTime end = new DateTime();
                    DateTime.TryParseExact(endString, "yyyy:MM:dd", null, DateTimeStyles.None, out end);
                    int revenue = rest.Get<int>($"noncrud/drinkrevenueintimeperiod/{start}/{end}").First();

                    Table table = new Table("Start date", "End date", "Revenue")
                    .AddRow(start.ToShortDateString(), end.ToShortDateString(), revenue);
                    Console.Write(table.ToString());
                    Console.ReadLine();
                })
                .Add("Most ordered combination", () =>
                {
                    string result = rest.Get<string>("noncrud/mostorderedcomboever").First();

                    Console.WriteLine("Most ordered combination:");
                    string[] combination = result.Split("\t");
                    Table table = new Table(combination[0], combination[1]);
                    Console.Write(table.ToString());
                    Console.ReadLine();
                })
                .Add("GO BACK", () => AdminMenu(rest, discount).Show())
                .Configure(config =>
                {
                    config.Selector = ">> ";
                    config.Title = "Extra features";
                    config.WriteHeaderAction = () => Console.WriteLine("Select query:");
                    config.EnableWriteTitle = true;
                    config.WriteTitleAction = title => Console.WriteLine(title);
                });
        }

        private static Drink DrinkSelector(RestService rest)
        {
            string identify = "";
            while (identify != "name" && identify != "id")
            {
                Console.WriteLine("How do you want to identify the drink? (name/id):");
                identify = Console.ReadLine();
            }
            if (identify == "name")
            {
                Console.WriteLine("Please specify the name of the drink:");
                string name = Console.ReadLine();
                return CloneObject<Drink>(rest.Get<Drink>($"drink/name/{name}").First());
            }
            else
            {
                Console.WriteLine("Please specify the id of the drink:");
                int id = int.Parse(Console.ReadLine());
                return CloneObject<Drink>(rest.Get<Drink>($"drink/id/{id}").First());
            }
        }

        private static Pizza PizzaSelector(RestService rest)
        {
            string identify = "";
            while (identify != "name" && identify != "id")
            {
                Console.WriteLine("How do you want to identify the pizza? (name/id):");
                identify = Console.ReadLine();
            }
            if (identify == "name")
            {
                Console.WriteLine("Please specify the name of the pizza:");
                string name = Console.ReadLine();
                return CloneObject<Pizza>(rest.Get<Pizza>($"pizza/name/{name}").First());
            }
            else
            {
                Console.WriteLine("Please specify the id of the pizza:");
                int id = int.Parse(Console.ReadLine());
                return CloneObject<Pizza>(rest.Get<Pizza>($"pizza/id/{id}").First());
            }
        }

        private static void DeleteOrder(RestService rest, int discount)
        {
            Console.WriteLine("Please specify the id of the order:");
            int id = int.Parse(Console.ReadLine());
            rest.Delete(id, "order/id");
            AdminMenu(rest, discount).Show();
        }
        private static void DeletePizza(RestService rest, int discount)
        {
            string identify = "";
            while (identify != "name" && identify != "id")
            {
                Console.WriteLine("How do you want to identify the pizza? (name/id):");
                identify = Console.ReadLine();
            }
            if (identify == "name")
            {
                Console.WriteLine("Please specify the name of the pizza:");
                string name = Console.ReadLine();
                rest.Delete(name, "pizza/name");
                Console.WriteLine("Item deleted successfully!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Please specify the id of the pizza:");
                int id = int.Parse(Console.ReadLine());
                rest.Delete(id, "pizza/id");
                Console.WriteLine("Item deleted successfully!");
                Console.ReadLine();
            }

            AdminMenu(rest, discount).Show();
        }

        private static void DeleteDrink(RestService rest, int discount)
        {
            string identify = "";
            while (identify != "name" && identify != "id")
            {
                Console.WriteLine("How do you want to identify the drink? (name/id):");
                identify = Console.ReadLine();
            }
            if (identify == "name")
            {
                Console.WriteLine("Please specify the name of the drink:");
                string name = Console.ReadLine();
                rest.Delete(name, "drink/name");
                Console.WriteLine("Item deleted successfully!");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Please specify the id of the drink:");
                int id = int.Parse(Console.ReadLine());
                rest.Delete(id, "drink/id");
                Console.WriteLine("Item deleted successfully!");
                Console.ReadLine();
            }

            AdminMenu(rest, discount).Show();
        }



        private static ConsoleMenu GuestMenu(RestService rest, int discount)
        {
            return new ConsoleMenu()
            .Add("Order something", () => CreateOrder(rest, discount)) //PromoOrder - Create
            .Add("GO BACK", () => MainMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.Title = $"Login as Guest successful!\nTake a look at our new offer: PIZZA + DRINK = {discount}% OFF!\n";
                config.WriteHeaderAction = () => Console.WriteLine("Make your order or take a look at the menu:");
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }

        #region CreateOrder
        private static void CreateOrder(RestService rest, int discount)
        {
            PromoOrder po = new PromoOrder();

            CreateOrder_SelectPizza(rest, po, discount).Show();

            Console.WriteLine("Total price: " + rest.Get<int>($"noncrud/totalprice/{po.Id}").First() + " HUF");

        }
        private static ConsoleMenu CreateOrder_SelectPizza(RestService rest, PromoOrder po, int discount)
        {
            //creates a menuItem for each pizza; uses ReadAll from Pizzas table
            List<Tuple<string, Action>> menuItems = new List<Tuple<string, Action>>();
            ReadPizzas(rest).Where(x => x.Promotional).ToList().ForEach(pizza =>
            {
                menuItems.Add(new Tuple<string, Action>(pizza.Name, () =>
                {
                    po.PizzaId = pizza.Id;
                    CreateOrder_SelectDrink(rest, po, discount).Show();
                }));
            });

            return new ConsoleMenu()
            .AddRange(menuItems)
            .Add("GO BACK", () => GuestMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("1 - Choose your pizza from the list:");
                config.Title = "Please specify the details of your order!";
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static ConsoleMenu CreateOrder_SelectDrink(RestService rest, PromoOrder po, int discount)
        {
            //creates a menuItem for each drink; uses ReadAll from Drinks table
            List<Tuple<string, Action>> menuItems = new List<Tuple<string, Action>>();
            ReadDrinks(rest).Where(x => x.Promotional).ToList().ForEach(drink =>
            {
                menuItems.Add(new Tuple<string, Action>(drink.Name, () =>
                {
                    po.DrinkId = drink.Id;
                    po.TimeOfOrder = DateTime.Now;
                    po.DiscountPercentage = discount;
                    CreateOrder_FinishOrder(rest, po, discount).Show();
                }));
            });

            return new ConsoleMenu()
            .AddRange(menuItems)
            .Add("GO BACK", () => CreateOrder_SelectPizza(rest, po, discount))
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("2 - Choose your drink from the list:");
                config.Title = "Please specify the details of your order!";
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static ConsoleMenu CreateOrder_FinishOrder(RestService rest, PromoOrder po, int discount)
        {
            return new ConsoleMenu()
            .Add("Finish order", () =>
            {
                rest.Post<PromoOrder>(po, "order");
                Console.WriteLine("Thank you for your order! Press enter to go back to menu...");
                Console.ReadLine();
                GuestMenu(rest, discount).Show();
            })
            .Add("Cancel order", () => GuestMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("3 - Confirm your order:");
                config.Title = "Please specify the details of your order!";
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        #endregion
        private static void CreatePizza(RestService rest, int discount)
        {
            Pizza pizza = new Pizza();
            Console.WriteLine("Name:");
            pizza.Name = Console.ReadLine();
            Console.WriteLine("Price: ");
            pizza.Price = int.Parse(Console.ReadLine());
            Console.WriteLine("PRESS ENTER TO SAVE");
            Console.ReadLine();
            rest.Post<Pizza>(pizza, "pizza");
            AdminMenu(rest, discount).Show();

        }
        private static void CreateDrink(RestService rest, int discount)
        {
            Drink drink = new Drink();
            Console.WriteLine("Name:");
            drink.Name = Console.ReadLine();
            Console.WriteLine("Price: ");
            drink.Price = int.Parse(Console.ReadLine());
            Console.WriteLine("Do you want it to be part of the promotional offer? (y/n):");
            if (Console.ReadLine() == "y")
                drink.Promotional = true;
            else if (Console.ReadLine() == "n")
                drink.Promotional = false;
            else
            {
                Console.WriteLine(">> INPUT INCORRECT! Value automatically set to false.");
                drink.Promotional = false;
            }

            Console.WriteLine("PRESS ENTER TO SAVE");
            Console.ReadLine();
            rest.Post<Drink>(drink, "pizza");
            AdminMenu(rest, discount).Show();

        }

        #region ReadOrders
        private static void OrdersToConsole(RestService rest, IEnumerable<PromoOrder> orders)
        {
            Table table = new Table("Id", "Pizza", "Drink", "Discount", "Total price", "Time of order");
            orders.ToList().ForEach(order => rest.Get<string>($"noncrud/ordermaindata/{order.Id}").ForEach(x => table.AddRow(x.Split('\t'))));
            Console.Write(table.ToString());

            Console.ReadLine();
        }
        #endregion

        #region ReadMenu (ReadPizzas, ReadDrinks)
        private static void MenuToConsole(RestService rest)
        {
            Console.WriteLine("Pizzas:");
            Table table = new Table("Name", "Price", "Promotional", "Number of orders");
            ReadPizzas(rest).ForEach(x => rest.Get<string>($"noncrud/pizzamaindata/name/{x.Name}").ForEach(x => table.AddRow(x.Split('\t'))));
            Console.Write(table.ToString());

            Console.WriteLine("\nDrinks:");
            table = new Table("Name", "Price", "Promotional", "Number of orders");
            ReadDrinks(rest).ForEach(x => rest.Get<string>($"noncrud/drinkmaindata/name/{x.Name}").ForEach(x => table.AddRow(x.Split('\t'))));
            Console.Write(table.ToString());

            Console.ReadLine();
        }
        private static List<Pizza> ReadPizzas(RestService rest)
        {
            return rest.Get<Pizza>("pizza");
        }
        private static List<Drink> ReadDrinks(RestService rest)
        {
            return rest.Get<Drink>("drink");
        }
        #endregion

        #region UpdateOrder
        private static void UpdateOrder(RestService rest, int discount)
        {
            Console.WriteLine("Please specify the order's ID:");
            int id = int.Parse(Console.ReadLine());
            PromoOrder po = CloneObject<PromoOrder>(rest.Get<PromoOrder>($"order/{id}").First());
            UpdateOrder_Menu(rest, po, discount).Show();
        }
        private static ConsoleMenu UpdateOrder_Menu(RestService rest, PromoOrder po, int discount)
        {
            return new ConsoleMenu()
            .Add("PizzaName", () => UpdateOrder_SelectPizza(rest, po, discount).Show())
            .Add("DrinkName", () => UpdateOrder_SelectDrink(rest, po, discount).Show())
            .Add("TimeOfOrder", () => UpdateOrder_ModifyTimeOfOrder(rest, po, discount).Show())
            .Add("DiscountPercentage", () =>
            {
                Console.WriteLine("Set a custom discount percentage (only for this order): ");
                po.DiscountPercentage = int.Parse(Console.ReadLine());
                Console.WriteLine($"The new discount percentage is {po.DiscountPercentage}%");
                Console.ReadLine();
            })
            .Add("SAVE", () =>
            {
                rest.Put<PromoOrder>(po, "order");
                Console.WriteLine("The order has been updated.");
                Console.ReadLine();
                AdminMenu(rest, discount).Show();
            })
            .Add("CANCEL", () => AdminMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("Select the property that you want to update:");
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static ConsoleMenu UpdateOrder_SelectPizza(RestService rest, PromoOrder po, int discount)
        {
            //creates a menuItem for each pizza; uses ReadAll from Pizzas table
            List<Tuple<string, Action>> menuItems = new List<Tuple<string, Action>>();
            ReadPizzas(rest).Where(x => x.Promotional).ToList().ForEach(pizza =>
            {
                menuItems.Add(new Tuple<string, Action>(pizza.Name, () =>
                {
                    po.PizzaId = pizza.Id;
                    UpdateOrder_Menu(rest, po, discount).Show();
                }));
            });

            return new ConsoleMenu()
            .AddRange(menuItems)
            .Add("GO BACK", () => UpdateOrder_Menu(rest, po, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("Choose a pizza from the list:");
                config.Title = "Updating order...";
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static ConsoleMenu UpdateOrder_SelectDrink(RestService rest, PromoOrder po, int discount)
        {
            //creates a menuItem for each drink; uses ReadAll from Drinks table
            List<Tuple<string, Action>> menuItems = new List<Tuple<string, Action>>();
            ReadDrinks(rest).Where(x => x.Promotional).ToList().ForEach(drink =>
              {
                  menuItems.Add(new Tuple<string, Action>(drink.Name, () =>
                  {
                      po.DrinkId = drink.Id;
                      UpdateOrder_Menu(rest, po, discount).Show();
                  }));
              });

            return new ConsoleMenu()
            .AddRange(menuItems)
            .Add("GO BACK", () => UpdateOrder_Menu(rest, po, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("Choose a drink from the list:");
                config.Title = "Updating order...";
                config.EnableWriteTitle = true;
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static ConsoleMenu UpdateOrder_ModifyTimeOfOrder(RestService rest, PromoOrder po, int discount)
        {

            return new ConsoleMenu()
                .Add("Set to current time", () => { po.TimeOfOrder = DateTime.Now; UpdateOrder_Menu(rest, po, discount).Show(); })
                .Add("Keep original time", () => UpdateOrder_Menu(rest, po, discount).Show())
                .Configure(config =>
                {
                    config.Selector = ">> ";
                    config.WriteHeaderAction = () => Console.WriteLine("Choose a time option:");
                    config.Title = "Updating order...";
                    config.EnableWriteTitle = true;
                    config.WriteTitleAction = title => Console.WriteLine(title);
                }); ;
        }
        #endregion

        #region UpdatePizza
        private static void UpdatePizza(RestService rest, int discount)
        {
            Pizza pizza = PizzaSelector(rest);
            UpdatePizza_Menu(rest, pizza, discount).Show();
        }
        private static ConsoleMenu UpdatePizza_Menu(RestService rest, Pizza pizza, int discount)
        {
            return new ConsoleMenu()
            .Add("Name", () =>
            {
                Console.WriteLine("Give a new name to the pizza:");
                pizza.Name = Console.ReadLine();
                Console.WriteLine($"The new name is \"{pizza.Name}\"");
                Console.ReadLine();
            })
            .Add("Price", () =>
            {
                Console.WriteLine("Change the price of the selected pizza: ");
                pizza.Price = int.Parse(Console.ReadLine());
                Console.WriteLine($"The new price is {pizza.Price} HUF.");
                Console.ReadLine();
            })
            .Add("SAVE", () =>
            {
                rest.Put<Pizza>(pizza, "pizza");
                Console.WriteLine("The pizza has been updated.");
                Console.ReadLine();
                AdminMenu(rest, discount).Show();
            })
            .Add("CANCEL", () => AdminMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("Select the property that you want to update:");
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        #endregion

        #region UpdateDrink
        private static void UpdateDrink(RestService rest, int discount)
        {
            Drink drink = DrinkSelector(rest);

            UpdateDrink_Menu(rest, drink, discount).Show();
        }
        private static ConsoleMenu UpdateDrink_Menu(RestService rest, Drink drink, int discount)
        {
            return new ConsoleMenu()
            .Add("Name", () =>
            {
                Console.WriteLine("Give a new name to the drink:");
                drink.Name = Console.ReadLine();
                Console.WriteLine($"The new name is \"{drink.Name}\"");
                Console.ReadLine();
            })
            .Add("Price", () =>
            {
                Console.WriteLine("Change the price of the selected drink: ");
                drink.Price = int.Parse(Console.ReadLine());
                Console.WriteLine($"The new price is {drink.Price} HUF.");
                Console.ReadLine();
            })
            .Add("Promotional", () =>
            {
                new ConsoleMenu()
                .Add("YES", () => { SetPromotional(drink, true); UpdateDrink_Menu(rest, drink, discount).Show(); })
                .Add("NO", () => { SetPromotional(drink, false); UpdateDrink_Menu(rest, drink, discount).Show(); })
                .Configure(config =>
                {
                    config.Selector = ">> ";
                    config.WriteHeaderAction = () => Console.WriteLine("Select an option:");
                    config.WriteTitleAction = title => Console.WriteLine(title);
                })
                .Show();
            })
            .Add("SAVE", () =>
            {
                rest.Put<Drink>(drink, "drink");
                Console.WriteLine("The drink has been updated.");
                Console.ReadLine();
                AdminMenu(rest, discount).Show();
            })
            .Add("CANCEL", () => AdminMenu(rest, discount).Show())
            .Configure(config =>
            {
                config.Selector = ">> ";
                config.WriteHeaderAction = () => Console.WriteLine("Select the property that you want to update:");
                config.WriteTitleAction = title => Console.WriteLine(title);
            });
        }
        private static void SetPromotional(Drink drink, bool isPromotional)
        {
            drink.Promotional = isPromotional;
        }
        #endregion

        private static T CloneObject<T>(T input) where T : class
        {
            T clone = Activator.CreateInstance(typeof(T)) as T;
            for (int i = 0; i < input.GetType().GetProperties().Length; i++)
            {
                var value = typeof(T).GetProperties()[i].GetValue(input);
                try
                {
                    typeof(T).GetProperties()[i].SetValue(clone, value);
                }
                catch
                {
                    //if property doesn't have setter => do nothing
                }

            }

            return clone;
        }
    }
}
