using CuaHangThoiTrangV2.Extensions;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangThoiTrangV2.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
