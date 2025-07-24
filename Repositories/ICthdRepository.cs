using BusinessObject;

public interface ICthdRepository {
    List<Cthd> GetAll();
    void Add(Cthd ct);
    void Update(Cthd ct);
    public void Delete(int maHD, string maSP, string kichCo);
    public void DeleteByMaHD(int maHD);
    List<Cthd> GetById(int id);
} 