using BusinessObject;

public interface IKhachHangRepository {
    List<Khachhang> GetAll();
    void Add(Khachhang kh);
    void Update(Khachhang kh);
    void Delete(int id);
    Khachhang GetById(int id);
} 