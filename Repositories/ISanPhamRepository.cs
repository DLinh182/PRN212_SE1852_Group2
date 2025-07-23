using BusinessObject;

public interface ISanPhamRepository {
    List<Sanpham> GetAll();
    void Add(Sanpham sp);
    void Update(Sanpham sp);
    void Delete(int id);
    Sanpham GetById(int id);
} 