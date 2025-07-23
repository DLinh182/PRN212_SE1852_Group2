using System.Windows;

namespace QuanLyShopQuanAoWPF
{
    public partial class ThongTinSPWindow : Window
    {
        public ThongTinSPWindow()
        {
            InitializeComponent();
        }

        // Hàm này có thể nhận dữ liệu sản phẩm để hiển thị
        public void HienThiThongTin(/*Sanpham sp*/)
        {
            // TODO: Gán dữ liệu sản phẩm vào các trường giao diện
        }

        private void BtnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 