using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Danhgium
{
    public int MaDg { get; set; }

    public int MaSp { get; set; }

    public int Sosao { get; set; }

    public string? NoidungDg { get; set; }

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
