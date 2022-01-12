﻿using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IPizzaRepository : IRepository<Pizza>
    {
        #region CRUD
        Pizza Read(string name);
        void Update(Pizza pizza);
        void Delete(string name);
        #endregion

        #region NON-CRUD
        void ChangePizzaName(string pizzaName, string newName);
        #endregion
    }
}