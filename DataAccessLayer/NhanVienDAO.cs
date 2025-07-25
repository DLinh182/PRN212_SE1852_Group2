using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

public class NhanVienDAO
{
    private readonly QlShopQuanAoContext _context;

    public NhanVienDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Nhanvien> GetAll()
    {
        return _context.Nhanviens.ToList();
    }

    public void Add(Nhanvien nv)
    {
        _context.Nhanviens.Add(nv);
        _context.SaveChanges();
    }

    public void Update(Nhanvien nv)
    {
        _context.Nhanviens.Update(nv);
        _context.SaveChanges();
    }

    public void Delete(string maNV)
    {
        var nv = _context.Nhanviens.Find(maNV);
        if (nv != null)
        {
            _context.Nhanviens.Remove(nv);
            _context.SaveChanges();
        }
    }

    public Nhanvien GetById(string maNV)
    {
        return _context.Nhanviens
                           .Include(nv => nv.MatkNavigation)
                           .FirstOrDefault(nv => nv.MaNv == maNV);
    }

    public List<Nhanvien> SearchByName(string tenNV)
    {
        if (string.IsNullOrWhiteSpace(tenNV))
        {
            return GetAll();
        }
        return _context.Nhanviens
                       .Include(nv => nv.MatkNavigation)
                       .Where(nv => nv.TenNv.Contains(tenNV))
                       .ToList();
    }
} 