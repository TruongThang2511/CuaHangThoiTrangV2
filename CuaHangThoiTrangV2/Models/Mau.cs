using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Mau
{
    public int MaMau { get; set; }

    public string TenMau { get; set; } = null!;

    public virtual ICollection<Chitietmau> Chitietmaus { get; set; } = new List<Chitietmau>();
}
