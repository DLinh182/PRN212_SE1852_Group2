using BusinessObject;

public class CthdRepository : ICthdRepository {
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
    public void Delete(int id) {
        dao.Delete(id);
    }
    public Cthd GetById(int id) {
        return dao.GetById(id);
    }
} 