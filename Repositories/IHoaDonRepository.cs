using BusinessObject;

public interface IHoaDonRepository {
    List<Hoadon> GetAll();
    void Add(Hoadon hd);
    void Update(Hoadon hd);
    void Delete(int maHD);
    Hoadon GetById(int id);
    List<Hoadon> GetByTrangThai(string trangThai);
    public decimal GetTotalTongThanhToan();
    decimal GetTotalChuyenKhoan();
    decimal GetTotalTienMat();
} 