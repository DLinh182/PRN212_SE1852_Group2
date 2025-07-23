using BusinessObject;

public interface IHoaDonRepository {
    List<Hoadon> GetAll();
    void Add(Hoadon hd);
    void Update(Hoadon hd);
    void Delete(int id);
    Hoadon GetById(int id);
} 