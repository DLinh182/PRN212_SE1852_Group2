using BusinessObject;
using DataAccessLayer;

public class SanPhamDAO
{
    private readonly QlShopQuanAoContext _context;

    public SanPhamDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Sanpham> GetAll()
    {
        return _context.Sanphams.ToList();
    }

    public void Add(Sanpham sp)
    {
        _context.Sanphams.Add(sp);
        _context.SaveChanges();
    }

    public void Update(Sanpham sp)
    {
        _context.Sanphams.Update(sp);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var sp = _context.Sanphams.Find(id);
        if (sp != null)
        {
            _context.Sanphams.Remove(sp);
            _context.SaveChanges();
        }
    }

    public List<Sanpham> GetById(string id)
    {
        return _context.Sanphams.Where(ct => ct.MaSp == id).ToList();
    }

    public List<Sanpham> SearchByName(string tenSP)
    {
        if (string.IsNullOrWhiteSpace(tenSP))
        {
            return _context.Sanphams.ToList(); // Trả về tất cả nếu chuỗi tìm kiếm rỗng
        }
        return _context.Sanphams
                       .Where(sp => sp.TenSp.Contains(tenSP))
                       .ToList();
    }
} 