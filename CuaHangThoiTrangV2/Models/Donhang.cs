using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Donhang
{
    public int MaDh { get; set; }

    public int MaNd { get; set; }

    public int MaPt { get; set; }

    public string HotenKh { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public decimal Tonggiatri { get; set; }

    public string TrangThai { get; set; } = null!;

    public string Diachi { get; set; } = null!;

    public string? Ghichu { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual Nguoidung MaNdNavigation { get; set; } = null!;

    public virtual Phuongthucthanhtoan MaPtNavigation { get; set; } = null!;
}
