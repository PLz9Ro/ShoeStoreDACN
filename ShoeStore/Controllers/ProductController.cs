using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShoeStore.Models;

namespace ShoeStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShoeStoreContext _context;
        public ProductController(ShoeStoreContext context)
        {
            _context = context;
        }

        [Route("shop.html", Name = "Product")]

        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 10;
                var lsBlog = _context.Products
                .AsNoTracking()
                .OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsBlog, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch {
                return RedirectToAction("Index", "Home");

            }
            
        }
        [Route("/{Alias}", Name = "ListProduct")]
        public IActionResult List(string Alias  , int page=1)
        {
            try
            {
                var pageSize = 10;
                var Cate = _context.Categories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias );
                var lsBlog = _context.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == Cate.CategoryId)
                .OrderByDescending(x => x.DateCreated);

                PagedList<Product> models = new PagedList<Product>(lsBlog, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = Cate;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
         
        }

        [Route("/{Alias}-{id}.html", Name = "ProductDetails")]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Products.Include(p => p.Category).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                var lsproduct = _context.Products.AsNoTracking()
                    .Where(x => x.CategoryId == product.CategoryId && x.ProductId != id && x.Active == true)
                    .OrderByDescending(x => x.DateCreated)
                    .Take(4)
                    .ToList();

                ViewBag.product = lsproduct;
                return View(product);

            }
            catch {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult SearchProducts(string searchText)
        {
            var products = _context.Products
                .Where(p => p.ProductName.Contains(searchText))
                .ToList();

            return Json(products);
        }
    }
}
