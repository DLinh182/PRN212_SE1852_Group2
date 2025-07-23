using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Nhanvien
{
    public string MaNv { get; set; } = null!;

    public string? TenNv { get; set; }

    public string? Sdtnv { get; set; }

    public DateTime? NgaySinhNv { get; set; }

    public string? DiaChiNv { get; set; }

    public string? GioiTinhNv { get; set; }

    public string? Cccd { get; set; }

    public int? Nvl { get; set; }

    public decimal? Luong { get; set; }

    public string? Matk { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual Dangnhap? MatkNavigation { get; set; }
}
