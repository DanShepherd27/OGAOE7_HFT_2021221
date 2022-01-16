using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public interface ILogic<T> where T : class
    {
        // Common CRUD
        IQueryable<T> ReadAll();
        void Create(T newItem);
    }
}
