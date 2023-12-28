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
    public class AdminDanhgiasController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminDanhgiasController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDanhgias
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsDanhgias = _context.Danhgia
                                    .AsNoTracking()
                                    .Include(x => x.MaSpNavigation)
                                    .OrderByDescending(x => x.MaDg);

            PagedList<Danhgium> models = new PagedList<Danhgium>(lsDanhgias, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminDanhgias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Danhgia == null)
            {
                return NotFound();
            }

            var danhgium = await _context.Danhgia
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDg == id);
            if (danhgium == null)
            {
                return NotFound();
            }

            return View(danhgium);
        }

        // GET: Admin/AdminDanhgias/Create
        public IActionResult Create()
        {
            ViewData["MaSp"] = new SelectList(_context.Sanphams, "MaSp", "MaSp");
            return View();
        }

        // POST: Admin/AdminDanhgias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDg,MaSp,Sosao,NoidungDg")] Danhgium danhgium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhgium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaSp"] = new SelectList(_context.Sanphams, "MaSp", "MaSp", danhgium.MaSp);
            return View(danhgium);
        }

        // GET: Admin/AdminDanhgias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Danhgia == null)
            {
                return NotFound();
            }

            var danhgium = await _context.Danhgia.FindAsync(id);
            if (danhgium == null)
            {
                return NotFound();
            }
            ViewData["MaSp"] = new SelectList(_context.Sanphams, "MaSp", "MaSp", danhgium.MaSp);
            return View(danhgium);
        }

        // POST: Admin/AdminDanhgias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDg,MaSp,Sosao,NoidungDg")] Danhgium danhgium)
        {
            if (id != danhgium.MaDg)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhgium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhgiumExists(danhgium.MaDg))
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
            ViewData["MaSp"] = new SelectList(_context.Sanphams, "MaSp", "MaSp", danhgium.MaSp);
            return View(danhgium);
        }

        // GET: Admin/AdminDanhgias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Danhgia == null)
            {
                return NotFound();
            }

            var danhgium = await _context.Danhgia
                .Include(d => d.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaDg == id);
            if (danhgium == null)
            {
                return NotFound();
            }

            return View(danhgium);
        }

        // POST: Admin/AdminDanhgias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Danhgia == null)
            {
                return Problem("Entity set 'dbCHTTContext.Danhgia'  is null.");
            }
            var danhgium = await _context.Danhgia.FindAsync(id);
            if (danhgium != null)
            {
                _context.Danhgia.Remove(danhgium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhgiumExists(int id)
        {
          return (_context.Danhgia?.Any(e => e.MaDg == id)).GetValueOrDefault();
        }
    }
}
