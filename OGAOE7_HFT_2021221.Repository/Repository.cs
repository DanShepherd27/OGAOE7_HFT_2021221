using OGAOE7_HFT_2021221.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected PizzaDbContext ctx;

        public Repository(PizzaDbContext ctx)
        {
            this.ctx = ctx;
        }

        public abstract void Create(T newItem);

        public IQueryable<T> ReadAll()
        {
            return this.ctx.Set<T>(); //"Set" means dataset here, not setter
        }
    }
}
