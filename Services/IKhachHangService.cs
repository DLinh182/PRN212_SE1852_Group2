using BusinessObject;

public interface IKhachHangService {
    List<Khachhang> GetAll();
    void Add(Khachhang kh);
    void Update(Khachhang kh);
    void Delete(int id);
    Khachhang GetById(int id);
} 