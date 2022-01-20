using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public class PromoOrderLogic : Logic<PromoOrder>, IPromoOrderLogic
    {
        IPizzaRepository pizzaRepository;
        IDrinkRepository drinkRepository;
        public PromoOrderLogic(IPromoOrderRepository repo, IPizzaRepository pizzaRepository, IDrinkRepository drinkRepository) : base(repo)
        {
            this.repo = repo;
            this.pizzaRepository = pizzaRepository;
            this.drinkRepository = drinkRepository;
        }

        #region CRUD
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
