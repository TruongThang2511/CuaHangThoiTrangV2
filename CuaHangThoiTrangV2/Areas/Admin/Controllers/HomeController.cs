using CuaHangThoiTrangV2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace CuaHangThoiTrangV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly dbCHTTContext _context;
        public HomeController(dbCHTTContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            var taikhoanID = HttpContext.Session.GetString("MaNd");

            var user = _context.Nguoidungs
                .FirstOrDefault(x => x.MaNd.ToString().Contains(taikhoanID));

            ViewBag.phuongthuc = new SelectList(_context.Phuongthucthanhtoans, "MaPt", "TenPt", 1);

            ViewBag.tongdoanhthu = (double)_context.Donhangs.Sum(m => m.Tonggiatri);

            if (user == null)
                return NotFound();

            if (user.MaRole != 1)
                return NotFound();

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaNd");
            return RedirectToAction("Index", "Home",new { area = "default" });
        }
    }
}
