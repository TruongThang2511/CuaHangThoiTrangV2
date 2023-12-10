using System;
using System.Collections.Generic;

namespace CuaHangThoiTrangV2.Models;

public partial class Sanpham
{
    public int MaSp { get; set; }

    public int MaL { get; set; }

    public int MaTh { get; set; }

    public string TenSp { get; set; } = null!;

    public decimal Gia { get; set; }

    public string Hinh { get; set; } = null!;

    public string? Hinh1 { get; set; }

    public string? Hinh2 { get; set; }

    public int Soluong { get; set; }

    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; } = new List<Chitietdonhang>();

    public virtual ICollection<Chitietmau> Chitietmaus { get; set; } = new List<Chitietmau>();

    public virtual ICollection<Chitietsize> Chitietsizes { get; set; } = new List<Chitietsize>();

    public virtual ICollection<Danhgium> Danhgia { get; set; } = new List<Danhgium>();

    public virtual Loai MaLNavigation { get; set; } = null!;

    public virtual Thuonghieu MaThNavigation { get; set; } = null!;
}
