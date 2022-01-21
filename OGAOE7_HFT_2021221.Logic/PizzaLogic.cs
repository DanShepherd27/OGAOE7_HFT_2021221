using OGAOE7_HFT_2021221.Logic.Exceptions;
using OGAOE7_HFT_2021221.Models;
using OGAOE7_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OGAOE7_HFT_2021221.Logic
{
    public class PizzaLogic : Logic<Pizza>, IPizzaLogic
    {
        public PizzaLogic(IPizzaRepository repo) : base(repo)
        {
            this.repo = repo;
        }

        #region CRUD
        public void Delete(string name)
        {
            (this.repo as IPizzaRepository).Delete(name);
        }

        public IEnumerable<Pizza> Read(string name)
        {
            return new List<Pizza> { (this.repo as IPizzaRepository).Read(name) };
        }

        public override void Create(Pizza newItem)
        {
            if (newItem.Id < 0) throw new UnsupportedValueException(newItem.Id);
            if (newItem.Name == "") throw new Exception("Item name cannot be empty string.");
            if (newItem.Price <= 0) throw new UnsupportedValueException(newItem.Price);
            base.Create(newItem);
        }
        #endregion

        #region NON-CRUD

        public IEnumerable<string> MainData(string name)
        {
            Pizza p = this.Read(name).First();
            return new List<string> { $"[{p.Name}]\t{p.Price} HUF\t{p.Promotional}\t{p.Orders.Count}" };
        }
        public override IEnumerable<string> MainData(int id)
        {
            Pizza p = this.Read(id).First();
            return new List<string> { $"[{p.Name}]\t{p.Price} HUF\t{p.Promotional}\t{p.Orders.Count}" };
        }
        #endregion

    }
}
