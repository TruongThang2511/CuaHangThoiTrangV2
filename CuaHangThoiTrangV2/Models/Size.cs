using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Size
{
    public int MaSize { get; set; }

    public string TenS { get; set; } = null!;

    public virtual ICollection<Chitietsize> Chitietsizes { get; set; } = new List<Chitietsize>();
}
