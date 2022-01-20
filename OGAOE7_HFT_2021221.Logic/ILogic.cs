using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public interface ILogic<T> where T : class
    {
        #region Common CRUD
        void Create(T newItem);
        IEnumerable<T> Read(int id);
        IEnumerable<T> ReadAll();
        void Update(T newItem);
        void Delete(int id);
        #endregion

        #region Non-CRUD
        IEnumerable<string> MainData(int id);
        #endregion
    }
}
