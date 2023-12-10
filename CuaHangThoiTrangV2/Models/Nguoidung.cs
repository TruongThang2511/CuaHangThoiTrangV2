using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Nguoidung
{
    public int MaNd { get; set; }

    public int MaRole { get; set; }

    public string TenNd { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Diachi { get; set; }

    public string Sdt { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Confirmpassword { get; set; } = null!;

    public double? Chieucao { get; set; }

    public double? Cannang { get; set; }

    public double? Chieudaichan { get; set; }

    public double? Chieurongchan { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Role MaRoleNavigation { get; set; } = null!;
}
