using CuaHangThoiTrangV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
