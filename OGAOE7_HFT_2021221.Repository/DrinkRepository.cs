using Microsoft.EntityFrameworkCore;
using OGAOE7_HFT_2021221.Data;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public class DrinkRepository : Repository<Drink>, IDrinkRepository
    {
        public DrinkRepository(PizzaDbContext ctx) : base(ctx)
        {
            /* ... */
        }

        #region CRUD
        public override void Create(Drink drink)
        {
            ctx.Drinks.Add(drink);
            ctx.SaveChanges();
        }

        public Drink Read(string name)
        {
            try
            {
                return ReadAll().SingleOrDefault(x => x.Name == name);
            }
            catch (ArgumentNullException)
            {
                return default(Drink);
            }
            catch (InvalidOperationException)
            {
                return default(Drink);
            }
        }
        public override Drink Read(int id)
        {
            try
            {
                return ReadAll().SingleOrDefault(x => x.Id == id);
            }
            catch (ArgumentNullException)
            {
                return default(Drink);
            }
            catch (InvalidOperationException)
            {
                return default(Drink);
            }
        }

        public override void Update(Drink drink)
        {
            Drink oldDrink = Read(drink.Id);
            oldDrink.Price = drink.Price;
            oldDrink.Promotional = drink.Promotional;
            oldDrink.Name = drink.Name;
            ctx.SaveChanges();
        }

        public void Delete(string name)
        {
            ctx.Drinks.Remove(Read(name));
            ctx.SaveChanges();
        }

        public override void Delete(int id)
        {
            ctx.Drinks.Remove(Read(id));
            ctx.SaveChanges();
        }        
        #endregion
    }
}


