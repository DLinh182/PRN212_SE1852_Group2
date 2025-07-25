using BusinessObject;

public interface IDangNhapRepository {
    List<Dangnhap> GetAll();
    void Add(Dangnhap tk);
    void Update(Dangnhap tk);
    bool KiemTraTaiKhoan(string taiKhoan, string matKhau);
    Dangnhap GetById(string maTK);
    void Delete(string maTK);
    bool IsTaiKhoanExists(string taiKhoan);
} 