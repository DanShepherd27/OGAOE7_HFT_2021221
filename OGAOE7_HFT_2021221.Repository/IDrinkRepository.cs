﻿using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IDrinkRepository : IRepository<Drink>
    {
        #region CRUD
        Drink Read(string name);
        void Update(Drink drink);
        void Delete(string name);
        #endregion

        #region NON-CRUD
        void ChangeDrinkName(string drinkName, string newName);
        #endregion
    }
}
