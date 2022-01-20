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
        public IEnumerable<Drink> Read(string name)
        {
            return new List<Drink> { (this.repo as IDrinkRepository).Read(name) };
        }

        public void Delete(string name)
        {
            (this.repo as IDrinkRepository).Delete(name);
        }
        #endregion

        #region NON-CRUD
        public IEnumerable<int> DrinkRevenueInTimePeriod(DateTime start, DateTime end)
        {
            return new List<int>{
                (from order in promoOrderRepository.ReadAll()
                where order.TimeOfOrder >= start && order.TimeOfOrder <= end
                select order.Drink.Price * (100 - order.DiscountPercentage) / 100)
                .Sum()
            };
        }

        public override IEnumerable<string> MainData(int id)
        {
            Drink d = this.Read(id).First();
            return new List<string> { $"[{d.Name}]\t{d.Price} HUF\t{d.Promotional}\t{d.Orders.Count}" };
        }
        public IEnumerable<string> MainData(string name)
        {
            Drink d = this.Read(name).First();
            return new List<string> { $"[{d.Name}]\t{d.Price} HUF\t{d.Promotional}\t{d.Orders.Count}" };
        }
        #endregion

    }
}
