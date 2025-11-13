-- 1. Thêm dữ liệu vào bảng TaiKhoan
INSERT INTO [dbo].[TaiKhoan] (TenDangNhap, MatKhau, VaiTro, TrangThai)
VALUES 
('khach1', '123456', 0, 1),   -- VaiTro: 0=Khách hàng, 1=Nhân viên, 2=Admin
('khach2', '123456', 0, 1),
('nhanvien1', '123456', 1, 1),
('admin', 'admin123', 2, 1);

-- 2. Thêm dữ liệu vào bảng KhachHang (Id phải trùng với TaiKhoan.Id)
INSERT INTO [dbo].[KhachHang] (Id, TenKhachHang, DiaChi, SoDienThoai, Email)
VALUES 
(1, 'Nguyen Van A', '123 Le Loi, HCMC', '0909123456', 'a@example.com'),
(2, 'Tran Thi B', '456 Nguyen Trai, HCMC', '0909876543', 'b@example.com');

-- 3. Thêm dữ liệu vào bảng NhanVien (Id trùng với TaiKhoan.Id)
INSERT INTO [dbo].[NhanVien] (Id, TenNhanVien, SoDienThoai)
VALUES 
(3, 'Nguyen Van NV', '0912345678');

-- 4. Thêm dữ liệu vào bảng DanhMucSanPham
INSERT INTO [dbo].[DanhMucSanPham] (TenDanhMuc)
VALUES 
('Điện thoại'), 
('Phụ kiện'), 
('Tablet');

-- 5. Thêm dữ liệu vào bảng KhuyenMai
INSERT INTO [dbo].[KhuyenMai] (TenKhuyenMai, MoTa, LoaiKhuyenMai, NgayBatDau, NgayKetThuc, GiaTri)
VALUES 
('Giảm 10%', 'Khuyến mãi giảm 10% cho sản phẩm điện thoại', 'Phần trăm', '2025-11-01', '2025-12-01', 10.00),
('Mua 1 tặng 1', 'Khuyến mãi mua 1 tặng 1 phụ kiện', 'Sản phẩm', '2025-11-05', '2025-11-30', 0.00);

-- 6. Thêm dữ liệu vào bảng NhaCungCap
INSERT INTO [dbo].[NhaCungCap] (TenNCC, DiaChi, SoDienThoai, Email)
VALUES 
('Cty ABC', '123 Nguyen Van Cu, HCMC', '0909123456', 'abc@example.com'),
('Cty XYZ', '456 Tran Hung Dao, HCMC', '0909876543', 'xyz@example.com');

-- 7. Thêm dữ liệu vào bảng SanPham
INSERT INTO [dbo].[SanPham] (TenSanPham, GiaNhap, GiaBan, SoLuongTon, DanhMucId, KhuyenMaiId, HangSanXuat, MoTa, HinhAnh)
VALUES 
('iPhone 15', 20000000, 25000000, 50, 1, 1, 'Apple', 'Điện thoại cao cấp', 'iphone15.jpg'),
('Samsung Galaxy S23', 15000000, 20000000, 30, 1, NULL, 'Samsung', 'Điện thoại Android', 'samsung_s23.jpg'),
('Tai nghe AirPods', 2000000, 2500000, 100, 2, 2, 'Apple', 'Tai nghe không dây', 'airpods.jpg');

-- 8. Thêm dữ liệu vào bảng GioHang
INSERT INTO [dbo].[GioHang] (KhachHangId, SessionId, NgayTao)
VALUES 
(1, 'SESSION1', GETDATE()),
(2, 'SESSION2', GETDATE());

-- 9. Thêm dữ liệu vào bảng HoaDonBan
INSERT INTO [dbo].[HoaDonBan] (NgayBan, KhachHangId, NhanVienId, PhuongThucThanhToan, TrangThai)
VALUES 
(GETDATE(), 1, 3, 1, 1),
(GETDATE(), 2, 3, 2, 1);

-- 10. Thêm dữ liệu vào bảng ChiTietGioHang
INSERT INTO [dbo].[ChiTietGioHang] (GioHangId, SanPhamId, Id, SoLuong, DonGia)
VALUES 
(1, 1, 1, 1, 25000000),
(1, 3, 2, 2, 2500000),
(2, 2, 3, 1, 20000000);

-- 11. Thêm dữ liệu vào bảng ChiTietHoaDonBan
INSERT INTO [dbo].[ChiTietHoaDonBan] (HoaDonBanId, SanPhamId, SoLuong, GiaBan)
VALUES 
(1, 1, 1, 25000000),
(1, 3, 2, 2500000),
(2, 2, 1, 20000000);

-- 12. Thêm dữ liệu vào bảng HoaDonNhap
INSERT INTO [dbo].[HoaDonNhap] (NgayLap, NCCId)
VALUES 
(GETDATE(), 1),
(GETDATE(), 2);

-- 13. Thêm dữ liệu vào bảng ChiTietHoaDonNhap
INSERT INTO [dbo].[ChiTietHoaDonNhap] (HoaDonNhapId, SanPhamId, SoLuong, GiaNhap)
VALUES 
(1, 1, 50, 20000000),
(1, 3, 100, 2000000),
(2, 2, 30, 15000000);

-- 14. Thêm dữ liệu vào bảng PhieuBaoHanh
INSERT INTO [dbo].[PhieuBaoHanh] (HoaDonBanId, SanPhamId, NgayLap, NgayHetHan, MoTa, TrangThai)
VALUES
(1, 1, GETDATE(), DATEADD(YEAR, 1, GETDATE()), 'Bảo hành 1 năm', 1),
(1, 3, GETDATE(), DATEADD(MONTH, 6, GETDATE()), 'Bảo hành 6 tháng', 1);
-- Thêm 20 sản phẩm mẫu
INSERT INTO [dbo].[SanPham] (TenSanPham, GiaNhap, GiaBan, SoLuongTon, DanhMucId, KhuyenMaiId, HangSanXuat, MoTa, HinhAnh)
VALUES 
('iPhone 15', 20000000, 25000000, 50, 1, 1, 'Apple', 'Điện thoại cao cấp', 'iphone15.jpg'),
('iPhone 14', 15000000, 20000000, 30, 1, 1, 'Apple', 'Điện thoại thế hệ trước', 'iphone14.jpg'),
('Samsung Galaxy S23', 15000000, 20000000, 40, 1, NULL, 'Samsung', 'Điện thoại Android cao cấp', 'samsung_s23.jpg'),
('Samsung Galaxy S22', 12000000, 17000000, 35, 1, NULL, 'Samsung', 'Điện thoại Android thế hệ trước', 'samsung_s22.jpg'),
('Xiaomi Mi 13', 8000000, 12000000, 50, 1, NULL, 'Xiaomi', 'Điện thoại Android tầm trung', 'xiaomi_mi13.jpg'),
('Xiaomi Mi 12', 7000000, 10000000, 60, 1, NULL, 'Xiaomi', 'Điện thoại Android giá rẻ', 'xiaomi_mi12.jpg'),
('iPad Pro', 20000000, 25000000, 20, 3, NULL, 'Apple', 'Tablet cao cấp', 'ipad_pro.jpg'),
('iPad Air', 15000000, 20000000, 25, 3, NULL, 'Apple', 'Tablet tầm trung', 'ipad_air.jpg'),
('Samsung Galaxy Tab S8', 12000000, 17000000, 30, 3, NULL, 'Samsung', 'Tablet Android', 'tab_s8.jpg'),
('AirPods Pro', 4000000, 5000000, 100, 2, 2, 'Apple', 'Tai nghe không dây cao cấp', 'airpods_pro.jpg'),
('AirPods 2', 2000000, 2500000, 150, 2, 2, 'Apple', 'Tai nghe không dây', 'airpods2.jpg'),
('Samsung Buds Live', 1500000, 2000000, 120, 2, NULL, 'Samsung', 'Tai nghe không dây', 'buds_live.jpg'),
('Tai nghe JBL', 800000, 1200000, 200, 2, NULL, 'JBL', 'Tai nghe phổ thông', 'jbl_headphone.jpg'),
('Ốp lưng iPhone', 200000, 300000, 300, 2, 2, 'Apple', 'Ốp lưng bảo vệ điện thoại', 'oplung_iphone.jpg'),
('Sạc dự phòng 10000mAh', 300000, 500000, 150, 2, NULL, 'Anker', 'Pin sạc dự phòng', 'anker_powerbank.jpg'),
('Cáp sạc Type-C', 100000, 200000, 500, 2, NULL, 'Baseus', 'Cáp sạc nhanh', 'cap_typec.jpg'),
('Samsung Galaxy Watch 5', 4000000, 5000000, 50, 2, NULL, 'Samsung', 'Đồng hồ thông minh', 'watch5.jpg'),
('Apple Watch Series 9', 8000000, 10000000, 30, 2, 1, 'Apple', 'Đồng hồ thông minh cao cấp', 'watch9.jpg'),
('MacBook Air M2', 25000000, 30000000, 15, 3, NULL, 'Apple', 'Laptop cao cấp', 'macbook_air_m2.jpg'),
('MacBook Pro M2', 40000000, 45000000, 10, 3, NULL, 'Apple', 'Laptop hiệu năng cao', 'macbook_pro_m2.jpg');
