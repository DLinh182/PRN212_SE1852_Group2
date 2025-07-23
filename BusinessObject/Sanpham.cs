using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Sanpham
{
    public string MaSp { get; set; } = null!;

    public string? TenSp { get; set; }

    public string? HinhSp { get; set; }

    public double? GiaBan { get; set; }

    public string? GioiTinh { get; set; }

    public string? ThongTinSp { get; set; }

    public string? ChatLieu { get; set; }

    public string? Form { get; set; }

    public int? SoLuongTon { get; set; }

    public int? DaBan { get; set; }

    public string? TinhTrang { get; set; }

    public string? MaL { get; set; }

    public virtual ICollection<Cthd> Cthds { get; set; } = new List<Cthd>();

    public virtual Loaisp? MaLNavigation { get; set; }
}
