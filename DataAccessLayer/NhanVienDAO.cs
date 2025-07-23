using BusinessObject;
using DataAccessLayer;

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

    public void Delete(int id)
    {
        var nv = _context.Nhanviens.Find(id);
        if (nv != null)
        {
            _context.Nhanviens.Remove(nv);
            _context.SaveChanges();
        }
    }

    public Nhanvien GetById(int id)
    {
        return _context.Nhanviens.Find(id);
    }
} 