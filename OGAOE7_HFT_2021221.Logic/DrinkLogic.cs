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
        
        #endregion

    }
}
