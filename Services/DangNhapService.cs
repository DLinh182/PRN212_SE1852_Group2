using BusinessObject;

public class DangNhapService : IDangNhapService 
{
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
    public void Delete(int id) {
        repo.Delete(id);
    }
    public Dangnhap GetById(int id) {
        return repo.GetById(id);
    }
    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau) {
        return repo.KiemTraTaiKhoan(taiKhoan, matKhau);
    }
} 