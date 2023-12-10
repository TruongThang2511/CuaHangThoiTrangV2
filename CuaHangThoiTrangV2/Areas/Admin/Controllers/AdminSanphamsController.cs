using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangThoiTrangV2.Models;
using PagedList.Core;
using CuaHangThoiTrangV2.Helper;
using CuaHangThoiTrangV2.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CuaHangThoiTrangV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminSanphamsController : Controller
    {
        private readonly dbCHTTContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminSanphamsController(dbCHTTContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/AdminSanphams
        public IActionResult Index(int page = 1, int LoaiID = 0, int ThuonghieuID = 0)
        {
            var pageNumber = page;
            var pageSize = 20;

            List<Sanpham> lsSanpham = new List<Sanpham>();
            if(LoaiID != 0)
            {
                lsSanpham = _context.Sanphams
                .AsNoTracking()
                .Where(x => x.MaL == LoaiID)
                .Include(x => x.MaThNavigation)
                .OrderByDescending(x => x.MaSp).ToList();
            }
            else
            {
                if(ThuonghieuID != 0)
                {
                    lsSanpham = _context.Sanphams
                        .AsNoTracking()
                        .Where(x => x.MaTh == ThuonghieuID)
                        .Include(x => x.MaLNavigation)
                        .OrderByDescending(x => x.MaSp).ToList();
                }
                else
                {
                    lsSanpham = _context.Sanphams
                       .AsNoTracking()
                       .Include(x => x.MaLNavigation)
                       .Include(x => x.MaThNavigation)
                       .OrderByDescending(x => x.MaSp).ToList();
                }              
            }

            PagedList<Sanpham> models = new PagedList<Sanpham>(lsSanpham.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentLoaiID = LoaiID;
            ViewBag.CurrentThuonghieuID = ThuonghieuID;
            ViewBag.CurrentPage = pageNumber;

            ViewData["MaL"] = new SelectList(_context.Loais, "MaL","TenL");
            ViewData["MaTh"] = new SelectList(_context.Thuonghieus, "MaTh","TenTh");

            return View(models);
        }

        // GET: Admin/AdminSanphams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sanphams == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.MaLNavigation)
                .Include(s => s.MaThNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // GET: Admin/AdminSanphams/Create
        public IActionResult Create()
        {
            ViewData["MaL"] = new SelectList(_context.Loais, "MaL", "TenL");
            ViewData["MaTh"] = new SelectList(_context.Thuonghieus, "MaTh", "TenTh");
            return View();
        }

        // POST: Admin/AdminSanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,MaL,MaTh,TenSp,Gia,Hinh,Hinh1,Hinh2,Soluong")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanpham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaL"] = new SelectList(_context.Loais, "MaL", "TenL", sanpham.MaL);
            ViewData["MaTh"] = new SelectList(_context.Thuonghieus, "MaTh", "TenTh", sanpham.MaTh);
            //sanpham.MaLNavigation = 
            return View(sanpham);
        }

        // GET: Admin/AdminSanphams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sanphams == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null)
            {
                return NotFound();
            }
            ViewData["MaL"] = new SelectList(_context.Loais, "MaL", "TenL", sanpham.MaL);
            ViewData["MaTh"] = new SelectList(_context.Thuonghieus, "MaTh", "TenTh", sanpham.MaTh);
            return View(sanpham);
        }

        // POST: Admin/AdminSanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSp,MaL,MaTh,TenSp,Gia,Hinh,Hinh1,Hinh2,Soluong")] Sanpham sanpham)
        {
            if (id != sanpham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanpham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanphamExists(sanpham.MaSp))
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
            ViewData["MaL"] = new SelectList(_context.Loais, "MaL", "TenL", sanpham.MaL);
            ViewData["MaTh"] = new SelectList(_context.Thuonghieus, "MaTh", "TenTh", sanpham.MaTh);
            return View(sanpham);
        }

        // GET: Admin/AdminSanphams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sanphams == null)
            {
                return NotFound();
            }

            var sanpham = await _context.Sanphams
                .Include(s => s.MaLNavigation)
                .Include(s => s.MaThNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanpham == null)
            {
                return NotFound();
            }

            return View(sanpham);
        }

        // POST: Admin/AdminSanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sanphams == null)
            {
                return Problem("Entity set 'dbCHTTContext.Sanphams'  is null.");
            }
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                _context.Sanphams.Remove(sanpham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanphamExists(int id)
        {
          return (_context.Sanphams?.Any(e => e.MaSp == id)).GetValueOrDefault();
        }

        public IActionResult Filter(int LoaiID = 0,int ThuonghieuID = 0)
        {
            var url = $"/Admin/AdminSanphams?LoaiID={LoaiID}&ThuonghieuID={ThuonghieuID}";
            if(LoaiID == 0)
            {
                url = $"/Admin/AdminSanphams";
            }
            else
            {
                if (LoaiID == 0) url = $"/Admin/AdminSanphams?ThuonghieuID={ThuonghieuID}";
                if(ThuonghieuID == 0) url = $"/Admin/AdminSanphams?LoaiID={LoaiID}";
            }    

            return Json(new {status = "success", redirectUrl = url});
        }      
    }
}
