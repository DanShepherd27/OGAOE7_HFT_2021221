using OGAOE7_HFT_2021221.Data;
using System.Linq;

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

        public abstract T Read(int id);

        public virtual IQueryable<T> ReadAll()
        {
            return this.ctx.Set<T>(); //"Set" means dataset here, not setter
        }

        public abstract void Update(T newItem);

        public abstract void Delete(int id);
    }
}
