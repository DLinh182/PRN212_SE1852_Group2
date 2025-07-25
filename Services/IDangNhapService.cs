using BusinessObject;

public interface IDangNhapService {
    List<Dangnhap> GetAll();
    void Add(Dangnhap tk);
    void Update(Dangnhap tk);
    void Delete(int id);
    Dangnhap GetById(int id);
    bool KiemTraTaiKhoan(string taiKhoan, string matKhau);
} 