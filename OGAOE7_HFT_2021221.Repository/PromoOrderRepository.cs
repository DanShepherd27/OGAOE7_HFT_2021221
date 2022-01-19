using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public override PromoOrder Read(int id)
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

        public override IQueryable<PromoOrder> ReadAll()
        {
            var q = base.ReadAll()/*.Include(x => x.Drink).Include(x=>x.Pizza)*/; //Kell ide az include-os eager loading?
            return q;
        }
        public override void Update(PromoOrder promoOrder)
        {
            PromoOrder oldOrder = Read(promoOrder.Id);
            oldOrder.DiscountPercentage = promoOrder.DiscountPercentage;
            oldOrder.DrinkId = promoOrder.DrinkId;
            oldOrder.PizzaId = promoOrder.PizzaId;
            oldOrder.TimeOfOrder = promoOrder.TimeOfOrder;
            ctx.SaveChanges();
        }

        public override void Delete(int id)
        {
            ctx.Orders.Remove(Read(id));
            ctx.SaveChanges();
        }
        #endregion

    }

}
