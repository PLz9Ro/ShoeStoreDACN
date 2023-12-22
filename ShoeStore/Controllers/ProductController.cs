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

        public IActionResult Index(int? page , string search , int? categoryId)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 10;
                IQueryable<Product> lsBlog = _context.Products.AsNoTracking().OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsBlog, pageNumber, pageSize);
                if (!string.IsNullOrEmpty(search))
                {
                    var searchValue = search.ToLower(); // Chuyển đổi chuỗi tìm kiếm thành chữ thường

                    var products = _context.Products
                        .Where(p => p.ProductName.ToLower().Contains(searchValue) || p.Description.ToLower().Contains(searchValue))
                        .ToPagedList(pageNumber, pageSize);

                    return View(products);
                }
                else
                {
                    // Truy vấn tất cả sản phẩm nếu không có từ khóa tìm kiếm
                    var allProducts = _context.Products.ToPagedList(pageNumber, pageSize);
                }
                    if (categoryId.HasValue)
                    {
                        var products = _context.Products
                            .Where(p => p.CategoryId == categoryId.Value)
                            .ToPagedList(pageNumber, pageSize);

                        return View(products);
                    }
                    else
                    {
                        var allProducts = _context.Products.ToPagedList(pageNumber, pageSize);
                        return View(allProducts);
                    }
                lsBlog = lsBlog.OrderByDescending(x => x.DateCreated);
                ViewBag.CurrentPage = pageNumber;
                ViewBag.SearchTerm = search;
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
