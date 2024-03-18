using CuaHangThoiTrangV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace CuaHangThoiTrangV2.Controllers
{
    public class ProductController : Controller
    {
        private readonly dbCHTTContext _context;
        public ProductController(dbCHTTContext context)
        {
            _context = context;
        }
        [Route("shop.html", Name = "ShopProduct")]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 12;
            var lsSanphams = _context.Sanphams
                .AsNoTracking()
                .OrderByDescending(x => x.MaSp);
            PagedList<Sanpham> models = new PagedList<Sanpham>(lsSanphams, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        
        public IActionResult List(int id, int page = 1)
        {
            try
            {
                var pageSize = 12;
                var loai = _context.Loais.AsNoTracking().SingleOrDefault(x => x.MaL == id);
                var lsSanphams = _context.Sanphams
                    .AsNoTracking()
                    .Where(x => x.MaL == loai.MaL)
                    .OrderByDescending(x => x.MaSp);
                PagedList<Sanpham> models = new PagedList<Sanpham>(lsSanphams, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentLoai = loai;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        public IActionResult ListThuonghieu(int id, int page = 1)
        {
            try
            {
                var pageSize = 12;
                var thuonghieu = _context.Thuonghieus.AsNoTracking().SingleOrDefault(x => x.MaTh == id);
                var lsSanphams = _context.Sanphams
                    .AsNoTracking()
                    .Where(x => x.MaTh == thuonghieu.MaTh)
                    .OrderByDescending(x => x.MaSp);
                PagedList<Sanpham> models = new PagedList<Sanpham>(lsSanphams, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentThuonghieu = thuonghieu;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ListLoai(int id, int page = 1)
        {
            try
            {
                var pageSize = 12;
                var loai = _context.Loais.AsNoTracking().SingleOrDefault(x => x.MaL == id);
                var lsSanphams = _context.Sanphams
                    .AsNoTracking()
                    .Where(x => x.MaL == loai.MaL)
                    .OrderByDescending(x => x.MaSp);
                PagedList<Sanpham> models = new PagedList<Sanpham>(lsSanphams, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentLoai = loai;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var product = _context.Sanphams.Include(x => x.MaLNavigation).Include(x => x.MaThNavigation).FirstOrDefault(x => x.MaSp == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Chitiet = product;
                
                var lsSanphams = _context.Sanphams
                    .AsNoTracking()
                    .Where(x => x.MaL == product.MaL && x.MaSp != id)
                    .OrderByDescending(x => x.MaSp)
                    .Take(4)
                    .ToList();

                ViewBag.Sanpham = lsSanphams;

                return View(product);

                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        } 
    }
}
