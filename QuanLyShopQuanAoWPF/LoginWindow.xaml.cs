using System.Windows;
using System.Linq; // Added for FirstOrDefault

namespace QuanLyShopQuanAoWPF
{
    public partial class LoginWindow : Window
    {
        private readonly IDangNhapService _dangNhapService = new DangNhapService();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnDN_Click(object sender, RoutedEventArgs e)
        {
            string taiKhoan = txtTK.Text.Trim();
            string matKhau = txtMK.Password;
            if (string.IsNullOrEmpty(taiKhoan))
            {
                MessageBox.Show("Bạn chưa nhập tài khoản!");
                return;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu!");
                return;
            }
            bool hopLe = _dangNhapService.KiemTraTaiKhoan(taiKhoan, matKhau);
            if (hopLe)
            {
                var dangNhap = _dangNhapService.GetAll().FirstOrDefault(x => x.Taikhoan == taiKhoan);
                string role = dangNhap?.Loaitk ?? "user";
                MessageBox.Show("Đăng nhập thành công!");
                var main = new MainWindow(role); // Truyền quyền sang MainWindow
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!");
            }
        }
    }
} 