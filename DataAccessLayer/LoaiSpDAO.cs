using BusinessObject;
using DataAccessLayer;

public class LoaiSpDAO
{
    private readonly QlShopQuanAoContext _context;

    public LoaiSpDAO()
    {
        _context = new QlShopQuanAoContext();
    }

    public List<Loaisp> GetAll()
    {
        return _context.Loaisps.ToList();
    }

    public void Add(Loaisp loai)
    {
        _context.Loaisps.Add(loai);
        _context.SaveChanges();
    }

    public void Update(Loaisp loai)
    {
        _context.Loaisps.Update(loai);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var loai = _context.Loaisps.Find(id);
        if (loai != null)
        {
            _context.Loaisps.Remove(loai);
            _context.SaveChanges();
        }
    }

    public Loaisp GetById(int id)
    {
        return _context.Loaisps.Find(id);
    }
} 