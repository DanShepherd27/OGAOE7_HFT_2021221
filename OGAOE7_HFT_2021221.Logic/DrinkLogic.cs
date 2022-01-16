using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public class DrinkLogic : Logic<Drink>, IDrinkLogic
    {
        IPromoOrderRepository promoOrderRepository;
        public DrinkLogic(IDrinkRepository repo, IPromoOrderRepository promoOrderRepository) : base(repo)
        {
            this.repo = repo;
            this.promoOrderRepository = promoOrderRepository;
        }

        #region CRUD
        public void Delete(string name)
        {
            (base.repo as IDrinkRepository).Delete(name);
        }

        public Drink Read(string name)
        {
            return (base.repo as IDrinkRepository).Read(name);
        }

        public void Update(Drink drink)
        {
            (base.repo as IDrinkRepository).Update(drink);
        }
        #endregion

        #region NON-CRUD
        public int DrinkRevenueInTimePeriod(DateTime start, DateTime end)
        {
            return (from drinks in ReadAll()
                    join orders in promoOrderRepository.ReadAll() on drinks.Name equals orders.DrinkName
                    where orders.TimeOfOrder >= start && orders.TimeOfOrder <= end
                    select drinks.Price * orders.DiscountPercentage / 100).Sum();
        }

        public IEnumerable<Drink> MostPopularDrinkWithACertainPizza(Pizza pizza)
        {
            var q = (from drinks in ReadAll()
                     join orders in promoOrderRepository.ReadAll() on drinks.Name equals orders.DrinkName
                     where orders.PizzaName == pizza.Name
                     select drinks).ToList();

            return new List<Drink>{
                    (from drink in q
                    group drink by drink into g
                    orderby g.Count()
                    select g).First().Key
            };
        }
        #endregion

    }
}
