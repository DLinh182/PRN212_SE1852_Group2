using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Loaisp
{
    public string MaL { get; set; } = null!;

    public string? TenL { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
