using BusinessObject;
using DataAccessLayer;

public class CthdDAO
{
    private readonly QlShopQuanAoContext _context;

    public CthdDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Cthd> GetAll()
    {
        return _context.Cthds.ToList();
    }

    public void Add(Cthd ct)
    {
        _context.Cthds.Add(ct);
        _context.SaveChanges();
    }

    public void Update(Cthd cthd)
    {
        var existingCthd = _context.Cthds.Find(cthd.MaHd, cthd.MaSp, cthd.KichCo); // Giả sử khóa chính là MaHD, MaSP, KichCo
        if (existingCthd != null)
        {
            _context.Entry(existingCthd).CurrentValues.SetValues(cthd);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception($"Chi tiết hóa đơn không tồn tại.");
        }
    }

    public void Delete(int maHD, string maSP, string kichCo)
    {
        var cthd = _context.Cthds.Find(maHD, maSP, kichCo); // Giả sử khóa chính là MaHD, MaSP, KichCo
        if (cthd != null)
        {
            _context.Cthds.Remove(cthd);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception($"Chi tiết hóa đơn không tồn tại.");
        }
    }

    public void DeleteByMaHD(int maHD)
    {
        var cthds = _context.Cthds.Where(ct => ct.MaHd == maHD).ToList();
        if (cthds.Any())
        {
            _context.Cthds.RemoveRange(cthds);
            _context.SaveChanges();
        }
    }

    public List<Cthd> GetById(int MaHD)
    {
        return _context.Cthds.Where(ct => ct.MaHd == MaHD).ToList();
    }


} 