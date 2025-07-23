using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Hoadon
{
    public int MaHd { get; set; }

    public string? MaNv { get; set; }

    public int? MaKh { get; set; }

    public DateTime? NgayBan { get; set; }

    public double? TongThanhToan { get; set; }

    public string? TrangThai { get; set; }

    public string? HinhThucThanhToan { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();

    public virtual Khachhang? MaKhNavigation { get; set; }

    public virtual Nhanvien? MaNvNavigation { get; set; }
}
