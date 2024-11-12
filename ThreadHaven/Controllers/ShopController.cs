using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using ThreadHaven.Data;
using ThreadHaven.Models;

namespace ThreadHaven.Controllers
{
    public class ShopController : Controller
    {
        // global db connection for all methods in controller
        private readonly ApplicationDbContext _context;

        // get access to appsettings.json to read the stripe key
        private readonly IConfiguration _configuration;

        // constructor to get db connection
        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        // POST: /Shop/AddToCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int Quantity, string Size, int ProductId)
        {
            // find price of selected product
            var product = _context.Products.FirstOrDefault(p => p.ProductId == ProductId);
            var price = product.Price;

            // identify user
            var customerId = GetCustomerId();

            // check if this user already has this item in this size in their cart
            // if they do => update quantity, if they don't => add new item
            var cartItem = _context.CartItems
                .SingleOrDefault(c => c.ProductId == ProductId && c.CustomerId == customerId && c.Size == Size);

            if (cartItem == null)
            {
                // create & save new CartItem
                cartItem = new CartItem
                {
                    Quantity = Quantity,
                    Size = Size,
                    ProductId = ProductId,
                    Price = price,
                    CustomerId = customerId
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity = cartItem.Quantity + Quantity;
                _context.CartItems.Update(cartItem);
            }
                
            _context.SaveChanges();

            // redirect to Cart
            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            // do we already have a Session variable identifier for this user's cart?
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                // we don't have a Session var identifier yet; we have to make one
                HttpContext.Session.SetString("CustomerId", Guid.NewGuid().ToString());
            }

            // send back the cart identifier unique to this particular browser session
            return HttpContext.Session.GetString("CustomerId");
        }

        // GET: /Shop/Cart
        public IActionResult Cart()
        {
            // fetch items in user's cart & include parent reference to Product
            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CustomerId == GetCustomerId()).ToList();

            // calculate total # of items in cart
            int itemCount = (from c in cartItems
                             select c.Quantity).Sum();

            // store item count in session var for display in navbar
            HttpContext.Session.SetInt32("ItemCount", itemCount);

            return View(cartItems);
        }

        // GET: /Shop/RemoveFromCart/5
        public IActionResult RemoveFromCart(int id)
        {
            // find & remove selected cart item
            var cartItem = _context.CartItems.SingleOrDefault(c => c.CartItemId == id);
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            // show updated cart
            return RedirectToAction("Cart");
        }

        // GET: /Shop/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: /Shop/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone")] Order order)
        {
            // auto-fill the other order properties
            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;
            order.OrderTotal = (from c in _context.CartItems
                                where c.CustomerId == HttpContext.Session.GetString("CustomerId")
                                select c.Quantity * c.Price).Sum();

            // use SessionExtensions class to store entire order in single session var
            // code from https://www.talkingdotnet.com/store-complex-objects-in-asp-net-core-session/
            HttpContext.Session.SetObject("Order", order);

            return RedirectToAction("Payment");
        }

        // GET: /Shop/Payment
        [Authorize]
        public IActionResult Payment()
        {
            // get the current order from the session var
            var order = HttpContext.Session.GetObject<Order>("Order");
            var orderTotal = order.OrderTotal;

            // get stripe key from configuration
            StripeConfiguration.ApiKey = _configuration.GetValue<string>("StripeSecretKey");

            // create new Stripe payment
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                      PriceData = new SessionLineItemPriceDataOptions
                      {
                        UnitAmount = (long?)(orderTotal * 100),
                        Currency = "cad",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Thread Haven Purchase"
                        }
                      },
                      Quantity = 1
                  },
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Shop/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Shop/Cart",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            // redirect user after payment
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);            
        }
    }
}
