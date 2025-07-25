using BusinessObject;

public interface INhanVienService
{
    List<Nhanvien> GetAll();
    void Add(Nhanvien nv);
    void Update(Nhanvien nv);
    void Delete(string maNV);
    Nhanvien GetById(string maNV);
    List<Nhanvien> SearchByName(string tenNV);
}