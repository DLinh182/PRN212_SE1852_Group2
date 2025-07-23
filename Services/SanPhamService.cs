using BusinessObject;

public class SanPhamService : ISanPhamService {
    private readonly ISanPhamRepository repo = new SanPhamRepository();
    public List<Sanpham> GetAll() {
        return repo.GetAll();
    }
    public void Add(Sanpham sp) {
        repo.Add(sp);
    }
    public void Update(Sanpham sp) {
        repo.Update(sp);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Sanpham GetById(int id) {
        return repo.GetById(id);
    }
} 