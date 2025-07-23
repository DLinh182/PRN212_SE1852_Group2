using BusinessObject;

public class NhanVienService : INhanVienService {
    private readonly INhanVienRepository repo = new NhanVienRepository();
    public List<Nhanvien> GetAll() {
        return repo.GetAll();
    }
    public void Add(Nhanvien nv) {
        repo.Add(nv);
    }
    public void Update(Nhanvien nv) {
        repo.Update(nv);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Nhanvien GetById(int id) {
        return repo.GetById(id);
    }
} 