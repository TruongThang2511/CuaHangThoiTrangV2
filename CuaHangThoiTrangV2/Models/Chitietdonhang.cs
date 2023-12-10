using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Chitietdonhang
{
    public int MaDh { get; set; }

    public int MaSp { get; set; }

    public int Soluong { get; set; }

    public decimal Gia { get; set; }

    public decimal Tonggiatri { get; set; }

    public virtual Donhang MaDhNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
