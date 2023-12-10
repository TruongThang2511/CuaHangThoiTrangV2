using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Chitietmau
{
    public int MaSp { get; set; }

    public int MaMau { get; set; }

    public int Soluong { get; set; }

    public virtual Mau MaMauNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
