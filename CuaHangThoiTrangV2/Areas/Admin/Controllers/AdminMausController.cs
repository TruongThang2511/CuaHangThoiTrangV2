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
    public class AdminMausController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminMausController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/Maus
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsMaus = _context.Maus
                                    .AsNoTracking()
                                    .OrderByDescending(x => x.MaMau);

            PagedList<Mau> models = new PagedList<Mau>(lsMaus, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/Maus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Maus == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus
                .FirstOrDefaultAsync(m => m.MaMau == id);
            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        // GET: Admin/Maus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Maus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMau,TenMau")] Mau mau)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mau);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mau);
        }

        // GET: Admin/Maus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Maus == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus.FindAsync(id);
            if (mau == null)
            {
                return NotFound();
            }
            return View(mau);
        }

        // POST: Admin/Maus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMau,TenMau")] Mau mau)
        {
            if (id != mau.MaMau)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mau);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MauExists(mau.MaMau))
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
            return View(mau);
        }

        // GET: Admin/Maus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Maus == null)
            {
                return NotFound();
            }

            var mau = await _context.Maus
                .FirstOrDefaultAsync(m => m.MaMau == id);
            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        // POST: Admin/Maus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Maus == null)
            {
                return Problem("Entity set 'dbCHTTContext.Maus'  is null.");
            }
            var mau = await _context.Maus.FindAsync(id);
            if (mau != null)
            {
                _context.Maus.Remove(mau);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MauExists(int id)
        {
          return (_context.Maus?.Any(e => e.MaMau == id)).GetValueOrDefault();
        }
    }
}
