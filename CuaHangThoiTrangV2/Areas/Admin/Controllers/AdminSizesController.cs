using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangThoiTrangV2.Models;
using PagedList.Core;

namespace CuaHangThoiTrangV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSizesController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminSizesController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/Sizes
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsSizes = _context.Sizes
                                    .AsNoTracking()
                                    .OrderByDescending(x => x.MaSize);

            PagedList<Size> models = new PagedList<Size>(lsSizes, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Sizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.MaSize == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // GET: Admin/Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSize,TenS")] Size size)
        {
            if (ModelState.IsValid)
            {
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        // GET: Admin/Sizes/Edit/5
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

        // POST: Admin/Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSize,TenS")] Size size)
        {
            if (id != size.MaSize)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.MaSize))
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

        // GET: Admin/Sizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sizes == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.MaSize == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Admin/Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sizes == null)
            {
                return Problem("Entity set 'dbCHTTContext.Sizes'  is null.");
            }
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
          return (_context.Sizes?.Any(e => e.MaSize == id)).GetValueOrDefault();
        }
    }
}
