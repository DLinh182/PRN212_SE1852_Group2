using System.Windows;

namespace QuanLyShopQuanAoWPF
{
    public partial class LoginWindow : Window
    {
        private readonly IDangNhapService _dangNhapService = new DangNhapService();
        public LoginWindow()
        {
            InitializeComponent();
            chkHienMatKhau.Checked += (s, e) => txtMatKhau.Visibility = Visibility.Collapsed;
            chkHienMatKhau.Unchecked += (s, e) => txtMatKhau.Visibility = Visibility.Visible;
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Password;
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
                MessageBox.Show("Đăng nhập thành công!");
                // TODO: Mở giao diện chính
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!");
            }
        }
    }
} 