using BusinessObject;

public class KhachHangService : IKhachHangService {
    private readonly IKhachHangRepository repo = new KhachHangRepository();
    public List<Khachhang> GetAll() {
        return repo.GetAll();
    }
    public void Add(Khachhang kh) {
        repo.Add(kh);
    }
    public void Update(Khachhang kh) {
        repo.Update(kh);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Khachhang GetById(int id) {
        return repo.GetById(id);
    }
} 