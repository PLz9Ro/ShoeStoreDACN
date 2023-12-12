using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.Extension;
using ShoeStore.Models;
using ShoeStore.ModelViews;
using System.Collections.Specialized;

namespace ShoeStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoeStoreContext _context;
        public ShoppingCartController (ShoeStoreContext context)
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
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart); // Gán danh sách mới vào Session
                }
                return cart;
            }
        }
        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            return View(cart);
        }
        [HttpPost]
        [Route("/api/cart/add")]
        public IActionResult AddToCart (int productID , int? amount)
        {
            try
            {
                 List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                CartItem item = Cart.SingleOrDefault(x => x.product.ProductId == productID);
                if (item != null)
                {
                    item.amount = item.amount + amount.Value;
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                }

                else
                {
                    Product product = _context.Products.SingleOrDefault(x => x.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount ?? 1,
                        product = product
                    };
                    cart.Add(item);
                }
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                return Json(new { success = true });

            }
            catch(Exception ex)
            {
                return Json(new { success = false });
            }
             
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            try {
                if(cart != null) { 
                    CartItem item = cart.SingleOrDefault(x =>x.product.ProductId == productID);
                    if(item != null &&amount.HasValue ) {
                        item.amount = amount.Value;
                    }
                    HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                }
                return Json(new { success = true });

            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove (int productID)
        {
            try
            {
                List<CartItem> cart = Cart;
                CartItem item = cart.SingleOrDefault(x =>x.product.ProductId == productID);
                if(item != null)
                {
                    cart.Remove(item);
                }
                HttpContext.Session.Set<List<CartItem>>("Cart", cart);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new {Success = false });
            }
        }

    
    }
}
