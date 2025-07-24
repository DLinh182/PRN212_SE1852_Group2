using System.Windows;
using System.Windows.Controls;
using BusinessObject;

namespace QuanLyShopQuanAoWPF
{
    public partial class DoanhThuWindow : Window
    {
        private readonly IHoaDonService hoaDonService = new HoaDonService();
        private readonly ICthdService cthdService = new CthdService();

        public DoanhThuWindow()
        {
            InitializeComponent();
            btnXem.Click += BtnXem_Click;
            dgDoanhThu.SelectionChanged += dgDoanhThu_SelectionChanged;
        }

        private void BtnXem_Click(object sender, RoutedEventArgs e)
        {
            DateTime? tuNgay = dpTuNgay.SelectedDate;
            DateTime? denNgay = dpDenNgay.SelectedDate;
            string maDon = txtMaDonHang.Text.Trim();
            string maKhach = txtMaKhachHang.Text.Trim();

            var dsHoaDon = hoaDonService.GetAll();
            if (tuNgay.HasValue)
                dsHoaDon = dsHoaDon.Where(hd => hd.NgayBan >= tuNgay.Value).ToList();
            if (denNgay.HasValue)
                dsHoaDon = dsHoaDon.Where(hd => hd.NgayBan <= denNgay.Value).ToList();
            if (!string.IsNullOrEmpty(maDon))
                dsHoaDon = dsHoaDon.Where(hd => hd.MaHd.ToString().Contains(maDon)).ToList();
            if (!string.IsNullOrEmpty(maKhach))
                dsHoaDon = dsHoaDon.Where(hd => hd.MaKh != null && hd.MaKh.ToString().Contains(maKhach)).ToList();

            dgDoanhThu.ItemsSource = dsHoaDon;
            lbTongTien.Text = dsHoaDon.Sum(hd => hd.TongThanhToan ?? 0).ToString("N0");
            dgChiTietHoaDon.ItemsSource = null;
        }

        private void dgDoanhThu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDoanhThu.SelectedItem is Hoadon hd)
            {
                var dsChiTiet = cthdService.GetAll().Where(ct => ct.MaHd == hd.MaHd).ToList();
                dgChiTietHoaDon.ItemsSource = dsChiTiet;
            }
            else
            {
                dgChiTietHoaDon.ItemsSource = null;
            }
        }
    }
} 