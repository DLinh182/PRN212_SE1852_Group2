using BusinessObject;
using DataAccessLayer;
using System.Collections.Generic;

public class LoaiSpRepository : ILoaiSpRepository
{
    private readonly LoaiSpDAO dao = new LoaiSpDAO();

    public List<Loaisp> GetAll()
    {
        return dao.GetAll();
    }

    public void Add(Loaisp loai)
    {
        dao.Add(loai);
    }

    public void Update(Loaisp loai)
    {
        dao.Update(loai);
    }

    public void Delete(int id)
    {
        dao.Delete(id);
    }

    public Loaisp GetById(int id)
    {
        return dao.GetById(id);
    }
} 