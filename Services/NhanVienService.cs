using BusinessObject;

public class NhanVienService : INhanVienService
{
    private readonly INhanVienRepository repo = new NhanVienRepository();
    public List<Nhanvien> GetAll()
    {
        return repo.GetAll();
    }
    public void Add(Nhanvien nv)
    {
        repo.Add(nv);
    }
    public void Update(Nhanvien nv)
    {
        repo.Update(nv);
    }
    public void Delete(string maNV) => repo.Delete(maNV);
    public Nhanvien GetById(string maNV) => repo.GetById(maNV);
    public List<Nhanvien> SearchByName(string tenNV) => repo.SearchByName(tenNV);
} 

