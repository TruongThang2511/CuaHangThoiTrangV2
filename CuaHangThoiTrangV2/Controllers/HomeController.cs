using CuaHangThoiTrangV2.Models;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CuaHangThoiTrangV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbCHTTContext _context;

        public HomeController(ILogger<HomeController> logger, dbCHTTContext context)
        {
            
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.lsLoais = _context.Loais.AsNoTracking().OrderByDescending(x => x.MaL).ToList();
            ViewBag.lsThuonghieus = _context.Thuonghieus.AsNoTracking().OrderByDescending(x => x.MaTh).ToList();
            ViewBag.lsSP = _context.Sanphams.AsNoTracking().OrderByDescending(x => x.MaSp).Take(8).ToList();
            var taikhoan = HttpContext.Session.GetString("MaNd");
            //if (tenNd != null) ViewBag.tenND = _context.Nguoidungs.FirstOrDefault(x => x.TenNd.ToString().Contains(tenNd)).ToString();
            //ViewBag.tenND = _context.Nguoidungs.AsNoTracking().Where(x => x.TenNd.ToString().Contains(tenNd)).ToString();
            var user = _context.Nguoidungs
                            .FirstOrDefault(x => x.MaNd.ToString().Contains(taikhoan));

            if(user != null)
            {
                ViewBag.tenND = user.TenNd.ToString();
                if (user.MaRole == 1)
                    ViewBag.role = "1";
            }

            return View();
        }
        [Route("lien-he.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}