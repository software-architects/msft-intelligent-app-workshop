using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Erp.WebApi
{
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private ErpContext context;

        public OrderController(ErpContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await context.Orders.ToArrayAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            return StatusCode(201); 
        }
    }
}
