using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Dangnhap
{
    public string Matk { get; set; } = null!;

    public string Taikhoan { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string? Loaitk { get; set; }

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
