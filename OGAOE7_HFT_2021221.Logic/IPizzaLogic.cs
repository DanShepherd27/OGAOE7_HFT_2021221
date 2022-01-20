using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public interface IPizzaLogic : ILogic<Pizza>
    {
        #region CRUD
        IEnumerable<Pizza> Read(string name);
        void Delete(string name);
        #endregion

        #region NON-CRUD
        public IEnumerable<string> PizzaStatsForToday(DateTime today);
        IEnumerable<string> MainData(string name);
        #endregion
    }
}
