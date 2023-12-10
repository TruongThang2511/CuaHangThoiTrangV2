using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Phuongthucthanhtoan
{
    public int MaPt { get; set; }

    public string TenPt { get; set; } = null!;

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}
