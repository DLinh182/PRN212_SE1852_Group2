using BusinessObject;

public interface INhanVienRepository {
    List<Nhanvien> GetAll();
    void Add(Nhanvien nv);
    void Update(Nhanvien nv);
    void Delete(string id);
    Nhanvien GetById(string id);
} 