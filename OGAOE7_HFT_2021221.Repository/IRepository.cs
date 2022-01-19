using System;
using System.Linq;
using OGAOE7_HFT_2021221.Models;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IRepository<T> where T : class
    {
        // Common CRUD
        void Create(T newItem);
        T Read(int id);
        IQueryable<T> ReadAll();
        void Update(T newItem);
        void Delete(int id);


    }
}
