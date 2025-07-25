using BusinessObject;

public class NhanVienRepository : INhanVienRepository
{
    private readonly NhanVienDAO dao = new NhanVienDAO();
    public List<Nhanvien> GetAll()
    {
        return dao.GetAll();
    }
    public void Add(Nhanvien nv)
    {
        dao.Add(nv);
    }
    public void Update(Nhanvien nv)
    {
        dao.Update(nv);
    }
    public void Delete(string maNV) => dao.Delete(maNV);
    public Nhanvien GetById(string maNV) => dao.GetById(maNV);
    public List<Nhanvien> SearchByName(string tenNV) => dao.SearchByName(tenNV);
} 

