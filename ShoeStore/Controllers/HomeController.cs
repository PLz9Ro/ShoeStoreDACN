using Microsoft.AspNetCore.Mvc;
using ShoeStore.Models;
using System.Diagnostics;

namespace ShoeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShoeStoreContext _context;
        public HomeController(ILogger<HomeController> logger , ShoeStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult _NewProductPartialView()
        {
            var items = _context.Products.OrderByDescending(p => p.DateCreated).ToList();
            return PartialView(items);
        }
        public ActionResult _NikePartialView()
        {
            var nikeProducts = _context.Products
        .Where(p => p.Category != null && p.Category.CategoryId == 9)
        .OrderByDescending(p => p.DateCreated)
        .ToList();

            return View(nikeProducts);
        }
        public IActionResult _AllProductsPartial(int categoryId)
        {
            var products = _context.Products
                .Where(p => p.Category != null && p.Category.CategoryId == categoryId)
                .OrderByDescending(p => p.DateCreated)
                .ToList();

            return PartialView( products);
        }
    }
}