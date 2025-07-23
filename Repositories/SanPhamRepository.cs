using BusinessObject;

public class SanPhamRepository : ISanPhamRepository {
    private readonly SanPhamDAO dao = new SanPhamDAO();
    public List<Sanpham> GetAll() {
        return dao.GetAll();
    }
    public void Add(Sanpham sp) {
        dao.Add(sp);
    }
    public void Update(Sanpham sp) {
        dao.Update(sp);
    }
    public void Delete(int id) {
        dao.Delete(id);
    }
    public Sanpham GetById(int id) {
        return dao.GetById(id);
    }
} 