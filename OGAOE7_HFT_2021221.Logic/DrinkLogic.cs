using OGAOE7_HFT_2021221.Logic.Exceptions;
using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic
{
    public class DrinkLogic : Logic<Drink>, IDrinkLogic
    {
        public DrinkLogic(IDrinkRepository repo) : base(repo)
        {
            this.repo = repo;
        }

        #region CRUD
        public IEnumerable<Drink> Read(string name)
        {
            return new List<Drink> { (this.repo as IDrinkRepository).Read(name) };
        }

        public void Delete(string name)
        {
            (this.repo as IDrinkRepository).Delete(name);
        }

        public override void Create(Drink newItem)
        {
            if (newItem.Id < 0) throw new UnsupportedValueException(newItem.Id);
            if (newItem.Name == "") throw new Exception("Item name cannot be empty string.");
            if (newItem.Price <= 0) throw new UnsupportedValueException(newItem.Price);
            base.Create(newItem);
        }
        #endregion

        #region NON-CRUD
        public override IEnumerable<string> MainData(int id)
        {
            Drink d = this.Read(id).First();
            return new List<string> { $"[{d.Name}]\t{d.Price} HUF\t{d.Promotional}\t{d.Orders.Count}" };
        }
        public IEnumerable<string> MainData(string name)
        {
            Drink d = this.Read(name).First();
            return new List<string> { $"[{d.Name}]\t{d.Price} HUF\t{d.Promotional}\t{d.Orders.Count}" };
        }
        #endregion

    }
}
