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

    public void Update(Hoadon hd)
    {
        _context.Hoadons.Update(hd);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var hd = _context.Hoadons.Find(id);
        if (hd != null)
        {
            _context.Hoadons.Remove(hd);
            _context.SaveChanges();
        }
    }

    public Hoadon GetById(int id)
    {
        return _context.Hoadons.Find(id);
    }
} 