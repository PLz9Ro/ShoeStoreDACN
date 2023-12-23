using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;

namespace ShoeStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminSizesController : Controller
    {
        private readonly ShoeStoreContext _context;
        public INotyfService _notifyService { get; }

        public AdminSizesController(ShoeStoreContext context , INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }

        // GET: Admin/AdminSizes
        public async Task<IActionResult> Index()
        {
              return _context.Sizes != null ? 
                          View(await _context.Sizes.ToListAsync()) :
                          Problem("Entity set 'ShoeStoreContext.Sizes'  is null.");
        }

        // GET: Admin/AdminSizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeId == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // GET: Admin/AdminSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SizeId,SizeName,SizeDescription")] Size size)
        {
            if (ModelState.IsValid)
            {
                _context.Add(size);
                await _context.SaveChangesAsync();
                _notifyService.Success("Create Success");
                return RedirectToAction(nameof(Index));

            }
            return View(size);
        }

        // GET: Admin/AdminSizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Admin/AdminSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SizeId,SizeName,SizeDescription")] Size size)
        {
            if (id != size.SizeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    _notifyService.Success("Edit Success");

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.SizeId))
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
            return View(size);
        }

        // GET: Admin/AdminSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeId == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Admin/AdminSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sizes == null)
            {
                return Problem("Entity set 'ShoeStoreContext.Sizes'  is null.");
            }
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
            }
            
            await _context.SaveChangesAsync();
            _notifyService.Warning("Delete Success");
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
          return (_context.Sizes?.Any(e => e.SizeId == id)).GetValueOrDefault();
        }
    }
}
