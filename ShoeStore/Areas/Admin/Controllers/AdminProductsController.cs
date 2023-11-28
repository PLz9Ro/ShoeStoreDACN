using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using ShoeStore.Hellper;
using ShoeStore.Models;

namespace ShoeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly ShoeStoreContext _context;

        public AdminProductsController(ShoeStoreContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index(int? page, int CategoryId = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 15;
            IQueryable<Product> query = _context.Products.AsQueryable().Include(x => x.Cartegory).AsNoTracking().OrderByDescending(x => x.ProductId);

            if (CategoryId != 0)
            {
                query = query.Where(x => x.CartegoryId == CategoryId);
            }

            PagedList<Product> models = new PagedList<Product>(query, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(models);
        }
        public IActionResult Filtter(int CategoryId = 0)
        {
            var url = $"/Admin/AdminProducts?CategoryId={CategoryId}";
            if (CategoryId == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cartegory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesciption,Description,CartegotyId,Price,Discount,Thumb,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitsStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            /* if (ModelState.IsValid)
             {

                 _context.Add(product);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
 *//*            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CartegotyId);
 *//*            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

             return View(product);*/
            if (ModelState.IsValid)
            {
                product.ProductName = Unilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Unilities.SEOUrl(product.ProductName) + extension;
                    product.Thumb = await Unilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Thumb))
                {
                    product.Thumb = "default.jpg";
                }
                product.Alias = Unilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.Now;
                product.DateCreated = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = _context.Categories.Select(c => new
            {
                c.CategoryId,
                c.CategoryName
            }).ToList();
            ViewData["DanhMuc"] = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product);
        }


        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CartegoryId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesciption,Description,CartegotyId,Price,Discount,Thumb,ProductImage,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitsStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fthumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CartegoryId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cartegory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShoeStoreContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
