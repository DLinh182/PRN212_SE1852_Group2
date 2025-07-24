using BusinessObject;

public class NhanVienRepository : INhanVienRepository {
    private readonly NhanVienDAO dao = new NhanVienDAO();
    public List<Nhanvien> GetAll() {
        return dao.GetAll();
    }
    public void Add(Nhanvien nv) {
        dao.Add(nv);
    }
    public void Update(Nhanvien nv) {
        dao.Update(nv);
    }
    public void Delete(string id) {
        dao.Delete(id);
    }
    public Nhanvien GetById(string id) {
        return dao.GetById(id);
    }
} 