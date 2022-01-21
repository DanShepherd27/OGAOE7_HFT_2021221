using OGAOE7_HFT_2021221.Logic.Exceptions;
using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OGAOE7_HFT_2021221.Logic
{
    public class PromoOrderLogic : Logic<PromoOrder>, IPromoOrderLogic
    {
        public PromoOrderLogic(IPromoOrderRepository repo) : base(repo)
        {
            this.repo = repo;
        }

        #region CRUD
        public override void Create(PromoOrder newItem)
        {
            if (newItem.DiscountPercentage < 0) throw new UnsupportedValueException(newItem.Id);
            if (newItem.PizzaId <= 0) throw new UnsupportedValueException(newItem.Id);
            if (newItem.DrinkId <= 0) throw new UnsupportedValueException(newItem.Id);
            base.Create(newItem);
        }
        #endregion

        #region NON-CRUD
        public IEnumerable<string> MostOrderedComboEver()
        {
            PromoOrder topCombo = this.ReadAll().GroupBy(x => (x.Pizza.Name, x.Drink.Name)).OrderBy(g => g.Count()).Reverse().Select(g => g.First()).FirstOrDefault();
            return new List<string> { topCombo.Pizza.Name + "\t" + topCombo.Drink.Name };
        }

        public IEnumerable<Pizza> MostPopularPizzaWithACertainDrink(Drink drink)
        {
            var q = this.ReadAll().Where(x => x.Drink.Id == drink.Id);

            return new List<Pizza>{
                    (from pizza in q.Select(o => o.Pizza)
                     group pizza by pizza.Id into g
                     orderby g.Count() descending
                     select g.First()).FirstOrDefault()
            };
        }
        public IEnumerable<Drink> MostPopularDrinkWithACertainPizza(Pizza pizza)
        {
            var q = this.ReadAll().Where(x => x.Pizza.Id == pizza.Id);

            return new List<Drink> {
                (from drink in q.Select(o => o.Drink)
                 group drink by drink.Id into g
                 orderby g.Count() descending
                 select g.First()).FirstOrDefault()
            };
        }

        /// <summary>
        /// This method shows how many pizzas got sold from each type on a certain date.
        /// </summary>
        /// <param name="today">The requested date. The time component can be anything. For today's date, call with DateTime.Now</param>
        /// <returns>Returns a string for each pizza type. Each string looks like this: PizzaName + "\t" + Quantity</returns>
        public IEnumerable<string> PizzaStatsForToday(DateTime today)
        {
            return from order in ReadAll().ToList()
                   where order.TimeOfOrder.Date == today.Date
                   group order by order.Pizza.Id into g
                   orderby g.Count() descending
                   select g.First().Pizza.Name + "\t" + g.Count();
        }
        public IEnumerable<int> DrinkRevenueInTimePeriod(DateTime start, DateTime end)
        {
            return new List<int>{
                (from order in this.ReadAll()
                where order.TimeOfOrder >= start && order.TimeOfOrder <= end
                select order.Drink.Price * (100 - order.DiscountPercentage) / 100)
                .Sum()
            };
        }
        public override IEnumerable<string> MainData(int id)
        {
            PromoOrder po = this.Read(id).First();
            return new List<string> { $"[{id}]\t{po.Pizza.Name} ({po.Pizza.Price} HUF)\t{po.Drink.Name} ({po.Drink.Price} HUF)\t{po.DiscountPercentage}%\t{this.TotalPrice(id).First()} HUF\t{po.TimeOfOrder.ToString("G", CultureInfo.CurrentCulture)}" };
        }

        public IEnumerable<int> TotalPrice(int id)
        {
            PromoOrder po = this.Read(id).First();
            return new List<int> { (po.Pizza.Price + po.Drink.Price) - (po.Pizza.Price + po.Drink.Price) * po.DiscountPercentage / 100 };
        }
        #endregion
    }
}
