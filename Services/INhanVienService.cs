using BusinessObject;

public interface INhanVienService {
    List<Nhanvien> GetAll();
    void Add(Nhanvien nv);
    void Update(Nhanvien nv);
    void Delete(string id);
    Nhanvien GetById(string id);
} 