using Microsoft.AspNetCore.Mvc;

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

            return View();
        }
    }
}
