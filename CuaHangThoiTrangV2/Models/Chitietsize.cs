using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Chitietsize
{
    public int MaSp { get; set; }

    public int MaSize { get; set; }

    public int Soluong { get; set; }

    public virtual Size MaSizeNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
