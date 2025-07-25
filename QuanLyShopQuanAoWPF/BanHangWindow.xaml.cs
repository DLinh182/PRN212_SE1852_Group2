using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessObject;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyShopQuanAoWPF
{
    public partial class BanHangWindow : Window
    {
        private ObservableCollection<GioHangItem> gioHang = new ObservableCollection<GioHangItem>();
        private ISanPhamService sanPhamService = new SanPhamService();
        private INhanVienService nhanVienService = new NhanVienService();
        private ILoaiSpService loaiSpService = new LoaiSpService();
        private IKhachHangService khachHangService = new KhachHangService();
        private IHoaDonService hoaDonService = new HoaDonService();
        private ICthdService cthdService = new CthdService();
    

        public BanHangWindow()
        {
            InitializeComponent();
            dgGioHang.ItemsSource = gioHang;
            Loaded += BanHangWindow_Loaded;
            txtTienNhan.TextChanged += TinhTienThua;
            txtGiamGia.TextChanged += TinhTienThua;
            cbo_HTGiamGia.SelectionChanged += TinhTienThua;
        }

        private void BanHangWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNhanVien();
            LoadSanPham();
            LoadHinhThucThanhToan();
            LoadHinhThucGiamGia();
            LoadKhachHang();
        }



        private void LoadKhachHang()
        {
            var dsKhach = khachHangService.GetAll();
            cboKhachHang.ItemsSource = dsKhach;
        }

        private void cboKhachHang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboKhachHang.SelectedItem is Khachhang kh)
            {
                txtSDTKH.Text = kh.Sdtkh; 
            }
            else
            {
                txtSDTKH.Text = "";
            }
        }

        private void LoadNhanVien()
        {
            var dsNhanVien = nhanVienService.GetAll();
            cbo_NhanVien.ItemsSource = dsNhanVien;
            cbo_NhanVien.DisplayMemberPath = "TenNv";
            cbo_NhanVien.SelectedValuePath = "MaNv";
        }

        private void LoadSanPham()
        {
            wpSanPham.Children.Clear();
            var dsSanPham = sanPhamService.GetAll();
            foreach (var sp in dsSanPham)
            {
                var stack = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(5) };
                var ten = new TextBlock
                {
                    Text = sp.TenSp,
                    TextWrapping = TextWrapping.Wrap,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                stack.Children.Add(ten);
                var gia = new TextBlock
                {
                    Text = "Giá: " + (sp.GiaBan?.ToString("N0") ?? "0"),
                    Foreground = System.Windows.Media.Brushes.Red,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 14
                };
                stack.Children.Add(gia);
                var btn = new Button
                {
                    Content = stack,
                    Tag = sp,
                    Width = 170,
                    Margin = new Thickness(10),
                    Padding = new Thickness(5)
                };
                btn.Click += (s, e) => MoThongTinSanPham(sp);
                wpSanPham.Children.Add(btn);
            }
        }

        private void MoThongTinSanPham(Sanpham sp)
        {
            var win = new ThongTinSPWindow();
            win.HienThiThongTin(sp, (kichCo, soLuong) => {
                // Thêm vào giỏ hàng với size và số lượng chọn từ popup
                var item = gioHang.FirstOrDefault(x => x.MaSP == sp.MaSp && x.KichCo == kichCo);
                if (item != null)
                {
                    item.SoLuong += soLuong;
                    item.ThanhTien = item.SoLuong * (sp.GiaBan ?? 0);
                }
                else
                {
                    gioHang.Add(new GioHangItem
                    {
                        MaSP = sp.MaSp,
                        TenSP = sp.TenSp,
                        KichCo = kichCo,
                        SoLuong = soLuong,
                        GiaBan = sp.GiaBan ?? 0,
                        ThanhTien = (sp.GiaBan ?? 0) * soLuong
                    });
                }
                CapNhatTongTien();
            });
            win.ShowDialog();
        }

        private void LoadHinhThucThanhToan()
        {
            cbo_HTThanhToan.ItemsSource = new[]
            {
                "Tiền mặt",
                "Chuyển khoản",
                "Thẻ ngân hàng",
                "Ví điện tử"
            };
            cbo_HTThanhToan.SelectedIndex = 0;
        }

        private void LoadHinhThucGiamGia()
        {
            cbo_HTGiamGia.ItemsSource = new[]
            {
                "%",
                "VNĐ"
            };
            cbo_HTGiamGia.SelectedIndex = 0;
        }


        private void BtnXoaKhoiGio_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is GioHangItem item)
            {
                gioHang.Remove(item);
                CapNhatTongTien();
            }
        }

        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            gioHang.Clear();
            txtSDTKH.Text = "";
            txtGiamGia.Text = "";
            cbo_HTGiamGia.SelectedIndex = -1;
            cbo_NhanVien.SelectedIndex = -1;
            cbo_HTThanhToan.SelectedIndex = -1;
            txtTienNhan.Text = "";
            lbTongTien.Text = "";
            lbTienThua.Text = "";
            txtGhiChu.Text = "";
            chk_ChuaThanhToan.IsChecked = false;
            chk_DatCoc.IsChecked = false;
            tbThongBao.Text = "";
        }

        private async void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            tbThongBao.Text = "";
            tbThongBaoThanhCong.Visibility = Visibility.Collapsed;
            if (gioHang.Count == 0)
            {
                tbThongBao.Text = "Vui lòng chọn sản phẩm!";
                return;
            }
            if (cboKhachHang.Text.Trim() == "" || string.IsNullOrWhiteSpace(txtSDTKH.Text))
            {
                tbThongBao.Text = "Vui lòng nhập đầy đủ thông tin khách hàng!";
                return;
            }
            if (cbo_NhanVien.SelectedIndex == -1)
            {
                tbThongBao.Text = "Vui lòng chọn nhân viên bán hàng!";
                return;
            }
            if (cbo_HTThanhToan.SelectedIndex == -1)
            {
                tbThongBao.Text = "Vui lòng chọn hình thức thanh toán!";
                return;
            }
            double tongTien = TinhTongThanhToanSauGiamGia();
            double tienNhan = 0;
            if (!double.TryParse(txtTienNhan.Text, out tienNhan) || tienNhan < tongTien)
            {
                tbThongBao.Text = "Tiền nhận không hợp lệ hoặc nhỏ hơn tổng tiền!";
                return;
            }
            // Lưu hóa đơn
            var hd = new Hoadon
            {
                MaNv = (cbo_NhanVien.SelectedItem as Nhanvien)?.MaNv,
                MaKh = (cboKhachHang.SelectedItem as Khachhang)?.MaKh,
                NgayBan = DateTime.Now,
                TongThanhToan = tongTien,
                TrangThai = chk_ChuaThanhToan.IsChecked == true ? "Chưa thanh toán" : (chk_DatCoc.IsChecked == true ? "Đặt cọc" : "Đã thanh toán"),
                HinhThucThanhToan = cbo_HTThanhToan.SelectedItem?.ToString(),
                GhiChu = txtGhiChu.Text
            };
            hoaDonService.Add(hd);
            // Lấy mã hóa đơn vừa thêm (giả sử tự tăng, lấy max)
            int maHdMoi = hoaDonService.GetAll().Max(x => x.MaHd);
            // Lưu chi tiết hóa đơn
            foreach (var item in gioHang)
            {
                var ct = new Cthd
                {
                    MaHd = maHdMoi,
                    MaSp = item.MaSP,
                    KichCo = item.KichCo,
                    SoLuong = item.SoLuong,
                    GiaBan = item.GiaBan,
                    ThanhTien = item.ThanhTien
                };
                cthdService.Add(ct);
            }
            tbThongBaoThanhCong.Text = "Thanh toán thành công!";
            tbThongBaoThanhCong.Visibility = Visibility.Visible;
            await Task.Delay(2000);
            tbThongBaoThanhCong.Visibility = Visibility.Collapsed;
            BtnLamMoi_Click(null, null);
        }

        private double TinhTongThanhToanSauGiamGia()
        {
            double tong = TinhTongTien();
            double giam = 0;
            double.TryParse(txtGiamGia.Text, out giam);
            if (cbo_HTGiamGia.SelectedItem?.ToString() == "%")
            {
                tong = tong - tong * giam / 100.0;
            }
            else 
            {
                tong = tong - giam;
            }
            return tong < 0 ? 0 : tong;
        }

        private void TinhTienThua(object sender, EventArgs e)
        {
            double tongThanhToan = TinhTongThanhToanSauGiamGia();
            double tienNhan = 0;
            double.TryParse(txtTienNhan.Text, out tienNhan);
            double tienThua = tienNhan - tongThanhToan;
            lbTienThua.Text = tienThua > 0 ? tienThua.ToString("N0") : "0";
        }

        private void CapNhatTongTien()
        {
            lbTongTien.Text = TinhTongTien().ToString("N0");
            TinhTienThua(null, null);
        }

        private double TinhTongTien()
        {
            double tong = 0;
            foreach (var item in gioHang)
            {
                tong += item.ThanhTien;
            }
            return tong;
        }
    }

    // Mẫu class item giỏ hàng, bạn cần thay bằng class thực tế của bạn
    public class GioHangItem
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string KichCo { get; set; }
        public int SoLuong { get; set; }
        public double GiaBan { get; set; }
        public double ThanhTien { get; set; }
    }
} 