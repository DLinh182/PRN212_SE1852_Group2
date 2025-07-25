using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore; // Đảm bảo namespace này đúng


namespace QuanLyShopQuanAoWPF
{
    public partial class QuanLyNhanVienWindow : Window
    {
        // Khai báo các Service
        private readonly NhanVienService _nhanvienService = new NhanVienService(); // Đổi interface thành class
        private readonly DangNhapService _dangnhapService = new DangNhapService(); // Đổi interface thành class
        // Biến để lưu trạng thái đang thêm/sửa nhân viên
        private bool _isAddingNhanvien = false;
        private bool _isUpdatingNhanvien = false;

        // Biến để lưu đối tượng đang được chọn/thao tác
        private Nhanvien? _selectedNhanvien; // Dùng kiểu nullable để tránh lỗi null reference
        private Dangnhap? _selectedDangnhap; // Dùng kiểu nullable

        public QuanLyNhanVienWindow()
        {
            InitializeComponent();
            // Khởi tạo các controls và load dữ liệu khi cửa sổ được tải hoàn toàn
            Loaded += QuanLyNhanVienWindow_Loaded;
        }

        private void QuanLyNhanVienWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            // Thiết lập ComboBox Giới tính
            cboGioiTinh.ItemsSource = new List<string> { "Nam", "Nữ", "Khác" };
            cboGioiTinh.SelectedIndex = 0; // Chọn mặc định "Nam"

            // Thiết lập ComboBox Loại tài khoản
            cboLoaiTK.ItemsSource = new List<string> { "admin", "user" };
            cboLoaiTK.SelectedIndex = 0; // Chọn mặc định "Nhân viên"

            SetInputEnabled(false);
            btnLuu.IsEnabled = false;
            txtMaNV.IsReadOnly = true;
            txtMaTK.IsReadOnly = true; // Mã TK luôn chỉ đọc

            // Gán sự kiện cho các nút
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click; // Đổi từ BtnLuuTK_Click sang BtnLuu_Click
            btnReset.Click += BtnReset_Click;
            btnTimKiem.Click += BtnTimKiem_Click;

            // Bỏ gán sự kiện cho các nút quản lý TK riêng (nếu có) vì đã loại bỏ chúng trong XAML
            // btnXoaTK.Click += BtnXoaTK_Click;
            // btnSuaTK.Click += BtnSuaTK_Click;

            // Gán sự kiện khi chọn một dòng trên DataGrid
            dgNhanVien.SelectionChanged += DgNhanVien_SelectionChanged;
            dgTaiKhoan.SelectionChanged += DgTaiKhoan_SelectionChanged; // Vẫn giữ để hiển thị (chỉ đọc)

            // Vô hiệu hóa các trường nhập liệu và nút Lưu ban đầu
            SetInputEnabled(false); ;
            btnLuu.IsEnabled = false;
            txtMaNV.IsReadOnly = true; // Mã NV chỉ đọc khi sửa/hiển thị
            txtMaTK.IsReadOnly = true; // Mã TK chỉ đọc khi sửa/hiển thị
        }

        private void LoadData()
        {
            try
            {
                // Load danh sách nhân viên vào DataGrid NhanVien
                dgNhanVien.ItemsSource = _nhanvienService.GetAll();

                // Load danh sách tài khoản vào DataGrid TaiKhoan
                dgTaiKhoan.ItemsSource = _dangnhapService.GetAll();

                // Reset trạng thái và giao diện
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Error loading data: {ex.ToString()}"); // Để debug lỗi chi tiết hơn
            }
        }

        private void ResetFormState()
        {
            ClearInputs(); // Gọi hàm xóa tất cả inputs
            SetInputEnabled(false); // Vô hiệu hóa tất cả inputs
            btnLuu.IsEnabled = false;
            txtMaNV.IsReadOnly = true;
            txtMaTK.IsReadOnly = true; // Mã TK cũng chỉ đọc khi reset

            // Bật lại các nút thao tác chính
            btnThem.IsEnabled = true;
            btnSua.IsEnabled = true;
            btnXoa.IsEnabled = true;
            btnTimKiem.IsEnabled = true;

            // Bỏ chọn trên DataGrid
            dgNhanVien.SelectedItem = null;
            dgTaiKhoan.SelectedItem = null;

            // Reset trạng thái
            _isAddingNhanvien = false;
            _isUpdatingNhanvien = false;
            _selectedNhanvien = null;
            _selectedDangnhap = null;
        }

        // Hàm xóa trắng tất cả các trường nhập liệu (cả nhân viên và tài khoản)
        private void ClearInputs()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            cboGioiTinh.SelectedIndex = -1; // Đặt về không chọn
            txtSDTNV.Clear();
            txtNgaySinh.Clear();
            txtDiaChi.Clear();
            txtCCCD.Clear();
            txtNamVaoLam.Clear();
            txtLuong.Clear();

            txtMaTK.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            cboLoaiTK.SelectedIndex = -1; // Đặt về không chọn
        }

        // Hàm bật/tắt trạng thái Enabled/IsReadOnly cho tất cả các trường nhập liệu
        private void SetInputEnabled(bool enabled)
        {
            // Trường Nhân viên
            txtTenNV.IsEnabled = enabled;
            cboGioiTinh.IsEnabled = enabled;
            txtSDTNV.IsEnabled = enabled;
            txtNgaySinh.IsEnabled = enabled;
            txtDiaChi.IsEnabled = enabled;
            txtCCCD.IsEnabled = enabled;
            txtNamVaoLam.IsEnabled = enabled;
            txtLuong.IsEnabled = enabled;

            // Trường Tài khoản
           // txtMaTK.IsEnabled = enabled;
            txtTaiKhoan.IsEnabled = enabled;
            txtMatKhau.IsEnabled = enabled;
            cboLoaiTK.IsEnabled = enabled;
        }


        // --- Lấy dữ liệu từ UI và gán vào Object ---
        // Hàm này sẽ trả về cả Nhanvien và Dangnhap
        private (Nhanvien?, Dangnhap?) GetNhanvienAndAccountFromInputs()
        {
            // Nếu đang sửa, sử dụng _selectedNhanvien (đã được theo dõi)
            // Ngược lại, tạo đối tượng mới cho chế độ thêm
            var nv = _isUpdatingNhanvien ? _selectedNhanvien : new Nhanvien();
            var tk = _isUpdatingNhanvien ? _selectedDangnhap : new Dangnhap(); // Sẽ tạo mới hoặc dùng _selectedDangnhap

            if (_isUpdatingNhanvien && nv == null) return (null, null); // Lỗi nếu đang sửa mà _selectedNhanvien null

            // Cập nhật thông tin Nhanvien từ UI
            // Lưu ý: KHÔNG gán lại nv.MaNv nếu đang sửa
            if (!_isUpdatingNhanvien) // Chỉ gán MaNv khi thêm mới
            {
                nv.MaNv = txtMaNV.Text.Trim();
            }
            // Các thuộc tính khác thì cập nhật bình thường
            nv.TenNv = txtTenNV.Text.Trim();
            nv.GioiTinhNv = cboGioiTinh.SelectedValue?.ToString();
            nv.Sdtnv = txtSDTNV.Text.Trim();
            nv.DiaChiNv = txtDiaChi.Text.Trim();
            nv.Cccd = txtCCCD.Text.Trim();

            if (!DateOnly.TryParseExact(txtNgaySinh.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateOnly ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return (null, null);
            }
            nv.NgaySinhNv = ngaySinh;

            if (!int.TryParse(txtNamVaoLam.Text, out int namVaoLam))
            {
                MessageBox.Show("Năm vào làm không hợp lệ. Vui lòng nhập số nguyên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return (null, null);
            }
            nv.Nvl = namVaoLam;

            if (!decimal.TryParse(txtLuong.Text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out decimal luong))
            {
                MessageBox.Show("Lương không hợp lệ. Vui lòng nhập số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return (null, null);
            }
            nv.Luong = luong;

            // Kiểm tra các trường bắt buộc khác cho Nhanvien (tùy thuộc vào logic của bạn)
            if (string.IsNullOrWhiteSpace(nv.TenNv) || string.IsNullOrWhiteSpace(nv.Sdtnv) ||
                string.IsNullOrWhiteSpace(nv.DiaChiNv) || string.IsNullOrWhiteSpace(nv.Cccd) ||
                string.IsNullOrWhiteSpace(nv.GioiTinhNv) || !nv.Nvl.HasValue || !nv.Luong.HasValue)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin nhân viên bắt buộc.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return (null, null);
            }

            // Tạo/cập nhật đối tượng Dangnhap tạm thời (nếu tk null thì tạo mới)
            var tkFromInputs = tk ?? new Dangnhap(); // Sử dụng tk nếu nó đã tồn tại, ngược lại tạo mới

            // Đối với tài khoản, nếu đang sửa, Matk của tkFromInputs sẽ là Matk của _selectedDangnhap
            // Nếu thêm mới, Matk sẽ được gán sau đó.
            tkFromInputs.Matk = _selectedDangnhap?.Matk ?? string.Empty; // Đảm bảo Matk không bị thay đổi nếu đang sửa

            tkFromInputs.Taikhoan = txtTaiKhoan.Text.Trim();
            tkFromInputs.Matkhau = txtMatKhau.Text.Trim();
            tkFromInputs.Loaitk = cboLoaiTK.SelectedValue?.ToString();

            if (string.IsNullOrWhiteSpace(tkFromInputs.Taikhoan) || string.IsNullOrWhiteSpace(tkFromInputs.Matkhau) || string.IsNullOrWhiteSpace(tkFromInputs.Loaitk))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin tài khoản bắt buộc (Tài khoản, Mật khẩu, Loại tài khoản).", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return (null, null);
            }

            return (nv, tkFromInputs);
        }


        // --- Hiển thị dữ liệu lên UI ---
        // Hàm này sẽ hiển thị cả Nhanvien và Dangnhap
        private void DisplayNhanvienAndAccount(Nhanvien nv, Dangnhap? tk)
        {
            // Hiển thị thông tin Nhân viên
            txtMaNV.Text = nv.MaNv ?? string.Empty;
            txtTenNV.Text = nv.TenNv ?? string.Empty;
            cboGioiTinh.SelectedValue = nv.GioiTinhNv;
            txtSDTNV.Text = nv.Sdtnv ?? string.Empty;
            txtNgaySinh.Text = nv.NgaySinhNv?.ToString("dd/MM/yyyy") ?? string.Empty;
            txtDiaChi.Text = nv.DiaChiNv ?? string.Empty;
            txtCCCD.Text = nv.Cccd ?? string.Empty;
            txtNamVaoLam.Text = nv.Nvl?.ToString() ?? string.Empty;
            txtLuong.Text = nv.Luong?.ToString("N0") ?? string.Empty; // Định dạng tiền tệ

            // Hiển thị thông tin Tài khoản (nếu có)
            if (tk != null)
            {
                txtMaTK.Text = tk.Matk ?? string.Empty;
                txtTaiKhoan.Text = tk.Taikhoan ?? string.Empty;
                txtMatKhau.Text = tk.Matkhau ?? string.Empty; // Cẩn thận với mật khẩu thực tế
                cboLoaiTK.SelectedValue = tk.Loaitk;
            }
            else
            {
                // Nếu không có tài khoản liên kết, xóa trắng các trường tài khoản
                txtMaTK.Clear();
                txtTaiKhoan.Clear();
                txtMatKhau.Clear();
                cboLoaiTK.SelectedIndex = -1;
            }
        }

        // --- Sự kiện DataGrid ---
        private void DgNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Nhanvien selectedNv)
            {
                _selectedNhanvien = selectedNv;
                // Lấy thông tin tài khoản liên kết
                _selectedDangnhap = null;
                if (!string.IsNullOrEmpty(selectedNv.Matk))
                {
                    _selectedDangnhap = _dangnhapService.GetById(selectedNv.Matk);
                }
                DisplayNhanvienAndAccount(_selectedNhanvien, _selectedDangnhap);

                SetInputEnabled(false);
                txtMaNV.IsReadOnly = true; // Mã NV không thể sửa
                txtMaTK.IsReadOnly = true; // Mã TK không thể sửa

                btnSua.IsEnabled = true;
                btnXoa.IsEnabled = true;
                btnLuu.IsEnabled = false; // Luôn disable Lưu khi chỉ chọn
                btnThem.IsEnabled = true;
            }
            else
            {
                ClearInputs();
                _selectedNhanvien = null;
                _selectedDangnhap = null;
            }
        }

        private void DgTaiKhoan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Trong trường hợp này, việc chọn tài khoản trong DataGrid chỉ để xem.
            // Mọi thao tác Thêm/Sửa/Xóa sẽ được thực hiện thông qua form Nhân viên.
            // Nếu bạn muốn hiển thị tài khoản đã chọn lên các ô nhập, bạn có thể gọi DisplayDangnhap ở đây
            // nhưng cần đảm bảo nó không gây xung đột với việc hiển thị của Nhanvien.
            // Hiện tại, chúng ta không cần thêm logic phức tạp ở đây.
        }

        // --- Các sự kiện nút hành động ---
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
            SetInputEnabled(true);
            txtMaNV.IsReadOnly = false; // Cho phép nhập mã NV mới
            txtMaTK.IsReadOnly = true; // Cho phép nhập mã TK mới
            btnLuu.IsEnabled = true;

            _isAddingNhanvien = true;
            _isUpdatingNhanvien = false;
            _selectedNhanvien = null; // Đảm bảo tạo mới
            _selectedDangnhap = null; // Đảm bảo tạo mới
            dgNhanVien.SelectedItem = null; // Bỏ chọn trên DataGrid
            dgTaiKhoan.SelectedItem = null; // Bỏ chọn trên DataGrid

            MessageBox.Show("Vui lòng nhập thông tin nhân viên và tài khoản mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txtMaNV.Focus();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNhanvien == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetInputEnabled(true);
            txtMaNV.IsReadOnly = true; // Không cho phép sửa mã NV
            txtMaTK.IsReadOnly = true; // Không cho phép sửa mã TK khi sửa NV để tránh phức tạp

            btnLuu.IsEnabled = true;

            _isAddingNhanvien = false;
            _isUpdatingNhanvien = true;

            MessageBox.Show("Bạn có thể chỉnh sửa thông tin nhân viên và tài khoản đã chọn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedNhanvien == null)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{_selectedNhanvien.TenNv}' và tài khoản liên kết (nếu có)?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string? maTkToDelete = _selectedNhanvien.Matk;

                    // Xóa nhân viên trước
                    _nhanvienService.Delete(_selectedNhanvien.MaNv);

                    // Sau đó xóa tài khoản liên kết nếu tồn tại và không có nhân viên khác dùng chung
                    if (!string.IsNullOrEmpty(maTkToDelete))
                    {
                        var accountsLinked = _nhanvienService.GetAll().Any(nv => nv.Matk == maTkToDelete);
                        if (!accountsLinked) // Chỉ xóa tài khoản nếu không còn nhân viên nào khác liên kết
                        {
                            _dangnhapService.Delete(maTkToDelete);
                        }
                    }

                    MessageBox.Show("Xóa nhân viên và tài khoản liên kết thành công (nếu không còn nhân viên nào khác sử dụng tài khoản đó)!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData(); // Tải lại dữ liệu sau khi xóa
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa nhân viên/tài khoản: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine($"Error deleting Nhanvien/Dangnhap: {ex.ToString()}");
                }
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            (Nhanvien? nvToSave, Dangnhap? tkFromInputs) = GetNhanvienAndAccountFromInputs();

            if (nvToSave == null || tkFromInputs == null) return;

            try
            {
                if (_isAddingNhanvien)
                {
                    // Logic thêm mới (giữ nguyên hoặc cập nhật như đã trao đổi trước đó)
                    if (_nhanvienService.GetById(nvToSave.MaNv) != null)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.", "Lỗi trùng lặp", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (_dangnhapService.IsTaiKhoanExists(tkFromInputs.Taikhoan))
                    {
                        MessageBox.Show("Tên tài khoản này đã tồn tại. Vui lòng chọn tên khác.", "Lỗi trùng lặp", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    tkFromInputs.Matk = "TK" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                    while (_dangnhapService.GetById(tkFromInputs.Matk) != null)
                    {
                        tkFromInputs.Matk = "TK" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                    }

                    _dangnhapService.Add(tkFromInputs);
                    nvToSave.Matk = tkFromInputs.Matk;
                    _nhanvienService.Add(nvToSave);
                    MessageBox.Show("Thêm nhân viên và tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_isUpdatingNhanvien)
                {
                    // Cập nhật thông tin tài khoản
                    if (_selectedDangnhap != null) // Nếu nhân viên đã có tài khoản
                    {
                        // Kiểm tra trùng tên tài khoản nếu tên bị thay đổi
                        if (_dangnhapService.IsTaiKhoanExists(tkFromInputs.Taikhoan) && tkFromInputs.Taikhoan != _selectedDangnhap.Taikhoan)
                        {
                            MessageBox.Show("Tên tài khoản này đã tồn tại. Vui lòng chọn tên khác.", "Lỗi trùng lặp", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Cập nhật các thuộc tính của đối tượng _selectedDangnhap (đã được theo dõi)
                        _selectedDangnhap.Taikhoan = tkFromInputs.Taikhoan;
                        _selectedDangnhap.Matkhau = tkFromInputs.Matkhau;
                        _selectedDangnhap.Loaitk = tkFromInputs.Loaitk;

                        _dangnhapService.Update(_selectedDangnhap); // Gọi update trên đối tượng đã theo dõi
                    }
                    else // Nhân viên chưa có tài khoản, thêm tài khoản mới cho nhân viên này
                    {
                        if (_dangnhapService.IsTaiKhoanExists(tkFromInputs.Taikhoan))
                        {
                            MessageBox.Show("Tên tài khoản này đã tồn tại. Vui lòng chọn tên khác.", "Lỗi trùng lặp", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        tkFromInputs.Matk = "TK" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                        while (_dangnhapService.GetById(tkFromInputs.Matk) != null)
                        {
                            tkFromInputs.Matk = "TK" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                        }
                        _dangnhapService.Add(tkFromInputs);
                        _selectedNhanvien.Matk = tkFromInputs.Matk; // Gán Matk mới cho nhân viên đang được theo dõi
                    }

                    // Cập nhật thông tin nhân viên
                    // Các thuộc tính của _selectedNhanvien đã được cập nhật trong GetNhanvienAndAccountFromInputs
                    _nhanvienService.Update(_selectedNhanvien); // Gọi update trên đối tượng đã theo dõi

                    MessageBox.Show("Cập nhật nhân viên và tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadData();
                ResetFormState();
            }
            catch (DbUpdateException dbEx)
            {
                MessageBox.Show($"Lỗi cơ sở dữ liệu: {dbEx.Message}\nChi tiết: {dbEx.InnerException?.Message}", "Lỗi DB", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"DbUpdateException: {dbEx.ToString()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu nhân viên/tài khoản: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Error saving Nhanvien/Dangnhap: {ex.ToString()}");
            }
        }


        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormState();
            MessageBox.Show("Trạng thái form đã được reset.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData(); // Nếu không có từ khóa, tải lại toàn bộ dữ liệu
            }
            else
            {
                try
                {
                    dgNhanVien.ItemsSource = _nhanvienService.SearchByName(keyword);
                    // Không cần tìm kiếm riêng tài khoản, vì chúng ta tìm NV và hiển thị TK theo NV
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine($"Error searching Nhanvien: {ex.ToString()}");
                }
            }
        }

        // Bỏ các hàm BtnTaoTK_Click, BtnSuaTK_Click, BtnXoaTK_Click, BtnLuuTK_Click vì đã tích hợp
        // vào logic của nhân viên.
    }
}