using System.Windows;
using System.Windows.Controls;
using BusinessObject;

namespace QuanLyShopQuanAoWPF
{
    public partial class QuanLyHoaDonWindow : Window
    {
        private readonly IHoaDonService hoaDonService = new HoaDonService();
        private readonly ICthdService cthdService = new CthdService();
        private Hoadon _selectedHoaDon;
        public QuanLyHoaDonWindow()
        {
            InitializeComponent();
            LoadInitialData();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            // Mở một cửa sổ mới để thêm hóa đơn
            MessageBox.Show("Chức năng thêm hóa đơn cần một form nhập liệu riêng.", "Thông báo");
            // Ví dụ:
            // var themHoaDonWindow = new ThemHoaDonWindow();
            // if (themHoaDonWindow.ShowDialog() == true)
            // {
            //     try
            //     {
            //         // Lấy dữ liệu từ cửa sổ thêm và gọi service để thêm
            //         hoaDonService.Add(themHoaDonWindow.NewHoaDonData); // Giả sử ThemHoaDonWindow có thuộc tính NewHoaDonData
            //         MessageBox.Show("Thêm hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            //         LoadHoaDonData();
            //         LoadThongKeDoanhThu();
            //     }
            //     catch (Exception ex)
            //     {
            //         MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            //     }
            // }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHoaDon != null)
            {
                // Mở một cửa sổ mới để sửa hóa đơn, truyền _selectedHoaDon vào
                MessageBox.Show("Chức năng sửa hóa đơn cần một form nhập liệu riêng với dữ liệu đã chọn.", "Thông báo");
                // Ví dụ:
                // var suaHoaDonWindow = new SuaHoaDonWindow(_selectedHoaDon);
                // if (suaHoaDonWindow.ShowDialog() == true)
                // {
                //     try
                //     {
                //         hoaDonService.Update(_selectedHoaDon); // Hoặc lấy dữ liệu đã sửa từ suaHoaDonWindow
                //         MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                //         LoadHoaDonData();
                //         LoadThongKeDoanhThu();
                //     }
                //     catch (Exception ex)
                //     {
                //         MessageBox.Show($"Lỗi khi cập nhật hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                //     }
                // }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHoaDon != null)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn có mã '{_selectedHoaDon.MaHd}' và tất cả chi tiết liên quan không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Lưu ý: Việc xóa hóa đơn cũng nên xóa các CTHD liên quan
                        // Trong HoaDonService, phương thức Delete đã được thiết kế để xóa CTHD trước
                        if (hoaDonService.DeleteHoaDon(_selectedHoaDon.MaHd))
                        {
                            MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadHoaDonData();
                            LoadThongKeDoanhThu();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa hóa đơn. Có thể có lỗi hoặc ràng buộc dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedHoaDon != null)
            {
                if (_selectedHoaDon.TrangThai == "Đã Hủy" || _selectedHoaDon.TrangThai == "Đã thanh toán")
                {
                    MessageBox.Show("Không thể hủy hóa đơn đã hủy hoặc đã thanh toán.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn hủy hóa đơn có mã '{_selectedHoaDon.MaHd}' không?", "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (hoaDonService.CancelHoaDon(_selectedHoaDon.MaHd))
                        {
                            MessageBox.Show("Hủy hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadHoaDonData();
                            LoadThongKeDoanhThu();
                        }
                        else
                        {
                            MessageBox.Show("Không thể hủy hóa đơn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi hủy hóa đơn: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để hủy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadInitialData()
        {
            // Tải danh sách trạng thái vào ComboBox
            cboTrangThai.ItemsSource = new List<string> { "Tất cả", "Đã thanh toán", "Chưa thanh toán", "Đã Hủy" };
            cboTrangThai.SelectedItem = "Tất cả"; // Chọn mặc định "Tất cả"

            LoadHoaDonData(); // Tải dữ liệu hóa đơn ban đầu
            LoadThongKeDoanhThu(); // Tải thống kê doanh thu
        }

        private void LoadHoaDonData()
        {
            List<Hoadon> hoaDons;
            string selectedStatus = cboTrangThai.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedStatus) || selectedStatus == "Tất cả")
            {
                hoaDons = hoaDonService.GetAll();
            }
            else
            {
                hoaDons = hoaDonService.GetHoaDonByTrangThai(selectedStatus);
            }

            // Gán dữ liệu vào DataGrid
            dgHoaDon.ItemsSource = hoaDons;
            dgCTHD.ItemsSource = null; // Xóa chi tiết hóa đơn cũ khi tải lại danh sách
            _selectedHoaDon = null;
            UpdateControlStates(); // Cập nhật trạng thái các nút
        }

        private void LoadThongKeDoanhThu()
        {
            lbTongDoanhThu.Text = hoaDonService.GetTotalTongThanhToan().ToString("N0") + " VNĐ";
            lbCK.Text = hoaDonService.GetTotalChuyenKhoan().ToString("N0") + " VNĐ";
            lbTM.Text = hoaDonService.GetTotalTienMat().ToString("N0") + " VNĐ";
        }

        private void UpdateControlStates()
        {
            // Cập nhật trạng thái của các nút dựa trên _selectedHoaDon
            bool isHoaDonSelected = (_selectedHoaDon != null);

            //btnSua.IsEnabled = isHoaDonSelected;
            //btnXoa.IsEnabled = isHoaDonSelected;
            //btnHuy.IsEnabled = isHoaDonSelected && _selectedHoaDon.TrangThai != "Đã Hủy" && _selectedHoaDon.TrangThai != "Đã thanh toán";
            //btnLuu.IsEnabled = false; // Nút lưu chỉ bật khi có chỉnh sửa (có thể thêm logic này sau)
        }

        private void DgHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedHoaDon = dgHoaDon.SelectedItem as Hoadon;
            if (_selectedHoaDon != null)
            {
                // Tải chi tiết hóa đơn tương ứng
                dgCTHD.ItemsSource = cthdService.GetById(_selectedHoaDon.MaHd);
            }
            else
            {
                dgCTHD.ItemsSource = null; // Không có hóa đơn nào được chọn
            }
            UpdateControlStates(); // Cập nhật trạng thái các nút khi chọn hóa đơn
        }

        private void CboTrangThai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHoaDonData(); // Tải lại dữ liệu khi trạng thái lọc thay đổi
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset trạng thái các nút công cụ
            UpdateControlStates();
            MessageBox.Show("Các nút công cụ đã được reset trạng thái.", "Thông báo");

        }

        private void btnResetDuLieu_Click(object sender, RoutedEventArgs e)
        {
            cboTrangThai.SelectedItem = "Tất cả";
            // Xóa nội dung ô tìm kiếm (nếu có)
            // txtTimKiem.Text = string.Empty; // Nếu bạn có TextBox tìm kiếm
            LoadHoaDonData();
            LoadThongKeDoanhThu();
            MessageBox.Show("Dữ liệu đã được reset.", "Thông báo");
        }

        private void cboTrangThai_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            LoadHoaDonData();
        }
    }
} 