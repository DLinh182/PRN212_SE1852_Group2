using BusinessObject;
using DataAccessLayer;

public class HoaDonDAO
{
    private readonly QlShopQuanAoContext _context;

    public HoaDonDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Hoadon> GetAll()
    {
        return _context.Hoadons.ToList();
    }

    public void Add(Hoadon hd)
    {
        _context.Hoadons.Add(hd);
        _context.SaveChanges();
    }

    public void Update(Hoadon hoaDon)
    {
        var existingHoaDon = _context.Hoadons.Find(hoaDon.MaHd);
        if (existingHoaDon != null)
        {
            _context.Entry(existingHoaDon).CurrentValues.SetValues(hoaDon);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception($"Hóa đơn với mã {hoaDon.MaHd} không tồn tại.");
        }
    }

    public void Delete(int maHD)
    {
        var hoaDon = _context.Hoadons.Find(maHD);
        if (hoaDon != null)
        {
            _context.Hoadons.Remove(hoaDon);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception($"Hóa đơn với mã {maHD} không tồn tại.");
        }
    }

    public List<Hoadon> GetByTrangThai(string trangThai)
    {
        if (string.IsNullOrEmpty(trangThai) || trangThai == "Tất cả")
        {
            return _context.Hoadons.ToList();
        }
        return _context.Hoadons.Where(hd => hd.TrangThai == trangThai).ToList();
    }

    public decimal GetTotalTongThanhToan()
    {
        return _context.Hoadons.Sum(hd => (decimal?)hd.TongThanhToan) ?? 0;
    }

    public decimal GetTotalByPaymentMethod(string paymentMethod)
    {
        return _context.Hoadons.Where(hd => hd.HinhThucThanhToan == paymentMethod)
                             .Sum(hd => (decimal?)hd.TongThanhToan) ?? 0;
    }

    public Hoadon GetById(int id)
    {
        return _context.Hoadons.Find(id);
    }
} 