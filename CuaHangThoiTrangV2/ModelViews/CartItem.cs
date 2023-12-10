using CuaHangThoiTrangV2.Models;

namespace CuaHangThoiTrangV2.ModelViews
{
    public class CartItem
    {
        public Sanpham sanpham { get; set; }
        public int amount { get; set; }
        public double tongTien => (double)(amount * sanpham.Gia);
    }
}
