using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BusinessObject;
using Microsoft.Win32;


namespace QuanLyShopQuanAoWPF
{
    public partial class QuanLyNhanVienWindow : Window
    {
        // Khai báo các Service
        private readonly INhanVienService _nhanvienService = new NhanVienService();
        private readonly IDangNhapService _dangnhapService = new DangNhapService();

        // Biến để lưu trạng thái đang thêm/sửa nhân viên hoặc tài khoản
        private bool _isAddingNhanvien = false;
        private bool _isUpdatingNhanvien = false;
        private bool _isAddingDangnhap = false;
        private bool _isUpdatingDangnhap = false;

        // Biến để lưu đối tượng đang được chọn/thao tác
        private Nhanvien _selectedNhanvien;
        private Dangnhap _selectedDangnhap;
        public QuanLyNhanVienWindow()
        {
            InitializeComponent();
            InitializeControls();
            LoadData(); // Tải dữ liệu ban đầu
        }

        private void InitializeControls()
        {
            // Thiết lập ComboBox Giới tính
            cboGioiTinh.ItemsSource = new List<string> { "Nam", "Nữ", "Khác" };
            cboGioiTinh.SelectedIndex = 0; // Chọn mặc định "Nam"

            // Gán sự kiện cho các nút
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuuTK_Click;
            btnReset.Click += BtnReset_Click;
            btnTimKiem.Click += BtnTimKiem_Click;

            btnTaoTK.Click += BtnTaoTK_Click;
            btnXoaTK.Click += BtnXoaTK_Click;
            btnSuaTK.Click += BtnSuaTK_Click;
            btnLuuTK.Click += BtnLuuTK_Click;

            // Gán sự kiện khi chọn một dòng trên DataGrid
            dgNhanVien.SelectionChanged += DgNhanVien_SelectionChanged;
            dgTaiKhoan.SelectionChanged += DgTaiKhoan_SelectionChanged;

            // Vô hiệu hóa các trường nhập liệu và nút Lưu ban đầu
            SetNhanvienInputEnabled(false);
            SetDangnhapInputEnabled(false);
            btnLuu.IsEnabled = false;
            btnLuuTK.IsEnabled = false;
            txtMaNV.IsReadOnly = true; // Mã NV chỉ đọc khi sửa/hiển thị

            // Thêm các controls cho tài khoản nếu chưa có trong XAML
            // Để quản lý tài khoản, bạn cần thêm các controls sau vào XAML:
            // <TextBlock Text="Mã TK ĐN:" Grid.Row="4" Grid.Column="0" VerticalAlignment="Center"/>
            // <TextBox x:Name="txtMaTKDN" Grid.Row="4" Grid.Column="1" Height="28"/>
            // <TextBlock Text="Tên TK ĐN:" Grid.Row="4" Grid.Column="2" VerticalAlignment="Center" Margin="50,0,0,0"/>
            // <TextBox x:Name="txtTenTKDN" Grid.Row="4" Grid.Column="3" Height="28"/>
            // <TextBlock Text="Mật khẩu TK:" Grid.Row="4" Grid.Column="4" VerticalAlignment="Center" Margin="63,0,0,0"/>
            // <TextBox x:Name="txtMatKhauTKDN" Grid.Row="4" Grid.Column="5" Height="28"/>
            // <TextBlock Text="Loại TK:" Grid.Row="5" Grid.Column="0" VerticalAlignment="Center"/>
            // <ComboBox x:Name="cboLoaiTKDN" Grid.Row="5" Grid.Column="1" Height="28"/>
        }

        private void LoadData()
        {
            try
            {
                // Load danh sách nhân viên vào DataGrid NhanVien
                dgNhanVien.ItemsSource = _nhanvienService.GetAll();

                // Load danh sách tài khoản vào DataGrid TaiKhoan
                dgTaiKhoan.ItemsSource = _dangnhapService.GetAll();

                // Load danh sách tài khoản vào ComboBox MaTK (cho Nhân viên)
                cboMaTK.ItemsSource = _dangnhapService.GetAll();
                cboMaTK.DisplayMemberPath = "TAIKHOAN";
                cboMaTK.SelectedValuePath = "MATK"; // Đảm bảo đúng tên property của khóa chính

                // Reset trạng thái và giao diện
                ResetFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetFormState()
        {
            ClearNhanvienInputs();
            // ClearTaiKhoanInputs(); // Nếu có riêng biệt controls cho TK
            SetNhanvienInputEnabled(false);
            SetDangnhapInputEnabled(false);
            btnLuu.IsEnabled = false;
            btnLuuTK.IsEnabled = false;
            txtMaNV.IsReadOnly = true;

            // Bật lại các nút thao tác chính
            btnThem.IsEnabled = true;
            btnSua.IsEnabled = true;
            btnXoa.IsEnabled = true;
            btnTimKiem.IsEnabled = true;
            btnTaoTK.IsEnabled = true;
            btnXoaTK.IsEnabled = true;
            btnSuaTK.IsEnabled = true;

            _isAddingNhanvien = false;
            _isUpdatingNhanvien = false;
            _isAddingDangnhap = false;
            _isUpdatingDangnhap = false;
            _selectedNhanvien = null;
            _selectedDangnhap = null;

            dgNhanVien.SelectedItem = null; // Bỏ chọn trên DataGrid
            dgTaiKhoan.SelectedItem = null; // Bỏ chọn trên DataGrid
        }

        private void ClearNhanvienInputs()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            cboGioiTinh.SelectedIndex = 0;
            txtSDTNV.Clear();
            txtNgaySinh.Clear();
            txtDiaChi.Clear();
            txtCCCD.Clear();
            txtNamVaoLam.Clear();
            txtLuong.Clear();
            cboMaTK.SelectedIndex = -1; // Bỏ chọn tài khoản
        }

        private void ClearDangnhapInputs()
        {
            // Nếu có các TextBox riêng biệt cho Tài khoản (ví dụ: txtMaTKDN, txtTenTKDN, txtMatKhauTKDN, cboLoaiTKDN)
            // txtMaTKDN.Clear();
            // txtTenTKDN.Clear();
            // txtMatKhauTKDN.Clear();
            // cboLoaiTKDN.SelectedIndex = -1;
        }

        private void SetNhanvienInputEnabled(bool enabled)
        {
            // txtMaNV.IsEnabled = enabled; // Mã NV được quản lý bởi IsReadOnly
            txtTenNV.IsEnabled = enabled;
            cboGioiTinh.IsEnabled = enabled;
            txtSDTNV.IsEnabled = enabled;
            txtNgaySinh.IsEnabled = enabled;
            txtDiaChi.IsEnabled = enabled;
            txtCCCD.IsEnabled = enabled;
            txtNamVaoLam.IsEnabled = enabled;
            txtLuong.IsEnabled = enabled;
            cboMaTK.IsEnabled = enabled;
        }

        private void SetDangnhapInputEnabled(bool enabled)
        {
            // Nếu có các TextBox riêng biệt cho Tài khoản
            // txtMaTKDN.IsEnabled = enabled;
            // txtTenTKDN.IsEnabled = enabled;
            // txtMatKhauTKDN.IsEnabled = enabled;
            // cboLoaiTKDN.IsEnabled = enabled;
            // txtMaTKDN.IsReadOnly = !enabled || _isUpdatingDangnhap; // Mã TK chỉ đọc khi sửa
        }

        // --- Lấy dữ liệu từ UI và gán vào Object ---
        private Nhanvien GetNhanvienFromInputs()
        {
            var nv = _selectedNhanvien ?? new Nhanvien();

            // Nếu là thêm mới, Mã NV cần được nhập hoặc tạo tự động
            if (_isAddingNhanvien)
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã nhân viên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
                }
                nv.MaNv = txtMaNV.Text.Trim();
            }
            // Nếu là sửa, Mã NV không thay đổi
            else if (_isUpdatingNhanvien)
            {
                nv.MaNv = txtMaNV.Text.Trim();
            }

            nv.TenNv = txtTenNV.Text.Trim();
            nv.GioiTinhNv = cboGioiTinh.SelectedItem?.ToString();
            nv.Sdtnv = txtSDTNV.Text.Trim();
            nv.DiaChiNv = txtDiaChi.Text.Trim();
            nv.Cccd = txtCCCD.Text.Trim();
            nv.Matk = cboMaTK.SelectedValue?.ToString(); // Lấy MATK từ ComboBox

            // Validate và Parse Ngày sinh
            if (DateOnly.TryParseExact(txtNgaySinh.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateOnly ngaySinh))
            {
                nv.NgaySinhNv = ngaySinh;
                // ... (your existing successful code)
            }
            else
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            // Validate và Parse Năm vào làm
            if (int.TryParse(txtNamVaoLam.Text, out int namVaoLam))
            {
                nv.Nvl = namVaoLam;
            }
            else
            {
                MessageBox.Show("Năm vào làm không hợp lệ. Vui lòng nhập số nguyên.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            // Validate và Parse Lương
            if (decimal.TryParse(txtLuong.Text, out decimal luong))
            {
                nv.Luong = luong;
            }
            else
            {
                MessageBox.Show("Lương không hợp lệ. Vui lòng nhập số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            // Kiểm tra các trường bắt buộc khác
            if (string.IsNullOrWhiteSpace(nv.TenNv) || string.IsNullOrWhiteSpace(nv.Sdtnv) ||
                string.IsNullOrWhiteSpace(nv.DiaChiNv) || string.IsNullOrWhiteSpace(nv.Cccd) ||
                string.IsNullOrWhiteSpace(nv.GioiTinhNv))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin nhân viên bắt buộc.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            return nv;
        }

        private Dangnhap GetDangnhapFromInputs()
        {
            // Để hàm này hoạt động, bạn cần có các TextBox riêng biệt cho Mã TK, Tên TK, Mật khẩu, Loại TK trong XAML
            // Hiện tại, tôi sẽ giả định bạn đã thêm chúng vào.
            // Nếu chưa có, các hàm này sẽ không hoạt động hoặc cần được điều chỉnh.

            //var tk = _selectedDangnhap ?? new Dangnhap();
            //if (_isAddingDangnhap)
            //{
            //    if (string.IsNullOrWhiteSpace(txtMaTKDN.Text))
            //    {
            //        MessageBox.Show("Vui lòng nhập Mã tài khoản.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return null;
            //    }
            //    tk.Matk = txtMaTKD.Text.Trim();
            //}
            //else if (_isUpdatingDangnhap)
            //{
            //    tk.MATK = txtMaTKDN.Text.Trim();
            //}
            //tk.Taikhoan = txtTenTKDN.Text.Trim();
            //tk.Matkhau = txtMatKhauTKDN.Text; // Cẩn thận với mật khẩu
            //tk.Loaitk = cboMaTK.SelectedItem?.ToString();

            //// Kiểm tra các trường bắt buộc cho tài khoản
            //if (string.IsNullOrWhiteSpace(tk.Taikhoan) || string.IsNullOrWhiteSpace(tk.Matkhau) ||
            //    string.IsNullOrWhiteSpace(tk.Loaitk))
            //{
            //    MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản: Tên tài khoản, Mật khẩu, Loại tài khoản.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return null;
            //}

            //return tk;
            MessageBox.Show("Chức năng quản lý Tài Khoản (GetDangnhapFromInputs) yêu cầu các trường nhập liệu riêng biệt cho Tài Khoản trong XAML.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return null; // Trả về null nếu không có form nhập liệu tài khoản riêng
        }

        // --- Hiển thị dữ liệu lên UI ---
        private void DisplayNhanvien(Nhanvien nv)
        {
            if (nv != null)
            {
                txtMaNV.Text = nv.MaNv;
                txtTenNV.Text = nv.TenNv;
                cboGioiTinh.SelectedItem = nv.GioiTinhNv;
                txtSDTNV.Text = nv.Sdtnv;
                txtNgaySinh.Text = nv.NgaySinhNv?.ToString("dd/MM/yyyy"); // Định dạng ngày tháng
                txtDiaChi.Text = nv.DiaChiNv;
                txtCCCD.Text = nv.Cccd;
                txtNamVaoLam.Text = nv.Nvl.ToString();
                txtLuong.Text = nv.Luong.ToString(); // Định dạng tiền tệ không có phần thập phân
                cboMaTK.SelectedValue = nv.Matk; // Chọn tài khoản liên kết trong ComboBox
            }
        }

        private void DisplayDangnhap(Dangnhap tk)
        {
            // Nếu có các TextBox riêng biệt cho Tài khoản, hiển thị ở đây
            //if (tk != null)
            //{
            //    txtMaTKDN.Text = tk.Matk;
            //    txtTenTKDN.Text = tk.Taikhoan;
            //    txtMatKhauTKDN.Text = tk.Matkhau; // Cẩn thận với việc hiển thị mật khẩu
            //    cboLoaiTKDN.SelectedItem = tk.Loaitk;
            //}
        }

        // --- Sự kiện DataGrid ---
        private void DgNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Nhanvien nv)
            {
                _selectedNhanvien = nv;
                DisplayNhanvien(nv);

                // Khi chọn NV, có thể load và chọn TK tương ứng trong dgTaiKhoan (nếu có)
                if (!string.IsNullOrEmpty(nv.Matk))
                {
                    _selectedDangnhap = _dangnhapService.GetById(nv.Matk);
                    // Để chọn dòng trong dgTaiKhoan, bạn cần phải tìm đối tượng đó trong ItemsSource của dgTaiKhoan
                    // Hoặc bạn có thể chỉ đơn giản là DisplayDangnhap(_selectedDangnhap) nếu có các controls riêng
                    if (dgTaiKhoan.ItemsSource is IEnumerable<Dangnhap> dangnhapList)
                    {
                        dgTaiKhoan.SelectedItem = dangnhapList.FirstOrDefault(t => t.Matk == nv.Matk);
                    }
                }
                else
                {
                    _selectedDangnhap = null;
                    dgTaiKhoan.SelectedItem = null;
                    // ClearDangnhapInputs(); // Nếu có riêng biệt controls cho TK
                }

                // Chuyển sang chế độ sửa khi chọn
                SetNhanvienInputEnabled(true);
                txtMaNV.IsReadOnly = true; // Mã NV không cho sửa
                btnLuu.IsEnabled = true;
                _isAddingNhanvien = false;
                _isUpdatingNhanvien = true;
            }
        }

        private void DgTaiKhoan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem is Dangnhap tk)
            {
                _selectedDangnhap = tk;
                DisplayDangnhap(tk); // Hiển thị lên các textbox tài khoản nếu có

                // Khi chọn TK, có thể tự động chọn NV liên quan (nếu có 1-1 hoặc tìm NV đầu tiên liên kết)
                // Theo diagram 1 Dangnhap có nhiều Nhanvien, nên việc tự động chọn 1 NV cụ thể sẽ phức tạp hơn.
                // Để đơn giản, ta chỉ hiển thị thông tin TK lên form TK (nếu có).

                // Chuyển sang chế độ sửa tài khoản (nếu có các control riêng)
                SetDangnhapInputEnabled(true);
                btnLuuTK.IsEnabled = true;
                _isAddingDangnhap = false;
                _isUpdatingDangnhap = true;
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            ClearNhanvienInputs();
            SetNhanvienInputEnabled(true);
            txtMaNV.IsReadOnly = false; // Cho phép nhập mã NV mới
            btnLuu.IsEnabled = true;

            _isAddingNhanvien = true;
            _isUpdatingNhanvien = false;
            _selectedNhanvien = null; // Đảm bảo tạo mới
            dgNhanVien.SelectedItem = null; // Bỏ chọn trên DataGrid

            MessageBox.Show("Vui lòng nhập thông tin nhân viên mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            txtMaNV.Focus();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Nhanvien nvToEdit)
            {
                _selectedNhanvien = nvToEdit;
                DisplayNhanvien(nvToEdit);
                SetNhanvienInputEnabled(true);
                txtMaNV.IsReadOnly = true; // Không cho phép sửa mã NV
                btnLuu.IsEnabled = true;

                _isAddingNhanvien = false;
                _isUpdatingNhanvien = true;

                MessageBox.Show($"Bạn đang sửa thông tin nhân viên: {nvToEdit.TenNv}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }   

        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgNhanVien.SelectedItem is Nhanvien nvToDelete)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên '{nvToDelete.TenNv}' không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _nhanvienService.Delete(nvToDelete.MaNv);
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); // Tải lại dữ liệu sau khi xóa
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            Nhanvien nvToSave = GetNhanvienFromInputs();
            if (nvToSave == null) return; // Validation thất bại

            try
            {
                if (_isAddingNhanvien)
                {
                    // Kiểm tra trùng mã NV
                    if (_nhanvienService.GetById(nvToSave.MaNv) != null)
                    {
                        MessageBox.Show("Mã nhân viên này đã tồn tại. Vui lòng chọn mã khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _nhanvienService.Add(nvToSave);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_isUpdatingNhanvien)
                {
                    _nhanvienService.Update(nvToSave);
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadData(); // Tải lại dữ liệu sau khi lưu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu nhân viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetFormState();
            MessageBox.Show("Trạng thái form đã được reset.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- Chức năng quản lý TÀI KHOẢN ---
        // Các hàm này sẽ cần các controls riêng biệt cho Tài khoản trong XAML để hoạt động đầy đủ.

        private void BtnTaoTK_Click(object sender, RoutedEventArgs e)
        {
            // Để tạo tài khoản mới, bạn cần có các trường nhập liệu cho tài khoản
            // (ví dụ: txtMaTKDN, txtTenTKDN, txtMatKhauTKDN, cboLoaiTKDN)
            // Hiện tại, chỉ có thể chọn tài khoản đã tồn tại qua cboMaTK.
            MessageBox.Show("Chức năng 'Tạo tài khoản' yêu cầu các trường nhập liệu riêng biệt cho Tài Khoản trong XAML.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Ví dụ nếu bạn có các control riêng:
            //ClearDangnhapInputs();
            //SetDangnhapInputEnabled(true);
            //btnLuuTK.IsEnabled = true;
            //txtMaTKDN.IsReadOnly = false; // Cho phép nhập mã TK mới
            //_isAddingDangnhap = true;
            //_isUpdatingDangnhap = false;
            //_selectedDangnhap = null;
            //txtMaTKDN.Focus();
        }

        private void BtnSuaTK_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem is Dangnhap tkToEdit)
            {
                _selectedDangnhap = tkToEdit;
                DisplayDangnhap(tkToEdit); // Hiển thị lên các textbox tài khoản nếu có

                MessageBox.Show($"Bạn đang sửa thông tin tài khoản: {tkToEdit.Taikhoan}. Vui lòng chỉnh sửa các trường tài khoản và nhấn Lưu Tài Khoản (nếu có các trường nhập liệu riêng).", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                // Ví dụ nếu bạn có các control riêng:
                SetDangnhapInputEnabled(true);
                btnLuuTK.IsEnabled = true;
                //txtMaTKDN.IsReadOnly = true; // Mã TK không cho sửa
                _isAddingDangnhap = false;
                _isUpdatingDangnhap = true;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để sửa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnXoaTK_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaiKhoan.SelectedItem is Dangnhap tkToDelete)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa tài khoản '{tkToDelete.Taikhoan}' không? Thao tác này sẽ không được thực hiện nếu có nhân viên đang liên kết với tài khoản này.", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _dangnhapService.Delete(tkToDelete.Matk);
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); // Tải lại dữ liệu sau khi xóa
                    }
                    catch (InvalidOperationException ex) // Bắt lỗi nếu có nhân viên liên kết
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa tài khoản: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnLuuTK_Click(object sender, RoutedEventArgs e)
        {
            //Để hàm này hoạt động, bạn cần có các TextBox riêng biệt cho Mã TK, Tên TK, Mật khẩu, Loại TK trong XAML
            Dangnhap tkToSave = GetDangnhapFromInputs();
            if (tkToSave == null) return; // Validation thất bại

            try
            {
                if (_isAddingDangnhap)
                {
                    // Kiểm tra trùng mã TK
                    if (_dangnhapService.GetById(tkToSave.Matk) != null)
                    {
                        MessageBox.Show("Mã tài khoản này đã tồn tại. Vui lòng chọn mã khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    // Kiểm tra trùng tên tài khoản (username)
                    if (_dangnhapService.IsTaiKhoanExists(tkToSave.Taikhoan))
                    {
                        MessageBox.Show("Tên tài khoản này đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _dangnhapService.Add(tkToSave);
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_isUpdatingDangnhap)
                {
                    _dangnhapService.Update(tkToSave);
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadData(); // Tải lại dữ liệu sau khi lưu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu tài khoản: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Chức năng 'Lưu Tài Khoản' chưa được triển khai hoàn chỉnh vì thiếu các trường nhập liệu tài khoản riêng biệt.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


    }
}