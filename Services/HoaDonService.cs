using BusinessObject;

public class HoaDonService : IHoaDonService {
    private readonly IHoaDonRepository repo = new HoaDonRepository();
    public List<Hoadon> GetAll() {
        return repo.GetAll();
    }
    public void Add(Hoadon hd) {
        repo.Add(hd);
    }
    public void Update(Hoadon hd) {
        repo.Update(hd);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Hoadon GetById(int id) {
        return repo.GetById(id);
    }
} 