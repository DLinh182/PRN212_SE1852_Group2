using System;
using System.Collections.Generic;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer;

public partial class QlShopQuanAoContext : DbContext
{
    public QlShopQuanAoContext()
    {
    }

    public QlShopQuanAoContext(DbContextOptions<QlShopQuanAoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cthd> Cthds { get; set; }

    public virtual DbSet<Dangnhap> Dangnhaps { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Loaisp> Loaisps { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cthd>(entity =>
        {
            entity.HasKey(e => new { e.MaHd, e.MaSp, e.KichCo }).HasName("PK_MaHD_MaSP_MaNV");

            entity.ToTable("CTHD", tb => tb.HasTrigger("TG_CAPNHAP_SANPHAM_KHITHEM"));

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaSP");
            entity.Property(e => e.KichCo)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_HOADON");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_SANPHAM");
        });

        modelBuilder.Entity<Dangnhap>(entity =>
        {
            entity.HasKey(e => e.Matk);

            entity.ToTable("DANGNHAP");

            entity.HasIndex(e => e.Taikhoan, "UQ__DANGNHAP__8C5E61046EDA7E4F").IsUnique();

            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATK");
            entity.Property(e => e.Loaitk)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasDefaultValue("user")
                .HasColumnName("LOAITK");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MATKHAU");
            entity.Property(e => e.Taikhoan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TAIKHOAN");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK_MaHD");

            entity.ToTable("HOADON");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.HinhThucThanhToan).HasMaxLength(20);
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayBan).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(20);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_HOADON_KHACHHANG");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK_HOADON_NHANVIEN");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK_MaKH");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.DiaChiKh)
                .HasMaxLength(200)
                .HasColumnName("DiaChiKH");
            entity.Property(e => e.GioiTinhKh)
                .HasMaxLength(10)
                .HasColumnName("GioiTinhKH");
            entity.Property(e => e.NamSinhKh).HasColumnName("NamSinhKH");
            entity.Property(e => e.Sdtkh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SDTKH");
            entity.Property(e => e.TenKh)
                .HasMaxLength(50)
                .HasColumnName("TenKH");
        });

        modelBuilder.Entity<Loaisp>(entity =>
        {
            entity.HasKey(e => e.MaL).HasName("PK_MaL");

            entity.ToTable("LOAISP");

            entity.Property(e => e.MaL)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TenL).HasMaxLength(30);
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK_MaNV");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaNV");
            entity.Property(e => e.Cccd)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChiNv)
                .HasMaxLength(50)
                .HasColumnName("DiaChiNV");
            entity.Property(e => e.GioiTinhNv)
                .HasMaxLength(10)
                .HasColumnName("GioiTinhNV");
            entity.Property(e => e.Luong).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATK");
            entity.Property(e => e.NgaySinhNv)
                .HasColumnType("datetime")
                .HasColumnName("NgaySinhNV");
            entity.Property(e => e.Nvl).HasColumnName("NVL");
            entity.Property(e => e.Sdtnv)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("SDTNV");
            entity.Property(e => e.TenNv)
                .HasMaxLength(30)
                .HasColumnName("TenNV");

            entity.HasOne(d => d.MatkNavigation).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.Matk)
                .HasConstraintName("FK_NHANVIEN_DANGNHAP");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK_MaSP");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaSP");
            entity.Property(e => e.ChatLieu).HasMaxLength(100);
            entity.Property(e => e.DaBan).HasDefaultValue(0);
            entity.Property(e => e.Form).HasMaxLength(20);
            entity.Property(e => e.GiaBan).HasDefaultValue(0.0);
            entity.Property(e => e.GioiTinh).HasMaxLength(4);
            entity.Property(e => e.HinhSp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HinhSP");
            entity.Property(e => e.MaL)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TenSp)
                .HasMaxLength(80)
                .HasColumnName("TenSP");
            entity.Property(e => e.ThongTinSp)
                .HasMaxLength(400)
                .HasColumnName("ThongTinSP");
            entity.Property(e => e.TinhTrang).HasMaxLength(20);

            entity.HasOne(d => d.MaLNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.MaL)
                .HasConstraintName("FK_SANPHAM_LOAISP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
