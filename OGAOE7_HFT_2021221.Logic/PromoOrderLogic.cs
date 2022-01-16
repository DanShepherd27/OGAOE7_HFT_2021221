using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
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
        public void Delete(int id)
        {
            (base.repo as IPromoOrderRepository).Delete(id);
        }

        public PromoOrder Read(int id)
        {
            return (base.repo as IPromoOrderRepository).Read(id);
        }

        public void Update(PromoOrder promoOrder)
        {
            (base.repo as IPromoOrderRepository).Update(promoOrder);
        }
        #endregion

        #region NON-CRUD
        public IEnumerable<PromoOrder> MostOrderedComboEver()
        {
            var q = from order in ReadAll()
                    join pizza in pizzaRepository.ReadAll() on order.PizzaName equals pizza.Name
                    join drink in drinkRepository.ReadAll() on order.DrinkName equals drink.Name
                    select new { Pizza = pizza, Drink = drink };
            var q2 = from record in q
                     group record by new
                     {
                         PName = record.Pizza.Name,
                         DName = record.Drink.Name
                     } into g
                     orderby g.Count() descending
                     select new PromoOrder
                     {
                         PizzaName = g.Key.PName,
                         DrinkName = g.Key.DName,
                         Drink = new Drink(),
                         Pizza = new Pizza()
                     };
            var q3 = q2.FirstOrDefault();
            q3.Drink = (from drink in drinkRepository.ReadAll()
                        where drink.Name == q3.DrinkName
                        select drink).FirstOrDefault();
            q3.Pizza = (from pizza in pizzaRepository.ReadAll()
                        where pizza.Name == q3.PizzaName
                        select pizza).FirstOrDefault();
            return new List<PromoOrder> { q3 };
        }
        #endregion
    }
}
