using BusinessObject;

public class CthdRepository : ICthdRepository
{
    private readonly CthdDAO dao = new CthdDAO();
    public List<Cthd> GetAll() {
        return dao.GetAll();
    }
    public void Add(Cthd ct) {
        dao.Add(ct);
    }
    public void Update(Cthd ct) {
        dao.Update(ct);
    }
    public List<Cthd> GetById(int id) {
        return dao.GetById(id);
    }

    public void Delete(int maHD, string maSP, string kichCo)
    {
        dao.Delete(maHD, maSP, kichCo);
    }

    public void DeleteByMaHD(int maHD)
    {
        dao.DeleteByMaHD(maHD);
    }
} 