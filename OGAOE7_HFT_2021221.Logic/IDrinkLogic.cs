using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public interface IDrinkLogic : ILogic<Drink>
    {
        #region CRUD
        IEnumerable<Drink> Read(string name);
        void Delete(string name);
        #endregion

        #region NON-CRUD      
        IEnumerable<string> MainData(string name);
        #endregion
    }
}
