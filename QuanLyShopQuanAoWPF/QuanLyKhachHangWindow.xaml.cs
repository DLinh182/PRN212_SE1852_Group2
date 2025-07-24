using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BusinessObject;
using System;
using System.Linq;

namespace QuanLyShopQuanAoWPF
{
    public partial class QuanLyKhachHangWindow : Window
    {
        private ObservableCollection<Khachhang> dsKhach = new ObservableCollection<Khachhang>();
        private IKhachHangService khachHangService = new KhachHangService();
        private Khachhang khachDangChon = null;
        // private bool isThemMoi = false; // Xóa biến này

        public QuanLyKhachHangWindow()
        {
            InitializeComponent();
            dgKhachHang.ItemsSource = dsKhach;
            Loaded += QuanLyKhachHangWindow_Loaded;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnReset.Click += BtnReset_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            dgKhachHang.SelectionChanged += DgKhachHang_SelectionChanged;
        }

        private void QuanLyKhachHangWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDanhSach();
            cboGioiTinh.ItemsSource = new[] { "Nam", "Nữ", "Khác" };
        }

        private void LoadDanhSach()
        {
            dsKhach.Clear();
            foreach (var kh in khachHangService.GetAll())
                dsKhach.Add(kh);
            dgKhachHang.SelectedIndex = -1;
            ClearForm();
        }

        private void ClearForm()
        {
            txtTenKH.Text = "";
            cboGioiTinh.SelectedIndex = -1;
            txtNamSinh.Text = "";
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            khachDangChon = null;
        }

        private void DgKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKhachHang.SelectedItem is Khachhang kh)
            {
                khachDangChon = kh;
                txtTenKH.Text = kh.TenKh;
                cboGioiTinh.Text = kh.GioiTinhKh;
                txtNamSinh.Text = kh.NamSinhKh?.ToString() ?? "";
                txtSDT.Text = kh.Sdtkh;
                txtDiaChi.Text = kh.DiaChiKh;
                // btnLuu.IsEnabled = false; // Xóa biến này
                // isThemMoi = false; // Xóa biến này
            }
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên và SĐT!");
                return;
            }
            int? namSinh = null;
            if (int.TryParse(txtNamSinh.Text, out int ns)) namSinh = ns;
            var kh = new Khachhang
            {
                TenKh = txtTenKH.Text.Trim(),
                GioiTinhKh = cboGioiTinh.Text,
                NamSinhKh = namSinh,
                Sdtkh = txtSDT.Text.Trim(),
                DiaChiKh = txtDiaChi.Text.Trim()
            };
            khachHangService.Add(kh);
            LoadDanhSach();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if (khachDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenKH.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên và SĐT!");
                return;
            }
            int? namSinh = null;
            if (int.TryParse(txtNamSinh.Text, out int ns)) namSinh = ns;
            var kh = new Khachhang
            {
                MaKh = khachDangChon.MaKh,
                TenKh = txtTenKH.Text.Trim(),
                GioiTinhKh = cboGioiTinh.Text,
                NamSinhKh = namSinh,
                Sdtkh = txtSDT.Text.Trim(),
                DiaChiKh = txtDiaChi.Text.Trim()
            };
            khachHangService.Update(kh);
            LoadDanhSach();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (khachDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xoá.");
                return;
            }
            if (MessageBox.Show("Bạn chắc chắn muốn xoá khách hàng này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    khachHangService.Delete(khachDangChon.MaKh);
                    LoadDanhSach();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            LoadDanhSach();
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();
            var ketQua = khachHangService.GetAll().Where(kh => (kh.TenKh ?? "").ToLower().Contains(keyword));
            dsKhach.Clear();
            foreach (var kh in ketQua)
                dsKhach.Add(kh);
        }
    }
} 