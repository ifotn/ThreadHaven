using Microsoft.AspNetCore.Mvc;
using ThreadHaven.Models;

namespace ThreadHaven.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        // GET: Shop/ByCategory/123
        public IActionResult ByCategory(int id)
        {
            if (id == 1)
            {
                ViewData["Category"] = "Sweaters";
            }
            else if (id == 2)
            {
                ViewData["Category"] = "Hoodies";
            }
            else if (id == 3)
            {
                ViewData["Category"] = "Pants";
            }
            else if (id == 4)
            {
                ViewData["Category"] = "T-Shirts";
            }
            else
            {
                // we have no id so take user back to Shop Index where they can choose a category
                return RedirectToAction("Index");
            }

            // use Product model to make an in-memory list of fake products 
            var products = new List<Product>();

            for (int i = 1; i <=10; i++)
            {
                products.Add(new Product { ProductId = i, Name = "Product " + i, Description = "Description of Product " + i, Price = 10.00M });
            }

            // now pass the product list to the view for display
            return View(products);
        }
    }
}
