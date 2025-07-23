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

    public void Delete(int id)
    {
        var sp = _context.Sanphams.Find(id);
        if (sp != null)
        {
            _context.Sanphams.Remove(sp);
            _context.SaveChanges();
        }
    }

    public Sanpham GetById(int id)
    {
        return _context.Sanphams.Find(id);
    }
} 