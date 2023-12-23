using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Extension;
using ShoeStore.Models;
using ShoeStore.ModelViews;
using System.Net;
using System.Security.Policy;

namespace ShoeStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ShoeStoreContext _context;
        public INotyfService _notifyService { get; }

        public CheckoutController(ShoeStoreContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }
        public List<CartItem> Cart
        {
            get
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
                if (cart == default(List<CartItem>))
                {
                    cart = new List<CartItem>();
                    HttpContext.Session.Set("Cart", cart);
                }
                return cart;
            }
        }
        [Route("Checkout.html", Name = "Checkout")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            var customerid = HttpContext.Session.GetString("CustomerId");
            CheckOutVM model = new CheckOutVM();
            if (customerid != null)
            {
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(customerid));
                model.CustomerId = customer.CustomerId;
                model.FullName = customer.FullName;
                model.Phone = customer.Phone;
                model.Email = customer.Email;
                model.Address = customer.Address;
            }
            ViewBag.Cart = cart;

            return View(model);
        }
        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(CheckOutVM checkout)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            var customerid = HttpContext.Session.GetString("CustomerId");
            CheckOutVM model = new CheckOutVM();
            if (customerid != null)
            {
                var customer = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(customerid));
                model.CustomerId = customer.CustomerId;
                model.FullName = customer.FullName;
                model.Email = customer.Email;
                model.Phone = customer.Phone;

                customer.Address = model.Address;
                customer.City = checkout.City;
                customer.District = checkout.District;
                customer.Ward = checkout.Ward;


                _context.Update(customer);
                _context.SaveChanges();
            }
            try
            {
                var customer = _context.Customers.SingleOrDefault(x => x.CustomerId == model.CustomerId);

                if (ModelState.IsValid)
                    {
                        Order order = new Order();
                        order.CustomerId = model.CustomerId;
                        order.Address = model.Address;
                        order.City = customer.City;
                        order.District = customer.District;
                        order.Ward = customer.Ward;

                    order.OrderDate = DateTime.Now;
                    order.TransactStatusId = 1; 
                    order.Deleted = false;
                    order.Paid = false;


                    order.Note = model.Note;
                    order.TotalMoney = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    _context.Add(order);
                    _context.SaveChanges();


                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = order.OrderId;
                        orderDetail.ProductId = item.product.ProductId;
                        orderDetail.Quantity = item.amount;
                        orderDetail.Total = order.TotalMoney;
                        orderDetail.Price = item.product.Price;
                        orderDetail.CreateDate = DateTime.Now;
                        orderDetail.Size = item.size;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //clear cart
                    HttpContext.Session.Remove("Cart");
                    // Xuất thông báo
                    _notifyService.Success("Đơn hàng đặt thành công");
                    // Cập nhật thông tin khách hàng
                    return RedirectToAction("Success");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Cart = cart;
                return View(model);
            }
            ViewBag.Cart = cart;
            return View(model);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
