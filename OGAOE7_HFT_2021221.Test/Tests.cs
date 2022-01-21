using NUnit.Framework;
using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using OGAOE7_HFT_2021221.Logic;
using OGAOE7_HFT_2021221.Repository;
using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Logic.Exceptions;

namespace OGAOE7_HFT_2021221.Test
{
    [TestFixture]
    public class Tests
    {
        private PromoOrderLogic pol { get; set; }
        private PizzaLogic pl { get; set; }
        private DrinkLogic dl { get; set; }

        Mock<IPromoOrderRepository> promoOrderRepoMock;
        Mock<IPizzaRepository> pizzaRepoMock;
        Mock<IDrinkRepository> drinkRepoMock;
        static List<(DateTime start, DateTime end)> startEndTimes = new()
        {
            (new DateTime(2022, 01, 08), new DateTime(2022, 01, 10)), //normal case
            (new DateTime(2022, 01, 08), new DateTime(2022, 01, 09)), //end corner case
            (new DateTime(2022, 01, 09), new DateTime(2022, 01, 10)), //start corner case
            (new DateTime(2022, 01, 09), new DateTime(2022, 01, 09)), //both start & end corner case

        };
        static DateTime Date(int y, int m, int d)
        {
            return new DateTime(y, m, d);
        }

        [SetUp]
        public void Setup()
        {
            promoOrderRepoMock = new Mock<IPromoOrderRepository>();
            pizzaRepoMock = new Mock<IPizzaRepository>();
            drinkRepoMock = new Mock<IDrinkRepository>();


            promoOrderRepoMock.Setup(repo => repo.ReadAll()).Returns(() => FakeOrders());
            drinkRepoMock.Setup(repo => repo.ReadAll()).Returns(() => FakeDrink());
            drinkRepoMock.Setup(repo => repo.Read(It.IsAny<int>())).Returns((int id) => FakeDrink().First(drink => drink.Id == id));
            pizzaRepoMock.Setup(repo => repo.Read(It.IsAny<int>())).Returns((int id) => FakePizza().First(pizza => pizza.Id == id));
            drinkRepoMock.Setup(repo => repo.Create(It.IsAny<Drink>())).Callback(() => FakeDrink().First());
            pizzaRepoMock.Setup(repo => repo.Create(It.IsAny<Pizza>())).Callback(() => FakePizza().First());
            promoOrderRepoMock.Setup(repo => repo.Create(It.IsAny<PromoOrder>())).Callback(() => FakeOrders().First());
            pizzaRepoMock.Setup(repo => repo.Delete(It.IsAny<int>())).Verifiable();
            
            

            this.pol = new PromoOrderLogic(promoOrderRepoMock.Object);
            this.pl = new PizzaLogic(pizzaRepoMock.Object);
            this.dl = new DrinkLogic(drinkRepoMock.Object);



        }

        #region Testing Non-CRUDs
        [Test]
        public void MostPopularPizzaWithACertainDrink_Test()
        {
            //ARRANGE
            Drink drink = drinkRepoMock.Object.Read(1);

            //ACT
            Pizza mostPopularPizzaForDrink = pol.MostPopularPizzaWithACertainDrink(drink).First();

            //ASSERT
            Assert.That(mostPopularPizzaForDrink.Id, Is.EqualTo(2));
        }

        [Test]
        public void MostPopularDrinkWithACertainPizza_Test()
        {
            //ARRANGE
            Pizza pizza = pizzaRepoMock.Object.Read(3); //Fattoria

            //ACT
            Drink mostPopularDrinkForPizza = pol.MostPopularDrinkWithACertainPizza(pizza).First(); //Dr. Pepper

            //ASSERT
            Assert.That(mostPopularDrinkForPizza.Id, Is.EqualTo(4));
        }

        [Test]
        public void PizzaStatsForToday_Test()
        {
            //ARRANGE
            DateTime fakeToday = new DateTime(2022, 01, 09);
            IEnumerable<string> expected = new List<string> { "Carbonara\t4", "Fattoria\t2" };

            //ACT
            IEnumerable<string> stats = pol.PizzaStatsForToday(fakeToday);

            //ASSERT
            Assert.That(stats, Is.EqualTo(expected));
        }

        //If only date is specified, time is 00:00:00
        [TestCase("2022-01-08", "2022-01-10", 1872)]
        [TestCase("2022-01-08", "2022-01-09", 0)]
        [TestCase("2022-01-09", "2022-01-10", 1872)]
        [TestCase("2022-01-09", "2022-01-09", 0)]
        [TestCase("2022-01-10", "2022-01-11", 0)]
        public void DrinkRevenueInTimePeriod_Test(DateTime start, DateTime end, int expectedRevenue)
        {
            //ARRANGE

            //ACT
            int revenue = pol.DrinkRevenueInTimePeriod(start, end).First();

            //ASSERT
            Assert.That(revenue, Is.EqualTo(expectedRevenue));
        }

        [Test]
        public void MostOrderedComboEver_Test()
        {
            //ARRANGE

            //ACT
            string top = pol.MostOrderedComboEver().First();

            //ASSERT
            Assert.That(top, Is.EqualTo("Fattoria\tDr. Pepper"));

        }
        #endregion

        #region Testing CRUDs
        [Test]
        public void CreateDrink_Test()
        {
            //ARRANGE
            Drink drink = new Drink { Name = "Cafe latte", Price = 350, Promotional = true };

            //ACT
            dl.Create(drink);

            //ASSERT
            drinkRepoMock.Verify(x => x.Create(It.IsAny<Drink>()), Times.Once);
        }

        [Test]
        public void CreateDrink_ExceptionTest()
        {
            //ARRANGE
            Drink drink = new Drink() { Id = -1 };

            //ACT


            //ASSERT
            Assert.Throws<UnsupportedValueException>(() => dl.Create(drink));
        }

        [Test]
        public void CreatePizza_Test()
        {
            //ARRANGE
            Pizza pizza = new Pizza() { Id = 5, Price = 2300, Name = "Songoku" };

            //ACT
            pl.Create(pizza);

            //ASSERT
            pizzaRepoMock.Verify(x => x.Create(pizza), Times.Once);
        }

        [Test]
        public void CreatePizza_ExceptionTest()
        {
            //ARRANGE
            Pizza pizza = new Pizza() { Id = 1, Price = -2 };

            //ACT

            //ASSERT
            Assert.Throws<UnsupportedValueException>(() => pl.Create(pizza));
        }

        [Test]
        public void CreatePromoOrder_Test()
        {
            //ARRANGE
            PromoOrder po = new PromoOrder() { Id = 10, DiscountPercentage = 18, DrinkId = 3, PizzaId = 1, TimeOfOrder = DateTime.Now };

            //ACT
            pol.Create(po);

            //ASSERT
            promoOrderRepoMock.Verify(x => x.Create(po), Times.Once);
        }

        [Test]
        public void CreatePromoOrder_ExceptionTest()
        {
            //ARRANGE
            PromoOrder po = new PromoOrder() { Id = -1 };

            //ACT

            //ASSERT
            Assert.Throws<UnsupportedValueException>(() => pol.Create(po));
        }

        [Test]
        public void ReadAllDrink_Test()
        {
            //ARRANGE
            IEnumerable<Drink> expectedDrinks = FakeDrink();
            //ACT
            IEnumerable<Drink> drinks = dl.ReadAll();

            //ASSERT
            Assert.That(drinks, Is.EqualTo(expectedDrinks));
        }

        [Test]
        public void DeletePizzaById_ExceptionTest()
        {
            //ARRANGE
            int id = -1; //this Id doesn't exist in database
            //ACT

            //ASSERT
            Assert.Throws<UnsupportedValueException>(() => pl.Delete(id));
        }
        #endregion

        private IQueryable<PromoOrder> FakeOrders()
        {
            var items = new List<PromoOrder>{
                new PromoOrder()
                {
                    Id = 1,
                    DrinkId = 1,
                    PizzaId = 2,
                    Drink = FakeDrink().ToArray()[0],
                    Pizza = FakePizza().ToArray()[1],
                    TimeOfOrder = new DateTime(2022, 01, 09, 20, 30, 35), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 2,
                    DrinkId = 1,
                    PizzaId = 2,
                    Drink = FakeDrink().ToArray()[0],
                    Pizza = FakePizza().ToArray()[1],
                    TimeOfOrder = new DateTime(2022, 01, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 3,
                    DrinkId = 2,
                    PizzaId = 3,
                    Drink = FakeDrink().ToArray()[1],
                    Pizza = FakePizza().ToArray()[2],
                    TimeOfOrder = new DateTime(2022, 01, 09, 21, 59, 05), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 4,
                    DrinkId = 2,
                    PizzaId = 2,
                    Drink = FakeDrink().ToArray()[1],
                    Pizza = FakePizza().ToArray()[1],
                    TimeOfOrder = new DateTime(2022, 01, 09, 23, 15, 12), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },
                new PromoOrder()
                {
                    Id = 5,
                    DrinkId = 3,
                    PizzaId = 2,
                    Drink = FakeDrink().ToArray()[2],
                    Pizza = FakePizza().ToArray()[1],
                    TimeOfOrder = new DateTime(2022, 01, 09, 23, 25, 43), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },

                new PromoOrder()
                {
                    Id = 6,
                    DrinkId = 4,
                    PizzaId = 3,
                    Drink = FakeDrink().ToArray()[3],
                    Pizza = FakePizza().ToArray()[2],
                    TimeOfOrder = new DateTime(2022, 01, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },
                new PromoOrder()
                {
                    Id = 7,
                    DrinkId = 4,
                    PizzaId = 3,
                    Drink = FakeDrink().ToArray()[3],
                    Pizza = FakePizza().ToArray()[2],
                    TimeOfOrder = new DateTime(2022, 02, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },new PromoOrder()
                {
                    Id = 8,
                    DrinkId = 4,
                    PizzaId = 3,
                    Drink = FakeDrink().ToArray()[3],
                    Pizza = FakePizza().ToArray()[2],
                    TimeOfOrder = new DateTime(2022, 03, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss
                    DiscountPercentage = 20
                },new PromoOrder()
                {
                    Id = 9,
                    DrinkId = 4,
                    PizzaId = 3,
                    Drink = FakeDrink().ToArray()[3],
                    Pizza = FakePizza().ToArray()[2],
                    TimeOfOrder = new DateTime(2022, 04, 09, 20, 32, 01), // YYYY, MM, DD, hh, mm, ss

                }
            };
            return items.AsQueryable();
        }

        private IQueryable<Drink> FakeDrink()
        {
            return new List<Drink>()
            {
                new Drink() { Id=1,Name = "Coca Cola", Price = 390, Promotional = true },
                new Drink() { Id=2, Name = "Fanta Orange", Price = 390, Promotional = true },
                new Drink() { Id=3,Name = "Sprite", Price = 390, Promotional = true },
                new Drink() { Id=4, Name = "Dr. Pepper", Price = 390, Promotional = true },
                new Drink() { Id=5, Name = "Cafe macchiato", Price = 460, Promotional = false }
            }.AsQueryable();
        }

        private IQueryable<Pizza> FakePizza()
        {
            return new List<Pizza>()
            {
                new Pizza() { Id=1, Name = "Margherita", Price = 2100 },
                new Pizza() { Id=2, Name = "Carbonara", Price = 2200 },
                new Pizza() { Id=3, Name = "Fattoria", Price = 2300 },
                new Pizza() { Id=4, Name = "Prosciutto e Funghi", Price = 2400 }
            }.AsQueryable();
        }

    }
}
