using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public class PizzaRepository : Repository<Pizza>, IPizzaRepository
    {
        public PizzaRepository(PizzaDbContext ctx) : base(ctx)
        {
            /* ... */
        }

        #region CRUD
        public override void Create(Pizza pizza)
        {
            ctx.Pizzas.Add(pizza);
            ctx.SaveChanges();
        }

        public Pizza Read(string name)
        {
            try
            {
                return ReadAll().SingleOrDefault(x => x.Name == name);
            }
            catch (ArgumentNullException)
            {
                return default(Pizza);
            }
            catch (InvalidOperationException)
            {
                return default(Pizza);
            }
        }

        public override Pizza Read(int id)
        {
            try
            {
                return ReadAll().SingleOrDefault(x => x.Id == id);
            }
            catch (ArgumentNullException)
            {
                return default(Pizza);
            }
            catch (InvalidOperationException)
            {
                return default(Pizza);
            }
        }

        public override void Update(Pizza pizza)
        {
            Pizza oldPizza = Read(pizza.Id);
            oldPizza.Price = pizza.Price;
            oldPizza.Name = pizza.Name;
            ctx.SaveChanges();
        }
        public void Delete(string name)
        {
            ctx.Pizzas.Remove(Read(name));
            ctx.SaveChanges();
        }

        public override void Delete(int id)
        {
            ctx.Pizzas.Remove(Read(id));
            ctx.SaveChanges();
        }
        #endregion
    }
}
