using BusinessObject;

public class DangNhapRepository : IDangNhapRepository {
    private readonly DangNhapDAO dao = new DangNhapDAO();
    public List<Dangnhap> GetAll() {
        return dao.GetAll();
    }
    public void Add(Dangnhap tk) {
        dao.Add(tk);
    }
    public void Update(Dangnhap tk) {
        dao.Update(tk);
    }
    public void Delete(string id) {
        dao.Delete(id);
    }
    public Dangnhap GetById(string id) {
        return dao.GetById(id);
    }
    public bool KiemTraTaiKhoan(string taiKhoan, string matKhau) {
        return dao.KiemTraTaiKhoan(taiKhoan, matKhau);
    }
    public bool IsTaiKhoanExists(string taiKhoan) => dao.IsTaiKhoanExists(taiKhoan);
} 