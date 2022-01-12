using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    class Logic<T> : ILogic<T> where T: class
    {
        protected IRepository<T> repo;
        public Logic(IRepository<T> repo)
        {
            this.repo = repo;
        }

        public void Create(T newItem)
        {
            repo.Create(newItem);
        }

        public IQueryable<T> ReadAll()
        {
            return repo.ReadAll();
        }
    }
}
