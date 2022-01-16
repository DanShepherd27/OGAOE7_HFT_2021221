using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public class PromoOrderRepository : Repository<PromoOrder>, IPromoOrderRepository
    {
        public PromoOrderRepository(PizzaDbContext ctx) : base(ctx)
        {
            /* ... */
        }

        #region CRUD
        public override void Create(PromoOrder promoOrder)
        {
            ctx.Orders.Add(promoOrder);
            ctx.SaveChanges();
        }
        public PromoOrder Read(int id)
        {
            try
            {
                return ctx.Orders.SingleOrDefault(x => x.Id == id);
            }
            catch(ArgumentNullException)
            {
                return default(PromoOrder);
            }
            catch(InvalidOperationException)
            {
                return default(PromoOrder);
            }
        }

        public void Update(PromoOrder promoOrder)
        {
            PromoOrder oldOrder = Read(promoOrder.Id);
            oldOrder.DiscountPercentage = promoOrder.DiscountPercentage;
            oldOrder.DrinkName = promoOrder.DrinkName;
            oldOrder.PizzaName = promoOrder.PizzaName;
            oldOrder.TimeOfOrder = promoOrder.TimeOfOrder;
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.Orders.Remove(Read(id));
            ctx.SaveChanges();
        }
        #endregion
    }

}
