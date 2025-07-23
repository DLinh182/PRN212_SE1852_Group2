using BusinessObject;

public class HoaDonRepository : IHoaDonRepository {
    private readonly HoaDonDAO dao = new HoaDonDAO();
    public List<Hoadon> GetAll() {
        return dao.GetAll();
    }
    public void Add(Hoadon hd) {
        dao.Add(hd);
    }
    public void Update(Hoadon hd) {
        dao.Update(hd);
    }
    public void Delete(int id) {
        dao.Delete(id);
    }
    public Hoadon GetById(int id) {
        return dao.GetById(id);
    }
} 