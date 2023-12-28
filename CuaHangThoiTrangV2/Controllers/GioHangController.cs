using CuaHangThoiTrangV2.Extensions;
using CuaHangThoiTrangV2.Models;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Controllers
{
    
    public class GioHangController : Controller
    {
        private readonly dbCHTTContext _context;
        public GioHangController(dbCHTTContext context)
        {
            _context = context;
        }

        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [HttpPost]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> gioHang = GioHang;
            try
            {
                
                CartItem item = GioHang.SingleOrDefault(p => p.sanpham.MaSp == productID);
                if (item != null)
                {
                    if (amount.HasValue)
                        item.amount = amount.Value;
                    else
                        item.amount++;
                }
                else
                {
                    Sanpham hh = _context.Sanphams.SingleOrDefault(p => p.MaSp == productID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        sanpham = hh
                    };
                    gioHang.Add(item);
                }

                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });

            }           
        }

        [HttpPost]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.sanpham.MaSp == productID);
                if (item != null)
                    gioHang.Remove(item);

                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch 
            {
                return Json(new { success = false });
            }          
        }

        public IActionResult Index()
        {

            return View(GioHang);
        }
        [HttpPost]
        public ActionResult UpdateCart(int productID,int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if(cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.sanpham.MaSp == productID);
                    if(item != null && amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
