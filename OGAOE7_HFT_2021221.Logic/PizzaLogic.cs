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
       
        #endregion

    }
}
