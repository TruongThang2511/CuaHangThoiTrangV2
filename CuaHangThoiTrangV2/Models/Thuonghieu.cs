using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Thuonghieu
{
    public int MaTh { get; set; }

    public string TenTh { get; set; } = null!;

    public string? Mota { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
