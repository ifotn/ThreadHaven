using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThreadHaven.Data;
using ThreadHaven.Models;

namespace ThreadHaven.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return View(await _context.Orders.OrderByDescending(o => o.OrderId).ToListAsync());
            }
            else
            {
                // if user is a customer, only fetch their own orders and no one else's
                return View(await _context.Orders
                    .Where(o => o.CustomerId == User.Identity.Name)
                    .OrderByDescending(o => o.OrderId).ToListAsync());
            }
            
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order;

            if (User.IsInRole("Administrator"))
            {
                order = await _context.Orders
                    .FirstOrDefaultAsync(m => m.OrderId == id);
            }
            else
            {
                // does the current user own the requested order?
                order = await _context.Orders
                    .FirstOrDefaultAsync(m => m.OrderId == id && m.CustomerId == User.Identity.Name);
            }
           
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
