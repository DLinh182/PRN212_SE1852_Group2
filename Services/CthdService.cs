using BusinessObject;

public class CthdService : ICthdService {
    private readonly ICthdRepository repo = new CthdRepository();
    public List<Cthd> GetAll() {
        return repo.GetAll();
    }
    public void Add(Cthd ct) {
        repo.Add(ct);
    }
    public void Update(Cthd ct) {
        repo.Update(ct);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Cthd GetById(int id) {
        return repo.GetById(id);
    }
} 