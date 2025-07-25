using System.Windows;
using BusinessObject;

namespace QuanLyShopQuanAoWPF
{
    public partial class SuaHoaDonWindow : Window
    {
        private Hoadon _hoaDon;
        public SuaHoaDonWindow(Hoadon hoaDon)
        {
            InitializeComponent();
            _hoaDon = hoaDon;
            this.DataContext = _hoaDon;
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            var hoaDonService = new HoaDonService();
            hoaDonService.Update(_hoaDon);
            MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
} 