using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public class PizzaLogic : Logic<Pizza>, IPizzaLogic
    {
        IPromoOrderRepository promoOrderRepository;
        public PizzaLogic(IPizzaRepository repo, IPromoOrderRepository promoOrderRepository) : base(repo)
        {
            this.repo = repo;
            this.promoOrderRepository = promoOrderRepository;
        }

        #region CRUD
        public void Delete(string name)
        {
            (base.repo as IPizzaRepository).Delete(name);
        }

        public Pizza Read(string name)
        {
            return (base.repo as IPizzaRepository).Read(name);
        }

        public void Update(Pizza pizza)
        {
            (base.repo as IPizzaRepository).Update(pizza);
        }

        #endregion

        #region NON-CRUD
        public IEnumerable<Pizza> MostPopularPizzaWithACertainDrink(Drink drink)
        {
            var q = (from pizza in ReadAll()
                     join orders in promoOrderRepository.ReadAll() on pizza.Name equals orders.PizzaName
                     where orders.DrinkName == drink.Name
                     select pizza).ToList();

            return new List<Pizza>{
                    (from pizza in q
                     group pizza by pizza into g
                     orderby g.Count()
                     select g).First().Key
            };
        }
        public IEnumerable<Tuple<Pizza, int>> PizzasStatsForToday(DateTime today)
        {
            var q = from orders in promoOrderRepository.ReadAll()
                    join pizzas in ReadAll() on orders.PizzaName equals pizzas.Name
                    where orders.TimeOfOrder.Date == today.Date
                    group pizzas by pizzas into g
                    select new Tuple<Pizza, int>(g.Key, g.Count());

            return q;
        }
        #endregion

    }
}
