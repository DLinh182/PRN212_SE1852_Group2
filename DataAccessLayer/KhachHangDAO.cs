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
        _context.Khachhangs.Update(kh);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var kh = _context.Khachhangs.Find(id);
        if (kh != null)
        {
            _context.Khachhangs.Remove(kh);
            _context.SaveChanges();
        }
    }

    public Khachhang GetById(int id)
    {
        return _context.Khachhangs.Find(id);
    }
} 