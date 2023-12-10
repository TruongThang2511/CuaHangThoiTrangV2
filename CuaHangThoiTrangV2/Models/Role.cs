using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Role
{
    public int MaRole { get; set; }

    public string TenRole { get; set; } = null!;

    public virtual ICollection<Nguoidung> Nguoidungs { get; set; } = new List<Nguoidung>();
}
