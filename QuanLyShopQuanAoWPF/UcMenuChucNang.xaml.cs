using System.Windows;
using System.Windows.Controls;

namespace QuanLyShopQuanAoWPF
{
    public partial class UcMenuChucNang : UserControl
    {
        public UcMenuChucNang()
        {
            InitializeComponent();
            mnuTrangChu.Click += MnuTrangChu_Click;
            mnuBanHang.Click += MnuBanHang_Click;
            mnuHoaDon.Click += MnuHoaDon_Click;
            mnuSanPham.Click += MnuSanPham_Click;
            mnuNhanVien.Click += MnuNhanVien_Click;
            mnuKhachHang.Click += MnuKhachHang_Click;
            mnuNhapKho.Click += MnuNhapKho_Click;
            mnuDoanhThu.Click += MnuDoanhThu_Click;
            mnuThoat.Click += MnuThoat_Click;
        }

        private void MnuTrangChu_Click(object sender, RoutedEventArgs e)
        {
            var win = new MainWindow();
            win.Show();
        }

        private void MnuBanHang_Click(object sender, RoutedEventArgs e)
        {
            var win = new BanHangWindow();
            win.Show();
        }

        private void MnuHoaDon_Click(object sender, RoutedEventArgs e)
        {
            var win = new QuanLyHoaDonWindow();
            win.Show();
        }

        private void MnuSanPham_Click(object sender, RoutedEventArgs e)
        {
            var win = new QuanLySanPhamWindow();
            win.Show();
        }

        private void MnuNhanVien_Click(object sender, RoutedEventArgs e)
        {
            var win = new QuanLyNhanVienWindow();
            win.Show();
        }

        private void MnuKhachHang_Click(object sender, RoutedEventArgs e)
        {
            var win = new QuanLyKhachHangWindow();
            win.Show();
        }

        private void MnuNhapKho_Click(object sender, RoutedEventArgs e)
        {
            var win = new QuanLyNhapKhoWindow();
            win.Show();
        }

        private void MnuDoanhThu_Click(object sender, RoutedEventArgs e)
        {
            var win = new DoanhThuWindow();
            win.Show();
        }

        private void MnuThoat_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
} 