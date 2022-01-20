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
            (this.repo as IPizzaRepository).Delete(name);
        }

        public IEnumerable<Pizza> Read(string name)
        {
            return new List<Pizza> { (this.repo as IPizzaRepository).Read(name) };
        }

        #endregion

        #region NON-CRUD
        /// <summary>
        /// This method shows how many pizzas got sold from each type on a certain date.
        /// </summary>
        /// <param name="today">Today's date. The time component can be anything. </param>
        /// <returns>Returns a string for each pizza type. Each string looks like this: PizzaName + "\t" + Quantity</returns>
        public IEnumerable<string> PizzaStatsForToday(DateTime today)
        {
            return from order in promoOrderRepository.ReadAll().ToList()
                   where order.TimeOfOrder.Date == today.Date
                   group order by order.Pizza into g
                   select g.First().Pizza.Name + "\t" + g.Count();
        }

        public IEnumerable<string> MainData(string name)
        {
            Pizza p = this.Read(name).First();
            return new List<string> { $"[{p.Name}]\t{p.Price} HUF\t{p.Promotional}\t{p.Orders.Count}" };
        }
        public override IEnumerable<string> MainData(int id)
        {
            Pizza p = this.Read(id).First();
            return new List<string> { $"[{p.Name}]\t{p.Price} HUF\t{p.Promotional}\t{p.Orders.Count}" };
        }
        #endregion

    }
}
