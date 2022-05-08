using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<SignalRHub> hub;
        public PizzaController(IPizzaLogic pl, IHubContext<SignalRHub> hub)
        {
            this.pl = pl;
            this.hub = hub;
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
            hub.Clients.All.SendAsync("PizzaCreated", value);
        }

        // PUT: /pizza
        [HttpPut]
        public void Put([FromBody] Pizza value)
        {
            pl.Update(value);
            hub.Clients.All.SendAsync("PizzaUpdated", value);
        }

        // DELETE: pizza
        [HttpDelete("name/{name}")]
        public void Delete(string name)
        {
            var pizzaToDelete = this.pl.Read(name);
            pl.Delete(name);
            hub.Clients.All.SendAsync("PizzaDeleted", pizzaToDelete);
        }
        
        // DELETE: pizza
        [HttpDelete("id/{id}")]
        public void Delete(int id)
        {
            var pizzaToDelete = this.pl.Read(id);
            pl.Delete(id);
            hub.Clients.All.SendAsync("PizzaDeleted", pizzaToDelete);
        }
    }
}
