using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OGAOE7_HFT_2021221.Logic;
using OGAOE7_HFT_2021221.Logic.Exceptions;
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
    public class DrinkController : ControllerBase
    {
        IDrinkLogic dl;
        private readonly IHubContext<SignalRHub> hub;
        public DrinkController(IDrinkLogic dl, IHubContext<SignalRHub> hub)
        {
            this.dl = dl;
            this.hub = hub;
        }

        // GET: /drink
        [HttpGet]
        public IEnumerable<Drink> Get()
        {
            return dl.ReadAll();
        }

        // GET: drink/name/{name}
        [HttpGet("name/{name}")]
        public IEnumerable<Drink> Get(string name)
        {
            return dl.Read(name);
        }

        // GET: drink/id/{id}
        [HttpGet("id/{id}")]
        public IEnumerable<Drink> Get(int id)
        {
                return dl.Read(id);
        }

        // POST: /drink
        [HttpPost]
        public void Post([FromBody] Drink value)
        {
            dl.Create(value);
            hub.Clients.All.SendAsync("DrinkCreated", value);
        }

        // PUT: /drink
        [HttpPut]
        public void Put([FromBody] Drink value)
        {
            dl.Update(value);
            hub.Clients.All.SendAsync("DrinkUpdated", value);
        }

        // DELETE: /drink/name
        [HttpDelete("name/{name}")]
        public void Delete(string name)
        {
            var drinkToDelete = this.dl.Read(name);
            dl.Delete(name);
            hub.Clients.All.SendAsync("DrinkDeleted", drinkToDelete);
        }
        
        // DELETE: /drink/id
        [HttpDelete("id/{id}")]
        public void Delete(int id)
        {
            var drinkToDelete = this.dl.Read(id);
            dl.Delete(id);
            hub.Clients.All.SendAsync("DrinkDeleted", drinkToDelete);
        }
    }
}
