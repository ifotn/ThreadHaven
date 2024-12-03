using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ThreadHaven.Data;

namespace ThreadHaven.Controllers.api.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // class level db object
        private readonly ApplicationDbContext _context;

        // constructor that accepts db dependency
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // send an HTTP 200 Ok response with list of orders from db
            return Ok(_context.Orders.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return BadRequest();  // http 400 - client didn't sent required id param
            }

            // we have an id, query db for this product
            var order = _context.Orders.Find(id);

            // does this order exist?
            if (order == null)
            {
                return NotFound(); // http 404 - resource doesn't exist
            }

            // order found - return it with 200 OK
            return Ok(order);
        }
    }
}
