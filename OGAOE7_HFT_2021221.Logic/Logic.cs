using OGAOE7_HFT_2021221.Logic.Exceptions;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public abstract class Logic<T> : ILogic<T> where T : class
    {
        protected IRepository<T> repo;
        public Logic(IRepository<T> repo)
        {
            this.repo = repo;
        }

        public virtual void Create(T newItem)
        {
            repo.Create(newItem);
        }

        public IEnumerable<T> Read(int id)
        {
            if (id < 0) throw new UnsupportedValueException(id);

            return new List<T> { repo.Read(id) };
        }

        public IEnumerable<T> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(T newItem)
        {
            try
            {
                repo.Update(newItem);
            }
            catch
            {
                throw new OperationFailedException<T>((newItem) => repo.Update(newItem));
            }
        }

        public void Delete(int id)
        {
            if (id < 0) throw new UnsupportedValueException(id);
            try
            {
                repo.Delete(id);
            }
            catch
            {
                throw new OperationFailedException<int>((id) => repo.Delete(id));
            }
        }

        public abstract IEnumerable<string> MainData(int id);
    }
}
