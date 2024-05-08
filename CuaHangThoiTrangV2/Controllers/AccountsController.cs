using CuaHangThoiTrangV2.Extensions;
using CuaHangThoiTrangV2.Models;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CuaHangThoiTrangV2.Controllers
{
    public class AccountsController : Controller
    {
        private readonly dbCHTTContext _context;

        public AccountsController(dbCHTTContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult ValidatePhone(string phoneNumber)
        {
            try
            {
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.Sdt.ToLower() == phoneNumber.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + phoneNumber + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [AllowAnonymous]
        public IActionResult ValidateEmail(string email)
        {
            try
            {
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + email + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNd");
            if (taikhoanID != null)
            {
                var khachhang = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.MaNd == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonhang = _context.Donhangs.AsNoTracking()
                        .Where(x => x.MaNd == khachhang.MaNd)
                        .OrderByDescending(x => x.MaDh).ToList();
                    ViewBag.Donhang = lsDonhang;
                    return View(khachhang);
                }                        
            }    
            return RedirectToAction("Login","Accounts");
        }
        //đăng ký
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // ĐĂNG KÝ PHƯƠNG THỨC POST
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel nguoidung)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Nguoidung tk = new Nguoidung
                    {
                        MaRole = 3,
                        TenNd = nguoidung.FullName,
                        Email = nguoidung.Email,
                        Sdt = nguoidung.Phone,
                        Password = nguoidung.Password.ToMD5(),
                        Confirmpassword = nguoidung.ConfirmPassword
                    };
                    try
                    {
                        _context.Add(tk);
                        await _context.SaveChangesAsync();
                        //Lưu session khách hàng
                        HttpContext.Session.SetString("MaNd", tk.MaNd.ToString());
                        var taikhoanID = HttpContext.Session.GetString("MaNd");
                        //identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, tk.TenNd),
                            new Claim("MaNd", tk.MaNd.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        //var SpCart = GioHang;
                        //if (SpCart.Count > 0) return RedirectToAction("Shipping", "Checkout");
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Index", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Register", "Accounts");
                    }
                }
                else
                    return View(nguoidung);
            }
            catch
            {
                return View(nguoidung);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var taikhoanID = HttpContext.Session.GetString("MaNd");
            if(taikhoanID != null)
            {
                return RedirectToAction("Index", "Accounts");
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnURL = null)
        {
            try
            {
                if (ModelState.IsValid) {
                    var nguoidung = _context.Nguoidungs.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == model.email);

                    if(nguoidung == null)
                    {
                        return RedirectToAction("Register");
                    }

                    string pass = model.Password.ToMD5();
                    if(nguoidung.Password != pass)
                    {
                        ViewBag.Error = "+ Sai thông tin đăng nhập";
                        return View(model);
                    }
                    HttpContext.Session.SetString("MaNd", nguoidung.MaNd.ToString());
                    var taikhoanID = HttpContext.Session.GetString("MaNd");
                    HttpContext.Session.SetString("TenNd", nguoidung.TenNd.ToString());
                    var tenND = HttpContext.Session.GetString("TenNd");
                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, nguoidung.TenNd),
                        new Claim("MaNd", nguoidung.MaNd.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    if(nguoidung.MaRole == 1)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex) {
                return RedirectToAction("Register", "Accounts");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaNd");
            return RedirectToAction("Index", "Home");
        }
    }
}
