using System.ComponentModel.DataAnnotations;

namespace CuaHangThoiTrangV2.ModelViews
{
    public class MuaHangViewModel
    {
        [Key]
        public int MaNd { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ va Tên")]
        public string TenNd { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Dia chi nhận hàng")]
        public string Diachi { get; set; }
        public int PaymentID { get; set; }
        public string Note { get; set; }
    }
}
