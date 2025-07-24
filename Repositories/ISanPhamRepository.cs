using BusinessObject;

public interface ISanPhamRepository {
    List<Sanpham> GetAll();
    void Add(Sanpham sp);
    void Update(Sanpham sp);
    void Delete(string id);
    List<Sanpham> GetById(string id);
    List<Sanpham> SearchByName(string tenSP);
} 