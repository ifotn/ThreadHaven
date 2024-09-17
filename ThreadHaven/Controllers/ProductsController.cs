using Microsoft.AspNetCore.Mvc;

namespace ThreadHaven.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
