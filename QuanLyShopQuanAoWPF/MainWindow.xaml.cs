﻿using System.Windows;

namespace QuanLyShopQuanAoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _role;
        public MainWindow(string role = "user")
        {
            InitializeComponent();
            _role = role;
            ucMenu.SetRole(_role);
        }

        private void BtnQuanLySanPham_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình quản lý sản phẩm
        }

        private void BtnQuanLyNhanVien_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình quản lý nhân viên
        }

        private void BtnQuanLyKhachHang_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình quản lý khách hàng
        }

        private void BtnQuanLyHoaDon_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình quản lý hóa đơn
        }

        private void BtnQuanLyNhapKho_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình quản lý nhập kho
        }

        private void BtnDoanhThu_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình thống kê doanh thu
        }

        private void BtnBanHang_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Mở màn hình bán hàng
        }

        private void BtnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Xử lý đăng xuất
            this.Close();
        }
    }
}