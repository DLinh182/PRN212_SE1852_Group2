using BusinessObject;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

public class DangNhapDAO
{
    private readonly QlShopQuanAoContext _context;

    public DangNhapDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Dangnhap> GetAll()
    {
        return _context.Dangnhaps.ToList();
    }

    public void Add(Dangnhap tk)
    {
        _context.Dangnhaps.Add(tk);
        _context.SaveChanges();
    }

    public void Update(Dangnhap tk)
    {
        _context.Dangnhaps.Update(tk);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var dangnhap = _context.Dangnhaps.FirstOrDefault(tk => tk.Matk == maTK);
        if (dangnhap != null)
        {
            // Kiểm tra xem có nhân viên nào đang sử dụng tài khoản này không
            // Nếu có, cần xử lý: hoặc ngăn xóa, hoặc set MATK của nhân viên đó về null.
            // Giả sử mối quan hệ là cascade delete không được cấu hình,
            // hoặc bạn muốn kiểm soát việc xóa tài khoản chỉ khi không có NV nào liên kết.
            var nhanviensLinked = _context.Nhanviens.Any(nv => nv.Matk == maTK);
            if (nhanviensLinked)
            {
                throw new InvalidOperationException("Không thể xóa tài khoản này vì có nhân viên đang liên kết với nó.");
            }

            _context.Dangnhaps.Remove(dangnhap);
            _context.SaveChanges();
        }
    }

    public Dangnhap GetById(int id)
    {
        return _context.Dangnhaps.FirstOrDefault(tk => tk.Matk == maTK);
    }

    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau)
    {
        return _context.Dangnhaps.Any(x => x.Taikhoan == taiKhoan && x.Matkhau == matKhau);
    }
} 