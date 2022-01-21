using OGAOE7_HFT_2021221.Models;
using System;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IPizzaRepository : IRepository<Pizza>
    {
        #region CRUD
        Pizza Read(string name);
        void Delete(string name);
        #endregion
    }
}
