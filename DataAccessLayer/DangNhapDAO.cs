using BusinessObject;
using DataAccessLayer;
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

    public void Delete(string id)
    {
        var tk = _context.Dangnhaps.Find(id);
        if (tk != null)
        {
            _context.Dangnhaps.Remove(tk);
            _context.SaveChanges();
        }
    }

    public Dangnhap GetById(string id)
    {
        return _context.Dangnhaps.Find(id);
    }

    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau)
    {
        return _context.Dangnhaps.Any(x => x.Taikhoan == taiKhoan && x.Matkhau == matKhau);
    }

    // Kiểm tra trùng tên tài khoản (username)
    public bool IsTaiKhoanExists(string taiKhoan)
    {
        return _context.Dangnhaps.Any(x => x.Taikhoan == taiKhoan);
    }
}