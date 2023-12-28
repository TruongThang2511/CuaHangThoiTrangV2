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
    public class AdminPhuongthucthanhtoansController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminPhuongthucthanhtoansController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminPhuongthucthanhtoans
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsPhuongthucs = _context.Phuongthucthanhtoans
                                    .AsNoTracking()
                                    .OrderByDescending(x => x.MaPt);

            PagedList<Phuongthucthanhtoan> models = new PagedList<Phuongthucthanhtoan>(lsPhuongthucs, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminPhuongthucthanhtoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Phuongthucthanhtoans == null)
            {
                return NotFound();
            }

            var phuongthucthanhtoan = await _context.Phuongthucthanhtoans
                .FirstOrDefaultAsync(m => m.MaPt == id);
            if (phuongthucthanhtoan == null)
            {
                return NotFound();
            }

            return View(phuongthucthanhtoan);
        }

        // GET: Admin/AdminPhuongthucthanhtoans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPhuongthucthanhtoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPt,TenPt")] Phuongthucthanhtoan phuongthucthanhtoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phuongthucthanhtoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(phuongthucthanhtoan);
        }

        // GET: Admin/AdminPhuongthucthanhtoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Phuongthucthanhtoans == null)
            {
                return NotFound();
            }

            var phuongthucthanhtoan = await _context.Phuongthucthanhtoans.FindAsync(id);
            if (phuongthucthanhtoan == null)
            {
                return NotFound();
            }
            return View(phuongthucthanhtoan);
        }

        // POST: Admin/AdminPhuongthucthanhtoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPt,TenPt")] Phuongthucthanhtoan phuongthucthanhtoan)
        {
            if (id != phuongthucthanhtoan.MaPt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phuongthucthanhtoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhuongthucthanhtoanExists(phuongthucthanhtoan.MaPt))
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
            return View(phuongthucthanhtoan);
        }

        // GET: Admin/AdminPhuongthucthanhtoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Phuongthucthanhtoans == null)
            {
                return NotFound();
            }

            var phuongthucthanhtoan = await _context.Phuongthucthanhtoans
                .FirstOrDefaultAsync(m => m.MaPt == id);
            if (phuongthucthanhtoan == null)
            {
                return NotFound();
            }

            return View(phuongthucthanhtoan);
        }

        // POST: Admin/AdminPhuongthucthanhtoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Phuongthucthanhtoans == null)
            {
                return Problem("Entity set 'dbCHTTContext.Phuongthucthanhtoans'  is null.");
            }
            var phuongthucthanhtoan = await _context.Phuongthucthanhtoans.FindAsync(id);
            if (phuongthucthanhtoan != null)
            {
                _context.Phuongthucthanhtoans.Remove(phuongthucthanhtoan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhuongthucthanhtoanExists(int id)
        {
          return (_context.Phuongthucthanhtoans?.Any(e => e.MaPt == id)).GetValueOrDefault();
        }
    }
}
