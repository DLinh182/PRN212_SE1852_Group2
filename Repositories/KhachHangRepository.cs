using BusinessObject;

public class KhachHangRepository : IKhachHangRepository {
    private readonly KhachHangDAO dao = new KhachHangDAO();
    public List<Khachhang> GetAll() {
        return dao.GetAll();
    }
    public void Add(Khachhang kh) {
        dao.Add(kh);
    }
    public void Update(Khachhang kh) {
        dao.Update(kh);
    }
    public void Delete(int id) {
        dao.Delete(id);
    }
    public Khachhang GetById(int id) {
        return dao.GetById(id);
    }
} 