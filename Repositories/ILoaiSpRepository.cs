using BusinessObject;

public interface ILoaiSpRepository
{
    List<Loaisp> GetAll();
    void Add(Loaisp loai);
    void Update(Loaisp loai);
    void Delete(string id);
    Loaisp GetById(int id);
}