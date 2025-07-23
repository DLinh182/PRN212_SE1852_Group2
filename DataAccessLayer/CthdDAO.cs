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

    public void Update(Cthd ct)
    {
        _context.Cthds.Update(ct);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var ct = _context.Cthds.Find(id);
        if (ct != null)
        {
            _context.Cthds.Remove(ct);
            _context.SaveChanges();
        }
    }

    public Cthd GetById(int id)
    {
        return _context.Cthds.Find(id);
    }
} 