using BusinessObject;

public interface INhanVienService {
    List<Nhanvien> GetAll();
    void Add(Nhanvien nv);
    void Update(Nhanvien nv);
    void Delete(int id);
    Nhanvien GetById(int id);
} 