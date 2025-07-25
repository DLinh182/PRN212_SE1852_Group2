using BusinessObject;

public class HoaDonService : IHoaDonService
{
    private readonly IHoaDonRepository repo = new HoaDonRepository();
    private readonly ICthdRepository cthdRepository = new CthdRepository();
    public List<Hoadon> GetAll() 
    {
        return repo.GetAll();
    }
    public void Add(Hoadon hd) 
    {
        repo.Add(hd);
    }
    public void Update(Hoadon hd) 
    {
        repo.Update(hd);
    }
    public Hoadon GetById(int id) 
    {
        return repo.GetById(id);
    }

    public List<Hoadon> GetHoaDonByTrangThai(string trangThai)
    {
        return repo.GetByTrangThai(trangThai);
    }

    public decimal GetTotalTongThanhToan()
    {
        return repo.GetTotalTongThanhToan();
    }

    public decimal GetTotalChuyenKhoan()
    {
        return repo.GetTotalChuyenKhoan();
    }

    public decimal GetTotalTienMat()
    {
        return repo.GetTotalTienMat();
    }

    public bool CancelHoaDon(int maHD)
    {
        try
        {
            var hoaDon = repo.GetById(maHD);
            if (hoaDon != null)
            {
                if (hoaDon.TrangThai == "Đã Hủy" || hoaDon.TrangThai == "Đã thanh toán")
                {
                    // Không cho phép hủy hóa đơn đã hủy hoặc đã thanh toán
                    return false;
                }
                hoaDon.TrangThai = "Đã Hủy"; // Cập nhật trạng thái
                repo.Update(hoaDon);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi hủy hóa đơn (Service): " + ex.Message);
            return false;
        }
    }

    public bool DeleteHoaDon(int maHD)
    {
        try
        {
            // Logic nghiệp vụ: Cần xóa CTHD trước rồi mới xóa HOADON
            cthdRepository.DeleteByMaHD(maHD);
            repo.Delete(maHD);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi xóa hóa đơn (Service): " + ex.Message);
            return false;
        }
    }

    public List<Cthd> GetCTHDByMaHD(int maHD)
    {
        return cthdRepository.GetById(maHD);
    }
} 