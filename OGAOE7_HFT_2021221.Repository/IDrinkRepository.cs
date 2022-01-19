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
        Drink ReadByName(string name);
        void DeleteByName(string name);
        #endregion
    }
}
