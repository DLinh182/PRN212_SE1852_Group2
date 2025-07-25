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

    public void Update(Dangnhap dangnhap) // dangnhap ở đây là đối tượng mới từ UI (detached)
    {
        // 1. Tìm thực thể Dangnhap đang được theo dõi bởi _context
        var existingDangnhap = _context.Dangnhaps.Find(dangnhap.Matk);

        if (existingDangnhap != null)
        {
            // 2. Nếu tìm thấy (tức là đã được theo dõi), cập nhật các thuộc tính của nó
            // Cách này an toàn và không gây lỗi tracking
            _context.Entry(existingDangnhap).CurrentValues.SetValues(dangnhap);
        }
        else
        {
            // 3. Nếu không tìm thấy trong cache của context, có thể nó là một detached entity
            // hoặc Matk bị thay đổi (mà Matk là khóa chính nên không được thay đổi).
            // Trong trường hợp cập nhật tài khoản, Matk không nên thay đổi.
            // Nếu Matk của dangnhap trùng với một entity đã có trong DB nhưng không được theo dõi bởi context này
            // thì Attach và đặt trạng thái Modified.
            _context.Dangnhaps.Update(dangnhap); // Attach entity và đặt trạng thái Modified
            // Hoặc có thể dùng: _context.Entry(dangnhap).State = EntityState.Modified;
        }
        _context.SaveChanges();
    }

    public void Delete(string maTK)
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

    public Dangnhap GetById(string maTK)
    {
        return _context.Dangnhaps.FirstOrDefault(tk => tk.Matk == maTK);
    }

    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau)
    {
        return _context.Dangnhaps.Any(x => x.Taikhoan == taiKhoan && x.Matkhau == matKhau);
    }

    // Phương thức kiểm tra tài khoản đã tồn tại chưa
    public bool IsTaiKhoanExists(string taiKhoan)
    {
        return _context.Dangnhaps.Any(tk => tk.Taikhoan == taiKhoan);
    }
} 