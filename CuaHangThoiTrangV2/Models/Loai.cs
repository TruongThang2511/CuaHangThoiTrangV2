using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Loai
{
    public int MaL { get; set; }

    public string TenL { get; set; } = null!;

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
