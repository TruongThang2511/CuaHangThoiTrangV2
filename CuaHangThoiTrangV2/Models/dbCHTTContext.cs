using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CuaHangThoiTrangV2.ModelViews;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CuaHangThoiTrangV2.Models;

public partial class dbCHTTContext : DbContext
{
    public dbCHTTContext()
    {
    }

    public dbCHTTContext(DbContextOptions<dbCHTTContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietdonhang> Chitietdonhangs { get; set; }

    public virtual DbSet<Chitietmau> Chitietmaus { get; set; }

    public virtual DbSet<Chitietsize> Chitietsizes { get; set; }

    public virtual DbSet<Danhgium> Danhgia { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<Mau> Maus { get; set; }

    public virtual DbSet<Nguoidung> Nguoidungs { get; set; }

    public virtual DbSet<Phuongthucthanhtoan> Phuongthucthanhtoans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Thuonghieu> Thuonghieus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TRUONGTHANG\\TRUONGTHANG;Initial Catalog=CHTT2;User ID=sa;Password=Thang@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitietdonhang>(entity =>
        {
            entity.HasKey(e => new { e.MaDh, e.MaSp }).HasName("PK__CHITIETD__CD9CD3A85D4551E2");

            entity.ToTable("CHITIETDONHANG");

            entity.Property(e => e.MaDh).HasColumnName("maDH");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("gia");
            entity.Property(e => e.Soluong).HasColumnName("soluong");
            entity.Property(e => e.Tonggiatri)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("tonggiatri");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDON__maDH__5EBF139D");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Chitietdonhangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDON__maSP__5FB337D6");
        });

        modelBuilder.Entity<Chitietmau>(entity =>
        {
            entity.HasKey(e => new { e.MaSp, e.MaMau }).HasName("PK__CHITIETM__885708903FBB70FB");

            entity.ToTable("CHITIETMAU");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.MaMau).HasColumnName("maMau");
            entity.Property(e => e.Soluong).HasColumnName("soluong");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.Chitietmaus)
                .HasForeignKey(d => d.MaMau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETMA__maMau__6A30C649");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Chitietmaus)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETMAU__maSP__693CA210");
        });

        modelBuilder.Entity<Chitietsize>(entity =>
        {
            entity.HasKey(e => new { e.MaSp, e.MaSize }).HasName("PK__CHITIETS__57A10D40B712D90F");

            entity.ToTable("CHITIETSIZE");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.MaSize).HasColumnName("maSize");
            entity.Property(e => e.Soluong).HasColumnName("soluong");

            entity.HasOne(d => d.MaSizeNavigation).WithMany(p => p.Chitietsizes)
                .HasForeignKey(d => d.MaSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETSI__maSiz__656C112C");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Chitietsizes)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETSIZ__maSP__6477ECF3");
        });

        modelBuilder.Entity<Danhgium>(entity =>
        {
            entity.HasKey(e => e.MaDg).HasName("PK__DANHGIA__7A3EF40E3003252E");

            entity.ToTable("DANHGIA");

            entity.Property(e => e.MaDg).HasColumnName("maDG");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.NoidungDg)
                .HasMaxLength(1000)
                .HasColumnName("noidungDG");
            entity.Property(e => e.Sosao).HasColumnName("sosao");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DANHGIA__maSP__6D0D32F4");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DONHANG__7A3EF40F65C520A8");

            entity.ToTable("DONHANG");

            entity.Property(e => e.MaDh).HasColumnName("maDH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("diachi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(200)
                .HasColumnName("ghichu");
            entity.Property(e => e.HotenKh)
                .HasMaxLength(100)
                .HasColumnName("hotenKH");
            entity.Property(e => e.MaNd).HasColumnName("maND");
            entity.Property(e => e.MaPt).HasColumnName("maPT");
            entity.Property(e => e.NgayDat)
                .HasColumnType("datetime")
                .HasColumnName("ngayDat");
            entity.Property(e => e.Tonggiatri)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("tonggiatri");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasColumnName("trangThai");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.MaNd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONHANG__maND__2B3F6F97");

            entity.HasOne(d => d.MaPtNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.MaPt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONHANG__maPT__2C3393D0");
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaL).HasName("PK__LOAI__DF50718E1F8AFD44");

            entity.ToTable("LOAI");

            entity.Property(e => e.MaL).HasColumnName("maL");
            entity.Property(e => e.TenL)
                .HasMaxLength(50)
                .HasColumnName("tenL");
        });

        modelBuilder.Entity<Mau>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__MAU__27572EAEC417745E");

            entity.ToTable("MAU");

            entity.Property(e => e.MaMau).HasColumnName("maMau");
            entity.Property(e => e.TenMau)
                .HasMaxLength(50)
                .HasColumnName("tenMau");
        });

        modelBuilder.Entity<Nguoidung>(entity =>
        {
            entity.HasKey(e => e.MaNd).HasName("PK__NGUOIDUN__7A3EC7CB7777967C");

            entity.ToTable("NGUOIDUNG");

            entity.Property(e => e.MaNd).HasColumnName("maND");
            entity.Property(e => e.Cannang).HasColumnName("cannang");
            entity.Property(e => e.Chieucao).HasColumnName("chieucao");
            entity.Property(e => e.Chieudaichan).HasColumnName("chieudaichan");
            entity.Property(e => e.Chieurongchan).HasColumnName("chieurongchan");
            entity.Property(e => e.Confirmpassword)
                .HasMaxLength(50)
                .HasColumnName("confirmpassword");
            entity.Property(e => e.Diachi)
                .HasMaxLength(200)
                .HasColumnName("diachi");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.MaRole).HasColumnName("maRole");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("sdt");
            entity.Property(e => e.TenNd)
                .HasMaxLength(100)
                .HasColumnName("tenND");

            entity.HasOne(d => d.MaRoleNavigation).WithMany(p => p.Nguoidungs)
                .HasForeignKey(d => d.MaRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NGUOIDUNG__maRol__267ABA7A");
        });

        modelBuilder.Entity<Phuongthucthanhtoan>(entity =>
        {
            entity.HasKey(e => e.MaPt).HasName("PK__PHUONGTH__7A3ED79C763D048D");

            entity.ToTable("PHUONGTHUCTHANHTOAN");

            entity.Property(e => e.MaPt).HasColumnName("maPT");
            entity.Property(e => e.TenPt)
                .HasMaxLength(50)
                .HasColumnName("tenPT");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.MaRole).HasName("PK__ROLES__509E0512B819F558");

            entity.ToTable("ROLES");

            entity.Property(e => e.MaRole).HasColumnName("maRole");
            entity.Property(e => e.TenRole)
                .HasMaxLength(50)
                .HasColumnName("tenRole");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SANPHAM__7A227A7AD6C7CBBD");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("gia");
            entity.Property(e => e.Hinh)
                .HasMaxLength(50)
                .HasColumnName("hinh");
            entity.Property(e => e.Hinh1)
                .HasMaxLength(50)
                .HasColumnName("hinh1");
            entity.Property(e => e.Hinh2)
                .HasMaxLength(50)
                .HasColumnName("hinh2");
            entity.Property(e => e.MaL).HasColumnName("maL");
            entity.Property(e => e.MaTh).HasColumnName("maTH");
            entity.Property(e => e.Soluong).HasColumnName("soluong");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("tenSP");

            entity.HasOne(d => d.MaLNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaL)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__maL__5535A963");

            entity.HasOne(d => d.MaThNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaTh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SANPHAM__maTH__5629CD9C");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.MaSize).HasName("PK__SIZE__D83773A3D2EDE676");

            entity.ToTable("SIZE");

            entity.Property(e => e.MaSize).HasColumnName("maSize");
            entity.Property(e => e.TenS)
                .HasMaxLength(50)
                .HasColumnName("tenS");
        });

        modelBuilder.Entity<Thuonghieu>(entity =>
        {
            entity.HasKey(e => e.MaTh).HasName("PK__THUONGHI__7A226253C0B459DE");

            entity.ToTable("THUONGHIEU");

            entity.Property(e => e.MaTh).HasColumnName("maTH");
            entity.Property(e => e.Logo)
                .HasMaxLength(50)
                .HasColumnName("logo");
            entity.Property(e => e.Mota)
                .HasMaxLength(1000)
                .HasColumnName("mota");
            entity.Property(e => e.TenTh)
                .HasMaxLength(50)
                .HasColumnName("tenTH");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<CuaHangThoiTrangV2.ModelViews.RegisterViewModel> RegisterViewModel { get; set; } = default!;

    public DbSet<CuaHangThoiTrangV2.ModelViews.LoginViewModel> LoginViewModel { get; set; } = default!;

    public DbSet<CuaHangThoiTrangV2.ModelViews.MuaHangViewModel> MuaHangViewModel { get; set; } = default!;
}
