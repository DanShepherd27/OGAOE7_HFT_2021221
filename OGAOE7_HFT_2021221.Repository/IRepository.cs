using System;
using System.Linq;
using OGAOE7_HFT_2021221.Models;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IRepository<T> where T : class
    {
        // Common CRUD
        IQueryable<T> ReadAll();
        void Create(T newItem);
    }
}
