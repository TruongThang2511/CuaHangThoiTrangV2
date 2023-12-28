using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangThoiTrangV2.Models;
using PagedList.Core;
using System.Net;

namespace CuaHangThoiTrangV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonhangsController : Controller
    {
        private readonly dbCHTTContext _context;

        public AdminDonhangsController(dbCHTTContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDonhangs
        public IActionResult Index(int? page)
        {

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsDonhang = _context.Donhangs.AsNoTracking()
                .Include(x => x.MaPtNavigation)
                .OrderByDescending(x => x.MaDh);

            PagedList<Donhang> models = new PagedList<Donhang>(lsDonhang, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        public ActionResult Xacnhan(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            Donhang donhang = _context.Donhangs.Find(id);

            if (donhang == null)
            {
                return NotFound();
            }
            donhang.TrangThai = "Đã xác nhận";
            _context.SaveChanges();

            var chitietdonhang = _context.Chitietdonhangs.Where(x => x.MaDh == id).ToList();
            foreach (var ctdh in chitietdonhang)
            {
                // update so luong
                Sanpham sp = _context.Sanphams.FirstOrDefault(x => x.MaSp == ctdh.MaSp);
                sp.Soluong = sp.Soluong - ctdh.Soluong;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Giaohang(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Donhang donhang = _context.Donhangs.Find(id);

            if (donhang == null)
            {
                return NotFound();
            }
            donhang.TrangThai = "Đang giao hàng";
            _context.SaveChanges();           
            return RedirectToAction("Index");
        }

        public ActionResult Hoanthanh(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Donhang donhang = _context.Donhangs.Find(id);

            if (donhang == null)
            {
                return NotFound();
            }
            donhang.TrangThai = "Đã hoàn thành";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/AdminDonhangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.MaNdNavigation)
                .Include(d => d.MaPtNavigation)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // GET: Admin/AdminDonhangs/Create
        public IActionResult Create()
        {
            ViewData["MaNd"] = new SelectList(_context.Nguoidungs, "MaNd", "MaNd");
            ViewData["MaPt"] = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "MaPt");
            return View();
        }

        // POST: Admin/AdminDonhangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDh,MaNd,MaPt,HotenKh,Email,NgayDat,Tonggiatri,TrangThai,Diachi,Ghichu")] Donhang donhang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donhang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNd"] = new SelectList(_context.Nguoidungs, "MaNd", "MaNd", donhang.MaNd);
            ViewData["MaPt"] = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "MaPt", donhang.MaPt);
            return View(donhang);
        }

        // GET: Admin/AdminDonhangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang == null)
            {
                return NotFound();
            }
            ViewData["MaNd"] = new SelectList(_context.Nguoidungs, "MaNd", "MaNd", donhang.MaNd);
            ViewData["MaPt"] = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "MaPt", donhang.MaPt);
            return View(donhang);
        }

        // POST: Admin/AdminDonhangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDh,MaNd,MaPt,HotenKh,Email,NgayDat,Tonggiatri,TrangThai,Diachi,Ghichu")] Donhang donhang)
        {
            if (id != donhang.MaDh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonhangExists(donhang.MaDh))
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
            ViewData["MaNd"] = new SelectList(_context.Nguoidungs, "MaNd", "MaNd", donhang.MaNd);
            ViewData["MaPt"] = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "MaPt", donhang.MaPt);
            return View(donhang);
        }

        // GET: Admin/AdminDonhangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Donhangs == null)
            {
                return NotFound();
            }

            var donhang = await _context.Donhangs
                .Include(d => d.MaNdNavigation)
                .Include(d => d.MaPtNavigation)
                .FirstOrDefaultAsync(m => m.MaDh == id);
            if (donhang == null)
            {
                return NotFound();
            }

            return View(donhang);
        }

        // POST: Admin/AdminDonhangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donhangs == null)
            {
                return Problem("Entity set 'dbCHTTContext.Donhangs'  is null.");
            }
            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang != null)
            {
                _context.Donhangs.Remove(donhang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonhangExists(int id)
        {
          return (_context.Donhangs?.Any(e => e.MaDh == id)).GetValueOrDefault();
        }
    }
}
