using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public interface IPromoOrderLogic : ILogic<PromoOrder>
    {
        #region CRUD
        #endregion

        #region NON-CRUD
        public IEnumerable<Pizza> MostPopularPizzaWithACertainDrink(Drink drink);
        public IEnumerable<Drink> MostPopularDrinkWithACertainPizza(Pizza pizza);
        public IEnumerable<string> PizzaStatsForToday(DateTime today);
        public IEnumerable<int> DrinkRevenueInTimePeriod(DateTime start, DateTime end);
        public IEnumerable<string> MostOrderedComboEver();
        IEnumerable<int> TotalPrice(int id);
        #endregion
    }
}
