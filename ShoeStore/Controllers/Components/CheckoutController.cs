using Microsoft.AspNetCore.Mvc;
using ShoeStore.Extension;
using ShoeStore.Models;
using ShoeStore.ModelViews;

namespace ShoeStore.Controllers.Components
{
    public class CheckoutController : Controller
    {
        private readonly ShoeStoreContext _context;
        public CheckoutController(ShoeStoreContext context)
        {
            _context = context;
        }
        public List<CartItem> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if (cart == default(List<CartItem>))
                {
                    cart = new List<CartItem>();
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                }
                return cart;
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
