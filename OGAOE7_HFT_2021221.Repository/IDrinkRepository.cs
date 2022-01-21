using OGAOE7_HFT_2021221.Models;
using System;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IDrinkRepository : IRepository<Drink>
    {
        #region CRUD
        Drink Read(string name);
        void Delete(string name);
        #endregion
    }
}
