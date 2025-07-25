using System.Windows;
using BusinessObject;
using System.Linq;

namespace QuanLyShopQuanAoWPF
{
    public partial class TaoDanhMucWindow : Window
    {
        public TaoDanhMucWindow()
        {
            InitializeComponent();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            string maDM = txtMaDM.Text.Trim();
            string tenDM = txtTenDM.Text.Trim();
            if (string.IsNullOrEmpty(maDM) || string.IsNullOrEmpty(tenDM))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã và tên danh mục!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var loaiSpService = new LoaiSpService();
            if (loaiSpService.GetAll().Any(l => l.MaL == maDM))
            {
                MessageBox.Show("Mã danh mục đã tồn tại, vui lòng nhập mã khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var loai = new Loaisp { MaL = maDM, TenL = tenDM };
            loaiSpService.Add(loai);
            MessageBox.Show("Tạo danh mục mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
} 