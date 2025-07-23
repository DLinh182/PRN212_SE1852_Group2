using BusinessObject;

public interface ICthdService {
    List<Cthd> GetAll();
    void Add(Cthd ct);
    void Update(Cthd ct);
    void Delete(int id);
    Cthd GetById(int id);
} 