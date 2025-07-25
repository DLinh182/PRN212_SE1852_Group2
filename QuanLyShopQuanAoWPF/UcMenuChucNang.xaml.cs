using System.Windows;
using System.Windows.Controls;

namespace QuanLyShopQuanAoWPF
{
    public partial class UcMenuChucNang : UserControl
    {
        public UcMenuChucNang() // Constructor mặc định để XAML khởi tạo
        {
            InitializeComponent();
            // Gán sự kiện ở đây nếu cần
            mnuTrangChu.Click += MnuTrangChu_Click;
            mnuBanHang.Click += MnuBanHang_Click;
            mnuSanPham.Click += MnuSanPham_Click;
            mnuNhanVien.Click += MnuNhanVien_Click;
            mnuKhachHang.Click += MnuKhachHang_Click;
            mnuDoanhThu.Click += MnuDoanhThu_Click;
            mnuThoat.Click += MnuThoat_Click;
        }

        private string _role;
        public void SetRole(string role)
        {
            if (role == "user")
                mnuNhanVien.Visibility = Visibility.Collapsed;
            else
                mnuNhanVien.Visibility = Visibility.Visible;
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