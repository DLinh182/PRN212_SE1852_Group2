using BusinessObject;

public interface IDangNhapRepository {
    List<Dangnhap> GetAll();
    void Add(Dangnhap tk);
    void Update(Dangnhap tk);
    void Delete(string id);
    Dangnhap GetById(string id);
    bool KiemTraTaiKhoan(string taiKhoan, string matKhau);
    bool IsTaiKhoanExists(string taiKhoan);
} 