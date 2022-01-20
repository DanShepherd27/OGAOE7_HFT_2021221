using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OGAOE7_HFT_2021221.Logic;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NonCrudController : ControllerBase
    {
        IPromoOrderLogic pol;
        IDrinkLogic dl;
        IPizzaLogic pl;
        public NonCrudController(IPromoOrderLogic pol, IDrinkLogic dl, IPizzaLogic pl)
        {
            this.pol = pol;
            this.dl = dl;
            this.pl = pl;
        }

        #region MULTI-TABLE NON-CRUDS

        //POST: noncrud/mostpopularpizzawithsacertaindrink
        [HttpPost]
        public IEnumerable<Pizza> MostPopularPizzaWithACertainDrink([FromBody] Drink drink)
        {
            return pol.MostPopularPizzaWithACertainDrink(drink);
        }

        //POST: noncrud/mostpopulardrinkwithacertainpizza
        [HttpPost]
        public IEnumerable<Drink> MostPopularDrinkWithACertainPizza([FromBody] Pizza pizza)
        {
            return pol.MostPopularDrinkWithACertainPizza(pizza);
        }

        //POST: noncrud/pizzastatsfortoday
        [HttpPost]
        public IEnumerable<string> PizzaStatsForToday([FromBody] DateTime today)
        {
            return pl.PizzaStatsForToday(today);
        }

        //GET: noncrud/drinkrevenueintimeperiod/{start}/{end}
        [HttpGet("{start}/{end}")]
        public IEnumerable<int> DrinkRevenueInTimePeriod(DateTime start, DateTime end)
        {
            return dl.DrinkRevenueInTimePeriod(start, end);
        }

        //GET: noncrud/mostorderedcomboever
        [HttpGet]
        public IEnumerable<string> MostOrderedComboEver()
        {
            return pol.MostOrderedComboEver();
        }
        #endregion

        #region SINGLE-TABLE NON-CRUDS

        //GET: noncrud/totalprice/{id}
        [HttpGet("{id}")]
        public IEnumerable<int> TotalPrice(int id)
        {
            return pol.TotalPrice(id);
        }

        //GET: noncrud/ordermaindata/id/{id}
        [HttpGet("{id}")]
        public IEnumerable<string> OrderMainData(int id)
        {
            return pol.MainData(id);
        }

        //GET: noncrud/pizzamaindata/name/{name}
        [HttpGet("name/{name}")]
        public IEnumerable<string> PizzaMainData(string name)
        {
            return pl.MainData(name);
        }
        
        //GET: noncrud/pizzamaindata/id/{id}
        [HttpGet("id/{id}")]
        public IEnumerable<string> PizzaMainData(int id)
        {
            return pl.MainData(id);
        }

        //GET: noncrud/drinkmaindata/name{name}
        [HttpGet("name/{name}")]
        public IEnumerable<string> DrinkMainData(string name)
        {
            return dl.MainData(name);
        }
        
        //GET: noncrud/drinkmaindata/id/{id}
        [HttpGet("id/{id}")]
        public IEnumerable<string> DrinkMainData(int id)
        {
            return dl.MainData(id);
        }
        #endregion

        [HttpGet]
        public IEnumerable<DateTime> CurrentTime()
        {
            DateTime now = DateTime.Now;
            return new List<DateTime> { now };
        }
    }
}

