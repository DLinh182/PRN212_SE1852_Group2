using BusinessObject;

public class LoaiSpService : ILoaiSpService {
    private readonly ILoaiSpRepository repo = new LoaiSpRepository();
    public List<Loaisp> GetAll() {
        return repo.GetAll();
    }
    public void Add(Loaisp loai) {
        repo.Add(loai);
    }
    public void Update(Loaisp loai) {
        repo.Update(loai);
    }
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Loaisp GetById(int id) {
        return repo.GetById(id);
    }
} 