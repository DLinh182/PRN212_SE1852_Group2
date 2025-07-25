using BusinessObject;

public class DangNhapService : IDangNhapService {
    private readonly IDangNhapRepository repo = new DangNhapRepository();
    public List<Dangnhap> GetAll() {
        return repo.GetAll();
    }
    public void Add(Dangnhap tk) {
        repo.Add(tk);
    }
    public void Update(Dangnhap tk) {
        repo.Update(tk);
    }
    public void Delete(string maTK) => repo.Delete(maTK);
    public Dangnhap GetById(string maTK) => repo.GetById(maTK);
    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau) {
        return repo.KiemTraTaiKhoan(taiKhoan, matKhau);
    }
    public bool IsTaiKhoanExists(string taiKhoan) => repo.IsTaiKhoanExists(taiKhoan);
} 