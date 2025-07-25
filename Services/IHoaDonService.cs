using BusinessObject;

public interface IHoaDonService {
    List<Hoadon> GetAll();
    void Add(Hoadon hd);
    void Update(Hoadon hd);
    Hoadon GetById(int id);
    bool CancelHoaDon(int maHD); // Chức năng hủy hóa đơn
    bool DeleteHoaDon(int maHD); // Chức năng xóa hóa đơn
    List<Hoadon> GetHoaDonByTrangThai(string trangThai);
    public decimal GetTotalTongThanhToan();
    decimal GetTotalChuyenKhoan();
    decimal GetTotalTienMat();
    List<Cthd> GetCTHDByMaHD(int maHD);
} 