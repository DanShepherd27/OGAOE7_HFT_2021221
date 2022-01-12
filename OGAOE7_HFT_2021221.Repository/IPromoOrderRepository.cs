using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Repository
{
    public interface IPromoOrderRepository : IRepository<PromoOrder>
    {
        #region CRUD
        PromoOrder Read(int id);
        void Update(PromoOrder promoOrder);
        void Delete(int id);
        #endregion

        #region NON-CRUD
        void ChangeDiscountPercentage(int id, int newPercentage);
        #endregion
    }
}
