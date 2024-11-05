using Microsoft.AspNetCore.Mvc;
using ThreadHaven.Data;
using ThreadHaven.Models;

namespace ThreadHaven.Controllers
{
    public class ShopController : Controller
    {
        // global db connection for all methods in controller
        private readonly ApplicationDbContext _context;

        // constructor to get db connection
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // get categories from db & pass to view for display
            var categories = _context.Categories.ToList();

            return View(categories);
        }

        // GET: Shop/ByCategory/123
        public IActionResult ByCategory(int id)
        {
            // ensure we have a category id
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            // get products in selected category
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();

            // get category name to show in heading and page title
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            ViewData["Category"] = category.Name;

            // now pass the product list to the view for display
            return View(products);
        }

        //// POST: /Shop/AddToCart
        //[HttpPost]
        //public IActionResult AddToCart(int Quantity, string Size, int ProductId)
        //{
        //    // find price of selected product
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == ProductId);
        //    var price = product.Price;

        //    // identify user
        //    var customerId = "SomeCustomer";


        //}
    }
}
