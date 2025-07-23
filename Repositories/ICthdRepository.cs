using BusinessObject;

public interface ICthdRepository {
    List<Cthd> GetAll();
    void Add(Cthd ct);
    void Update(Cthd ct);
    void Delete(int id);
    Cthd GetById(int id);
} 