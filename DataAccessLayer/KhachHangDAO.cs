using BusinessObject;
using DataAccessLayer;

public class KhachHangDAO
{
    private readonly QlShopQuanAoContext _context;

    public KhachHangDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Khachhang> GetAll()
    {
        return _context.Khachhangs.ToList();
    }

    public void Add(Khachhang kh)
    {
        _context.Khachhangs.Add(kh);
        _context.SaveChanges();
    }

    public void Update(Khachhang kh)
    {
        var entity = _context.Khachhangs.FirstOrDefault(x => x.MaKh == kh.MaKh);
        if (entity != null)
        {
            entity.TenKh = kh.TenKh;
            entity.GioiTinhKh = kh.GioiTinhKh;
            entity.NamSinhKh = kh.NamSinhKh;
            entity.Sdtkh = kh.Sdtkh;
            entity.DiaChiKh = kh.DiaChiKh;
            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var kh = _context.Khachhangs.Find(id);
        if (kh != null)
        {
            if (_context.Hoadons.Any(hd => hd.MaKh == id))
                throw new InvalidOperationException("Không thể xóa khách hàng đã phát sinh hóa đơn!");
            _context.Khachhangs.Remove(kh);
            _context.SaveChanges();
        }
    }

    public Khachhang GetById(int id)
    {
        return _context.Khachhangs.Find(id);
    }
} 