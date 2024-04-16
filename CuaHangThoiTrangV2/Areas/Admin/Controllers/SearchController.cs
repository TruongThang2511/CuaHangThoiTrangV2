using CuaHangThoiTrangV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CuaHangThoiTrangV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly dbCHTTContext _context;
        public SearchController(dbCHTTContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Sanpham> ls = new List<Sanpham>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListSanphamsSearchPartial", null);
            }
            ls = _context.Sanphams
                .AsNoTracking()
                .Include(a => a.MaLNavigation)
                .Include(a => a.MaThNavigation)
                .Where(x => x.TenSp.Contains(keyword))
                .OrderByDescending(x => x.TenSp)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListSanphamsSearchPartial", null);
            }
            else
            {
                return PartialView("ListSanphamsSearchPartial", ls);
            }
        }
        [HttpPost]
        public IActionResult Doanhthuthang(int keyword)
        {
            List<Donhang> ls = new List<Donhang>();
            double doanhthu = 0;
            if (keyword == null)
            {
                return PartialView("DoanhthuPartial", null);
            }
            ls = _context.Donhangs
                    .AsNoTracking()
                    .Where(x => x.NgayDat.Month == keyword)
                    .OrderByDescending(x => x.MaDh)
                    .ToList();
            foreach (var item in ls)
            {
                doanhthu = doanhthu + (double)item.Tonggiatri;
            }
            if (ls == null)
            {
                return PartialView("DoanhthuPartial", null);
            }
            else
            {
                return PartialView("DoanhthuPartial", doanhthu);
            }
        }

        public IActionResult DoanhthuPT(string keyword)
        {
            List<Donhang> ls = new List<Donhang>();
            double doanhthu = 0;
            if (keyword == null)
            {
                return PartialView("DoanhthuphuongthucPartial", null);
            }
            var tenPT = _context.Phuongthucthanhtoans.FirstOrDefault(x => x.TenPt.Contains(keyword));
           ls = _context.Donhangs
                    .AsNoTracking()
                    .Where(x => x.MaPtNavigation.TenPt.Contains(tenPT.TenPt))
                    .OrderByDescending(x => x.MaDh)
                    .ToList();
            foreach (var item in ls)
            {
                doanhthu = doanhthu + (double)item.Tonggiatri;
            }
            if (ls == null)
            {
                return PartialView("DoanhthuphuongthucPartial", doanhthu);
            }
            else
            {
                return PartialView("DoanhthuphuongthucPartial", doanhthu);
            }
        }
    }
}
