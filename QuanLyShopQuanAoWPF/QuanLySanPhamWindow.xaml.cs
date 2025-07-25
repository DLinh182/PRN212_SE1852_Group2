using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BusinessObject;
using Microsoft.Win32;

namespace QuanLyShopQuanAoWPF
{
    public partial class QuanLySanPhamWindow : Window
    {
        private readonly ISanPhamService _sanPhamService = new SanPhamService();
        private readonly ILoaiSpService _loaiSpService = new LoaiSpService();

        // Biến để lưu trữ sản phẩm và danh mục đang được chọn/thao tác
        private Sanpham _selectedSanPham;
        private Loaisp _selectedLoaiSp;

        // Biến trạng thái cho việc thêm/sửa sản phẩm
        private bool _isAddingSanPham = false;
        private bool _isUpdatingSanPham = false;

        // Biến trạng thái cho việc thêm/sửa danh mục
        private bool _isAddingLoaiSp = false;
        private bool _isUpdatingLoaiSp = false;

        // Đường dẫn hình ảnh tạm thời
        private string _tempImagePath;
        public QuanLySanPhamWindow()
        {
            InitializeComponent();
            InitializeControls(); // Thiết lập ban đầu cho các control
            LoadData();           // Tải dữ liệu ban đầu
        }

        private void InitializeControls()
        {
            // Vô hiệu hóa các trường nhập liệu sản phẩm ban đầu
            SetSanPhamInputEnabled(false);
            btnLuu.IsEnabled = false; // Nút lưu sản phẩm

            // Điền dữ liệu cho ComboBox Giới tính
            cboGioiTinh.ItemsSource = new List<string> { "Nam", "Nữ", "Unisex" };
            cboGioiTinh.SelectedIndex = 0; // Chọn mặc định

            // Gán sự kiện cho các control
            dgSanPham.SelectionChanged += DgSanPham_SelectionChanged;
            dgDanhMuc.SelectionChanged += DgDanhMuc_SelectionChanged;
            btnUpHinh.Click += BtnUpHinh_Click;

            // Các nút Sản phẩm
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnReset.Click += BtnReset_Click;
            btnTimKiem.Click += BtnTimKiem_Click;

            // Các nút Danh mục
            btnTaoDM.Click += BtnTaoDM_Click;
            btnXoaDM.Click += BtnXoaDM_Click;
        }

        private void LoadData()
        {
            LoadSanPhams();
            LoadLoaiSps();
            ClearSanPhamInputs();
            ClearLoaiSpSelection();
            UpdateSanPhamButtonStates();
            UpdateLoaiSpButtonStates();
        }

        private void LoadSanPhams()
        {
            try
            {
                dgSanPham.ItemsSource = _sanPhamService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLoaiSps()
        {
            try
            {
                var loaiSps = _loaiSpService.GetAll();
                dgDanhMuc.ItemsSource = loaiSps;
                cboDanhMuc.ItemsSource = loaiSps; // Đổ dữ liệu vào ComboBox Danh mục
                cboDanhMuc.DisplayMemberPath = "TenL"; // Hiển thị tên loại
                cboDanhMuc.SelectedValuePath = "MaL"; // Giá trị thực là mã loại
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- Quản lý trạng thái Enabled của các control nhập liệu và nút bấm ---
        private void SetSanPhamInputEnabled(bool enabled)
        {
            txtMaSP.IsEnabled = enabled;
            txtTenSP.IsEnabled = enabled;
            txtGiaBan.IsEnabled = enabled;
            cboGioiTinh.IsEnabled = enabled;
            txtThongTin.IsEnabled = enabled;
            cboDanhMuc.IsEnabled = enabled;
            txtKieuDang.IsEnabled = enabled;
            txtChatLieu.IsEnabled = enabled;
            txtSLT.IsEnabled = enabled;
            txtDaBan.IsEnabled = enabled;
            txtTinhTrang.IsEnabled = enabled;
            btnUpHinh.IsEnabled = enabled;
        }

        private void UpdateSanPhamButtonStates()
        {
            bool isSanPhamSelected = (_selectedSanPham != null);

            // Nút sản phẩm
            btnThem.IsEnabled = true; // Luôn có thể thêm
            btnSua.IsEnabled = isSanPhamSelected && !_isAddingSanPham;
            btnXoa.IsEnabled = isSanPhamSelected && !_isAddingSanPham;
            btnLuu.IsEnabled = _isAddingSanPham || _isUpdatingSanPham; // Bật khi đang thêm hoặc sửa

            // Vô hiệu hóa DataGrid khi đang thêm/sửa
            dgSanPham.IsEnabled = !(_isAddingSanPham || _isUpdatingSanPham);
        }

        private void UpdateLoaiSpButtonStates()
        {
            bool isLoaiSpSelected = (_selectedLoaiSp != null);

            btnTaoDM.IsEnabled = true; // Luôn có thể tạo danh mục
            btnXoaDM.IsEnabled = isLoaiSpSelected && !_isAddingLoaiSp;


            // Vô hiệu hóa DataGrid khi đang thêm/sửa danh mục
            dgDanhMuc.IsEnabled = !(_isAddingLoaiSp || _isUpdatingLoaiSp);
        }

        // --- Xóa dữ liệu trên các control nhập liệu ---
        private void ClearSanPhamInputs()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGiaBan.Clear();
            cboGioiTinh.SelectedIndex = 0; // Reset
            txtThongTin.Clear();
            cboDanhMuc.SelectedIndex = -1; // Reset
            txtKieuDang.Clear();
            txtChatLieu.Clear();
            txtSLT.Clear();
            txtDaBan.Clear();
            txtTinhTrang.Clear();
            imgHinh.Source = null;
            _tempImagePath = null;
        }

        private void ClearLoaiSpSelection()
        {
            dgDanhMuc.SelectedItem = null;
            _selectedLoaiSp = null;
        }

        // --- Gán dữ liệu từ DataGrid vào các control nhập liệu (khi chọn sản phẩm) ---
        private void DisplaySanPham(Sanpham sp)
        {
            if (sp != null)
            {
                txtMaSP.Text = sp.MaSp;
                txtTenSP.Text = sp.TenSp;
                txtGiaBan.Text = sp.GiaBan.ToString();
                cboGioiTinh.SelectedItem = sp.GioiTinh;
                txtThongTin.Text = sp.ThongTinSp;
                cboDanhMuc.SelectedValue = sp.MaL; // Chọn danh mục theo MaL
                txtKieuDang.Text = sp.Form;
                txtChatLieu.Text = sp.ChatLieu;
                txtSLT.Text = sp.SoLuongTon.ToString();
                txtDaBan.Text = sp.DaBan.ToString();
                txtTinhTrang.Text = sp.TinhTrang;

                // Hiển thị hình ảnh
                if (!string.IsNullOrEmpty(sp.HinhSp) && File.Exists(sp.HinhSp))
                {
                    try
                    {
                        // Tạo một BitmapImage mới để tránh khóa file
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Tải toàn bộ vào bộ nhớ
                        bitmap.UriSource = new Uri(sp.HinhSp, UriKind.Absolute);
                        bitmap.EndInit();
                        imgHinh.Source = bitmap;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải hình ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        imgHinh.Source = null;
                    }
                }
                else
                {
                    imgHinh.Source = null;
                }
                _tempImagePath = null; // Reset temp path
            }
        }

        // --- Lấy dữ liệu từ các control nhập liệu để tạo/cập nhật sản phẩm ---
        private Sanpham GetSanPhamFromInputs()
        {
            var sanPham = _selectedSanPham ?? new Sanpham(); // Nếu đang sửa dùng _selectedSanPham, nếu thêm thì tạo mới

            // Gán giá trị từ UI vào đối tượng Sanpham
            sanPham.MaSp = txtMaSP.Text;
            sanPham.TenSp = txtTenSP.Text;

            // Xử lý giá bán
            if (decimal.TryParse(txtGiaBan.Text, out decimal giaBan))
            {
                sanPham.GiaBan = (double)giaBan;
            }
            else
            {
                MessageBox.Show("Giá bán không hợp lệ.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            sanPham.GioiTinh = cboGioiTinh.SelectedItem?.ToString();
            sanPham.ThongTinSp = txtThongTin.Text;
            sanPham.ChatLieu = txtChatLieu.Text;
            sanPham.Form = txtKieuDang.Text;

            // Xử lý số lượng tồn
            if (int.TryParse(txtSLT.Text, out int soLuongTon))
            {
                sanPham.SoLuongTon = soLuongTon;
            }
            else
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // Xử lý đã bán (chỉ cho phép sửa khi thêm mới nếu cần, hoặc quản lý tự động)
            if (int.TryParse(txtDaBan.Text, out int daBan))
            {
                sanPham.DaBan = daBan;
            }
            else
            {
                // Nếu đang thêm, có thể mặc định là 0
                if (_isAddingSanPham) sanPham.DaBan = 0;
                else
                {
                    MessageBox.Show("Số lượng đã bán không hợp lệ.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }

            sanPham.TinhTrang = txtTinhTrang.Text;

            // Xử lý Mã Loại (Danh mục)
            if (cboDanhMuc.SelectedValue != null)
            {
                sanPham.MaL = cboDanhMuc.SelectedValue.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một danh mục.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // Xử lý hình ảnh: Nếu có hình ảnh mới được upload (_tempImagePath) thì dùng nó
            // Nếu không, giữ nguyên hình ảnh cũ (khi cập nhật)
            // Nếu là thêm mới mà không có hình, có thể để null hoặc default
            if (!string.IsNullOrEmpty(_tempImagePath))
            {
                // Copy ảnh vào thư mục dự án (ví dụ: Images)
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string imagesPath = Path.Combine(appDirectory, "Images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }
                string fileName = Path.GetFileName(_tempImagePath);
                string destinationPath = Path.Combine(imagesPath, fileName);

                try
                {
                    File.Copy(_tempImagePath, destinationPath, true); // true để ghi đè nếu đã tồn tại
                    sanPham.HinhSp = destinationPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi sao chép hình ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            // else: nếu _tempImagePath rỗng, giữ nguyên sanPham.HinhSP (nếu đang sửa)
            // Hoặc nếu là thêm mới mà không upload ảnh, HinhSP sẽ là null hoặc chuỗi rỗng

            return sanPham;
        }

        // --- Xử lý sự kiện DataGrid ---
        private void DgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isAddingSanPham && !_isUpdatingSanPham) // Chỉ cập nhật khi không ở chế độ thêm/sửa
            {
                _selectedSanPham = dgSanPham.SelectedItem as Sanpham;
                DisplaySanPham(_selectedSanPham);
                UpdateSanPhamButtonStates();
            }
        }

        private void DgDanhMuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isAddingLoaiSp && !_isUpdatingLoaiSp) // Chỉ cập nhật khi không ở chế độ thêm/sửa
            {
                _selectedLoaiSp = dgDanhMuc.SelectedItem as Loaisp;
                UpdateLoaiSpButtonStates();
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            _isAddingSanPham = true;
            _isUpdatingSanPham = false;
            ClearSanPhamInputs();
            SetSanPhamInputEnabled(true);
            txtMaSP.IsEnabled = true; // Mã sản phẩm có thể nhập khi thêm
            UpdateSanPhamButtonStates();
            MessageBox.Show("Bắt đầu thêm sản phẩm. Vui lòng nhập thông tin và nhấn Lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSanPham != null)
            {
                _isAddingSanPham = false;
                _isUpdatingSanPham = true;
                SetSanPhamInputEnabled(true);
                txtMaSP.IsEnabled = false; // Không cho phép sửa mã sản phẩm
                UpdateSanPhamButtonStates();
                MessageBox.Show("Bắt đầu sửa sản phẩm. Vui lòng chỉnh sửa thông tin và nhấn Lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSanPham != null)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{_selectedSanPham.TenSp}' có mã '{_selectedSanPham.MaSp}' không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _sanPhamService.Delete(_selectedSanPham.MaSp);
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadSanPhams(); // Tải lại danh sách
                        ClearSanPhamInputs();
                        _selectedSanPham = null;
                        UpdateSanPhamButtonStates();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            Sanpham sanPhamToSave = GetSanPhamFromInputs();
            if (sanPhamToSave == null) return; // Lỗi trong quá trình lấy dữ liệu từ input

            try
            {
                if (_isAddingSanPham)
                {
                    _sanPhamService.Add(sanPhamToSave);
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_isUpdatingSanPham)
                {
                    _sanPhamService.Update(sanPhamToSave);
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                _isAddingSanPham = false;
                _isUpdatingSanPham = false;
                SetSanPhamInputEnabled(false);
                LoadSanPhams(); // Tải lại danh sách sản phẩm
                ClearSanPhamInputs();
                _selectedSanPham = null;
                UpdateSanPhamButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _isAddingSanPham = false;
            _isUpdatingSanPham = false;
            _isAddingLoaiSp = false;
            _isUpdatingLoaiSp = false;

            ClearSanPhamInputs();
            SetSanPhamInputEnabled(false);
            _selectedSanPham = null;

            ClearLoaiSpSelection();
            _selectedLoaiSp = null;

            UpdateSanPhamButtonStates();
            UpdateLoaiSpButtonStates();
            LoadData(); // Load lại toàn bộ data để reset trạng thái tìm kiếm, v.v.
            MessageBox.Show("Đã reset trạng thái và dữ liệu.", "Thông báo");
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtTimKiem.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                try
                {
                    dgSanPham.ItemsSource = _sanPhamService.SearchByName(searchText);
                    ClearSanPhamInputs();
                    _selectedSanPham = null;
                    UpdateSanPhamButtonStates();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadSanPhams(); // Nếu ô tìm kiếm rỗng, hiển thị tất cả
            }
        }

        // --- Xử lý upload hình ảnh ---
        private void BtnUpHinh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _tempImagePath = openFileDialog.FileName;
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(_tempImagePath, UriKind.Absolute);
                    bitmap.EndInit();
                    imgHinh.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xem trước hình ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    imgHinh.Source = null;
                    _tempImagePath = null;
                }
            }
        }

        // --- Sự kiện của các nút Danh mục ---
        private void BtnTaoDM_Click(object sender, RoutedEventArgs e)
        {
            var taoDMWindow = new TaoDanhMucWindow();
            if (taoDMWindow.ShowDialog() == true)
            {
                LoadLoaiSps(); // Sau khi tạo xong, reload lại danh mục
            }
        }

        private void BtnXoaDM_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLoaiSp != null)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa danh mục '{_selectedLoaiSp.TenL}' không? Thao tác này có thể ảnh hưởng đến các sản phẩm liên quan.", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Kiểm tra xem có sản phẩm nào thuộc danh mục này không
                        var sanPhamsInLoai = _sanPhamService.GetAll().Where(sp => sp.MaL == _selectedLoaiSp.MaL).ToList();
                        if (sanPhamsInLoai.Any())
                        {
                            MessageBox.Show("Không thể xóa danh mục này vì có sản phẩm đang thuộc danh mục này. Vui lòng chuyển các sản phẩm sang danh mục khác trước.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        _loaiSpService.Delete(_selectedLoaiSp.MaL);
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadLoaiSps(); // Tải lại danh sách danh mục
                        ClearLoaiSpSelection();
                        UpdateLoaiSpButtonStates();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa danh mục: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một danh mục để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --- Xử lý chỉnh sửa DataGrid Danh mục trực tiếp ---
        private void DgDanhMuc_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (_isAddingLoaiSp || _isUpdatingLoaiSp)
            {
                // Logic này sẽ được gọi khi người dùng kết thúc chỉnh sửa một ô trong DataGrid
                // (ví dụ: nhấn Enter hoặc Tab ra khỏi ô)
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    var editedLoaiSp = e.Row.Item as Loaisp;
                    if (editedLoaiSp != null)
                    {
                        try
                        {
                            if (_isAddingLoaiSp)
                            {
                                // Bạn cần đảm bảo MaLSP được tạo ra hoặc nhập vào hợp lệ
                                if (string.IsNullOrEmpty(editedLoaiSp.MaL) || string.IsNullOrEmpty(editedLoaiSp.TenL))
                                {
                                    MessageBox.Show("Mã loại và Tên loại không được để trống.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                                    e.Cancel = true; // Hủy bỏ chỉnh sửa
                                    return;
                                }
                                _loaiSpService.Add(editedLoaiSp);
                                MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else if (_isUpdatingLoaiSp)
                            {
                                _loaiSpService.Update(editedLoaiSp);
                                MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi lưu danh mục: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            e.Cancel = true; // Hủy bỏ chỉnh sửa nếu có lỗi
                        }
                        finally
                        {
                            _isAddingLoaiSp = false;
                            _isUpdatingLoaiSp = false;
                            LoadLoaiSps();
                            UpdateLoaiSpButtonStates();
                        }
                    }
                }
            }
        }

        private void btnSua_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
} 