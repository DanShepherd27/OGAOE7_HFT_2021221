using Microsoft.AspNetCore.Mvc;
using OGAOE7_HFT_2021221.Logic;
using OGAOE7_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OGAOE7_HFT_2021221.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        IPizzaLogic pl;
        public PizzaController(IPizzaLogic pl)
        {
            this.pl = pl;
        }
        // GET: /pizza
        [HttpGet]
        public IEnumerable<Pizza> Get()
        {
            return pl.ReadAll();
        }

        // GET: /pizza/{name}
        [HttpGet("name/{name}")]
        public IEnumerable<Pizza> Get(string name)
        {
            return pl.Read(name);
        }

        // GET: /pizza/{id}
        [HttpGet("id/{id}")]
        public IEnumerable<Pizza> Get(int id)
        {
            return pl.Read(id);
        }

        // POST: /pizza
        [HttpPost]
        public void Post([FromBody] Pizza value)
        {
            pl.Create(value);
        }

        // PUT: /pizza
        [HttpPut]
        public void Put([FromBody] Pizza value)
        {
            pl.Update(value);
        }

        // DELETE: pizza
        [HttpDelete("name/{name}")]
        public void Delete(string name)
        {
            pl.Delete(name);
        }
        
        // DELETE: pizza
        [HttpDelete("id/{id}")]
        public void Delete(int id)
        {
            pl.Delete(id);
        }
    }
}
