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
    public class AdminThuonghieusController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminThuonghieusController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminThuonghieus
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsThuonghieus = _context.Thuonghieus
                                    .AsNoTracking()
                                    .OrderByDescending(x => x.MaTh);

            PagedList<Thuonghieu> models = new PagedList<Thuonghieu>(lsThuonghieus, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminThuonghieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Thuonghieus == null)
            {
                return NotFound();
            }

            var thuonghieu = await _context.Thuonghieus
                .FirstOrDefaultAsync(m => m.MaTh == id);
            if (thuonghieu == null)
            {
                return NotFound();
            }

            return View(thuonghieu);
        }

        // GET: Admin/AdminThuonghieus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminThuonghieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTh,TenTh,Mota,Logo")] Thuonghieu thuonghieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thuonghieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thuonghieu);
        }

        // GET: Admin/AdminThuonghieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Thuonghieus == null)
            {
                return NotFound();
            }

            var thuonghieu = await _context.Thuonghieus.FindAsync(id);
            if (thuonghieu == null)
            {
                return NotFound();
            }
            return View(thuonghieu);
        }

        // POST: Admin/AdminThuonghieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTh,TenTh,Mota,Logo")] Thuonghieu thuonghieu)
        {
            if (id != thuonghieu.MaTh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuonghieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuonghieuExists(thuonghieu.MaTh))
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
            return View(thuonghieu);
        }

        // GET: Admin/AdminThuonghieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Thuonghieus == null)
            {
                return NotFound();
            }

            var thuonghieu = await _context.Thuonghieus
                .FirstOrDefaultAsync(m => m.MaTh == id);
            if (thuonghieu == null)
            {
                return NotFound();
            }

            return View(thuonghieu);
        }

        // POST: Admin/AdminThuonghieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thuonghieus == null)
            {
                return Problem("Entity set 'dbCHTTContext.Thuonghieus'  is null.");
            }
            var thuonghieu = await _context.Thuonghieus.FindAsync(id);
            if (thuonghieu != null)
            {
                _context.Thuonghieus.Remove(thuonghieu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuonghieuExists(int id)
        {
          return (_context.Thuonghieus?.Any(e => e.MaTh == id)).GetValueOrDefault();
        }
    }
}
