using BusinessObject;

public interface ILoaiSpService {
    List<Loaisp> GetAll();
    void Add(Loaisp loai);
    void Update(Loaisp loai);
    void Delete(int id);
    Loaisp GetById(int id);
} 