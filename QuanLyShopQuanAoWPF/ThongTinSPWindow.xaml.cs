using System;
using System.Windows;
using BusinessObject;

namespace QuanLyShopQuanAoWPF
{
    public partial class ThongTinSPWindow : Window
    {
        private Action<string, int> _onAddToCart;
        private Sanpham _sp;

        public ThongTinSPWindow()
        {
            InitializeComponent();
            btnThemVaoGioHang.Click += BtnThemVaoGioHang_Click;
            btnHuy.Click += BtnDong_Click;
        }

        // Hàm này nhận dữ liệu sản phẩm và callback khi thêm vào giỏ
        public void HienThiThongTin(Sanpham sp, Action<string, int> onAddToCart)
        {
            _sp = sp;
            _onAddToCart = onAddToCart;
            lbTenSP.Text = sp.TenSp;
            lbGiaSP.Text = (sp.GiaBan ?? 0).ToString("N0") + "đ";
            lb_SP_SC.Text = $"Sản phẩm sẵn có: {sp.SoLuongTon ?? 0}";
            lbTTSP.Text = sp.ThongTinSp ?? "";
            // TODO: Hiển thị ảnh nếu có
        }

        private void BtnThemVaoGioHang_Click(object sender, RoutedEventArgs e)
        {
            // Lấy size được chọn
            string kichCo = "M";
            if (this.FindName("RadioButtonS") is System.Windows.Controls.RadioButton rS && rS.IsChecked == true) kichCo = "S";
            if (this.FindName("RadioButtonM") is System.Windows.Controls.RadioButton rM && rM.IsChecked == true) kichCo = "M";
            if (this.FindName("RadioButtonL") is System.Windows.Controls.RadioButton rL && rL.IsChecked == true) kichCo = "L";
            if (this.FindName("RadioButtonXL") is System.Windows.Controls.RadioButton rXL && rXL.IsChecked == true) kichCo = "XL";
            if (this.FindName("RadioButtonXXL") is System.Windows.Controls.RadioButton rXXL && rXXL.IsChecked == true) kichCo = "XXL";
            // Lấy số lượng
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (soLuong > (_sp.SoLuongTon ?? 0))
            {
                MessageBox.Show("Số lượng vượt quá tồn kho!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _onAddToCart?.Invoke(kichCo, soLuong);
            this.Close();
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 