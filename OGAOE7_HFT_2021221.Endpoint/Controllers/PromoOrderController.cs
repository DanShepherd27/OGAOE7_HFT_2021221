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
    public class PromoOrderController : ControllerBase
    {
        IPromoOrderLogic pol;
        public PromoOrderController(IPromoOrderLogic pol)
        {
            this.pol = pol;
        }

        // GET: /promoorder
        [HttpGet]
        public IEnumerable<PromoOrder> Get()
        {
            return pol.ReadAll();
        }

        // GET: /promoorder/{id}
        [HttpGet("{id}")]
        public IEnumerable<PromoOrder> Get(int id)
        {
            return pol.Read(id);
        }

        // POST: /promoorder
        [HttpPost]
        public void Post([FromBody] PromoOrder value)
        {
            pol.Create(value);
        }

        // PUT: /promoorder
        [HttpPut]
        public void Put([FromBody] PromoOrder value)
        {
            pol.Update(value);
        }

        // DELETE: /promoorder
        [HttpDelete("id/{id}")]
        public void Delete(int id)
        {
            pol.Delete(id);
        }
    }
}
