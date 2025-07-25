using BusinessObject;

public interface INhanVienRepository
{
    List<Nhanvien> GetAll();
    Nhanvien GetById(string maNV);
    void Add(Nhanvien nhanvien);
    void Update(Nhanvien nhanvien);
    void Delete(string maNV);
    List<Nhanvien> SearchByName(string tenNV);
}