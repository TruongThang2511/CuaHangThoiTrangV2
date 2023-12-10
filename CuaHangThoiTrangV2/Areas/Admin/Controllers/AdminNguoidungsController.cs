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
    public class AdminNguoidungsController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminNguoidungsController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminNguoidungs
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsNguoidung = _context.Nguoidungs.AsNoTracking()
                .OrderByDescending(x => x.MaNd);

            PagedList<Nguoidung> models = new PagedList<Nguoidung>(lsNguoidung, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewData["MaRole"] = new SelectList(_context.Roles, "MaRole", "TenRole");
            return View(models);
        }

        // GET: Admin/AdminNguoidungs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nguoidungs == null)
            {
                return NotFound();
            }

            var nguoidung = await _context.Nguoidungs
                .Include(n => n.MaRoleNavigation)
                .FirstOrDefaultAsync(m => m.MaNd == id);
            if (nguoidung == null)
            {
                return NotFound();
            }

            return View(nguoidung);
        }

        // GET: Admin/AdminNguoidungs/Create
        public IActionResult Create()
        {
            ViewData["MaRole"] = new SelectList(_context.Roles, "MaRole", "MaRole");
            return View();
        }

        // POST: Admin/AdminNguoidungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNd,MaRole,TenNd,Email,Diachi,Sdt,Password,Confirmpassword,Chieucao,Cannang,Chieudaichan,Chieurongchan")] Nguoidung nguoidung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguoidung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaRole"] = new SelectList(_context.Roles, "MaRole", "MaRole", nguoidung.MaRole);
            return View(nguoidung);
        }

        // GET: Admin/AdminNguoidungs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nguoidungs == null)
            {
                return NotFound();
            }

            var nguoidung = await _context.Nguoidungs.FindAsync(id);
            if (nguoidung == null)
            {
                return NotFound();
            }
            ViewData["MaRole"] = new SelectList(_context.Roles, "MaRole", "MaRole", nguoidung.MaRole);
            return View(nguoidung);
        }

        // POST: Admin/AdminNguoidungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNd,MaRole,TenNd,Email,Diachi,Sdt,Password,Confirmpassword,Chieucao,Cannang,Chieudaichan,Chieurongchan")] Nguoidung nguoidung)
        {
            if (id != nguoidung.MaNd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoidung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoidungExists(nguoidung.MaNd))
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
            ViewData["MaRole"] = new SelectList(_context.Roles, "MaRole", "MaRole", nguoidung.MaRole);
            return View(nguoidung);
        }

        // GET: Admin/AdminNguoidungs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nguoidungs == null)
            {
                return NotFound();
            }

            var nguoidung = await _context.Nguoidungs
                .Include(n => n.MaRoleNavigation)
                .FirstOrDefaultAsync(m => m.MaNd == id);
            if (nguoidung == null)
            {
                return NotFound();
            }

            return View(nguoidung);
        }

        // POST: Admin/AdminNguoidungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nguoidungs == null)
            {
                return Problem("Entity set 'dbCHTTContext.Nguoidungs'  is null.");
            }
            var nguoidung = await _context.Nguoidungs.FindAsync(id);
            if (nguoidung != null)
            {
                _context.Nguoidungs.Remove(nguoidung);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NguoidungExists(int id)
        {
          return (_context.Nguoidungs?.Any(e => e.MaNd == id)).GetValueOrDefault();
        }

        public IActionResult Filter(int RoleID = 0)
        {
            var url = $"/Admin/AdminNguoidungs?RoleID={RoleID}";
            if (RoleID == 0)
            {
                url = $"/Admin/AdminNguoidungs";
            }

            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
