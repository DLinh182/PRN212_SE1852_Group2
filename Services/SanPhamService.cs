using BusinessObject;

public class SanPhamService : ISanPhamService
{
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
    public void Delete(string id) {
        repo.Delete(id);
    }
    public List<Sanpham> GetById(string id) {
        return repo.GetById(id);
    }
    public List<Sanpham> SearchByName(string tenSP)
    {
        return repo.SearchByName(tenSP); // Triển khai bằng cách gọi Repository
    }
} 