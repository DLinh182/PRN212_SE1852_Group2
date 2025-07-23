using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Khachhang
{
    public int MaKh { get; set; }

    public string? TenKh { get; set; }

    public string? Sdtkh { get; set; }

    public string? DiaChiKh { get; set; }

    public string? GioiTinhKh { get; set; }

    public int? NamSinhKh { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
