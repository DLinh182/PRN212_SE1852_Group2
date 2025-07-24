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
    public void Delete(string id) {
        dao.Delete(id);
    }
    public List<Sanpham> GetById(string id) {
        return dao.GetById(id);
    }

    public List<Sanpham> SearchByName(string tenSP)
    {
        return dao.SearchByName(tenSP); // Triển khai bằng cách gọi DAO
    }
} 