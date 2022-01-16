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
        #endregion
    }
}
