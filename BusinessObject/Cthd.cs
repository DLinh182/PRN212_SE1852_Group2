using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Cthd
{
    public int MaHd { get; set; }

    public string MaSp { get; set; } = null!;

    public string KichCo { get; set; } = null!;

    public int? SoLuong { get; set; }

    public double? GiaBan { get; set; }

    public double? ThanhTien { get; set; }

    public virtual Hoadon MaHdNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
