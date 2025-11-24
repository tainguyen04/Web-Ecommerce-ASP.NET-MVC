USE [QLCHBanDienThoaiMoi];

GO
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai)
VALUES
('khachhang1', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 0, 1), -- 123456
('nhanvien1', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 1, 1), -- 123456
('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 2, 1); -- admin123

GO
-- 2. KhachHang (Id trùng với TaiKhoan.Id)

INSERT INTO KhachHang (Id, TenKhachHang, DiaChi, SoDienThoai, Email)

VALUES

(1, 'Nguyen Van A', '123 Le Loi, TP. HCM', '0909123456', 'a@example.com'),

(2, 'Tran Thi B', '456 Tran Hung Dao, TP. HCM', '0909988776', 'b@example.com');

GO
-- 3. NhanVien (Id trùng với TaiKhoan.Id)

INSERT INTO NhanVien (Id, TenNhanVien, SoDienThoai)

VALUES

(2, 'Le Van C', '0912345678'),

(3, 'Pham Thi D', '0987654321');

GO
-- 4. DanhMucSanPham

INSERT INTO DanhMucSanPham (TenDanhMuc)

VALUES

('Điện thoại'),

('Laptop'),

('Phụ kiện');

GO
-- 5. KhuyenMai

INSERT INTO KhuyenMai (TenKhuyenMai, MoTa, LoaiKhuyenMai, NgayBatDau, NgayKetThuc, GiaTri)

VALUES

('KM 10%', 'Giảm 10% cho đơn hàng trên 1 triệu', 'Phần trăm', '2025-11-01', '2025-12-31', 10.0),

('KM 200k', 'Giảm 200k cho đơn hàng trên 5 triệu', 'Tiền mặt', '2025-11-15', '2025-12-31', 200.0);

GO
-- 6. NhaCungCap

INSERT INTO NhaCungCap (TenNCC, DiaChi, SoDienThoai, Email)

VALUES

('Công ty ABC', '123 ABC Street', '0912345678', 'abc@example.com'),

('Công ty XYZ', '456 XYZ Street', '0987654321', 'xyz@example.com');

GO
-- 7. SanPham

INSERT INTO SanPham (TenSanPham, GiaNhap, GiaBan, DanhMucId, KhuyenMaiId, HangSanXuat, MoTa, HinhAnh)
VALUES 
-- --- APPLE (Điện thoại & Tablet) ---
('iPhone 14', 18000000, 21000000, 1, 1, 'Apple', 'Thiết kế sang trọng, hiệu năng mạnh mẽ', 'iphone14.jpg'),
('iPhone 15', 22000000, 25000000, 1, NULL, 'Apple', 'Cổng Type-C mới, camera 48MP', 'iphone15.jpg'),
('iPhone 17 Pro Max', 30000000, 35000000, 1, NULL, 'Apple', 'Phiên bản Concept tương lai', 'iphone17.jpg'),
('iPad Air', 12000000, 14500000, 1, 2, 'Apple', 'Mỏng nhẹ, sức mạnh chip M1', 'ipad_air.jpg'),
('iPad Pro', 18000000, 21000000, 1, NULL, 'Apple', 'Màn hình 120Hz, hiệu năng đỉnh cao', 'ipad_pro.jpg'),

-- --- SAMSUNG (Điện thoại & Tablet) ---
('Samsung Galaxy S22', 10000000, 13000000, 1, 2, 'Samsung', 'Camera mắt thần bóng đêm', 'samsung_s22.jpg'),
('Samsung Galaxy S23', 14000000, 17000000, 1, 1, 'Samsung', 'Snapdragon 8 Gen 2 for Galaxy', 'samsung_s23.jpg'),
('Samsung Galaxy S25 Plus', 20000000, 24000000, 1, NULL, 'Samsung', 'Siêu phẩm dòng S mới nhất', 'samsunggalaxys25plus.jpeg'),
('Samsung Galaxy Tab S8', 13000000, 16000000, 1, NULL, 'Samsung', 'Máy tính bảng kèm bút S-Pen', 'tab_s8.jpg'),

-- --- XIAOMI (Điện thoại) ---
('Xiaomi Mi 12', 8000000, 10500000, 1, 2, 'Xiaomi', 'Thiết kế nhỏ gọn, sạc nhanh', 'xiaomi_mi12.jpg'),
('Xiaomi Mi 13', 11000000, 13500000, 1, 1, 'Xiaomi', 'Camera Leica chuyên nghiệp', 'xiaomi_mi13.jpg'),
('Xiaomi 15T Pro', 14000000, 16500000, 1, NULL, 'Xiaomi', 'Hiệu năng khủng, pin trâu', 'xiaomi15tprogold1.jpg'),

-- --- LAPTOP ---
('Dell Inspiron 15 3530', 13000000, 15500000, 2, 2, 'Dell', 'Laptop văn phòng bền bỉ, Core i5', 'dellinspiron153530i5.jpg'),
('MacBook Air M2', 23000000, 26000000, 2, 1, 'Apple', 'Thiết kế mới, chip M2 mạnh mẽ', 'macbook_air_m2.jpg'),
('MacBook Pro M2', 28000000, 32000000, 2, NULL, 'Apple', 'Hiệu năng chuyên nghiệp cho đồ họa', 'macbook_pro_m2.jpg'),

-- --- PHỤ KIỆN (Tai nghe, Đồng hồ, Sạc...) ---
('AirPods 2', 2000000, 2600000, 3, NULL, 'Apple', 'Tai nghe quốc dân, kết nối nhanh', 'airpods2.jpg'),
('AirPods 3', 3500000, 4200000, 3, NULL, 'Apple', 'Âm thanh vòm Spatial Audio', 'airpods.jpg'),
('AirPods Pro', 4500000, 5500000, 3, 1, 'Apple', 'Chống ồn chủ động, xuyên âm', 'airpods_pro.jpg'),
('Samsung Buds Live', 1200000, 1900000, 3, 2, 'Samsung', 'Thiết kế hạt đậu độc đáo', 'buds_live.jpg'),
('JBL Headphone', 1500000, 2200000, 3, NULL, 'JBL', 'Âm bass mạnh mẽ, pin lâu', 'jbl_headphone.jpg'),
('Apple Watch Series 9', 9000000, 10500000, 3, NULL, 'Apple', 'Đồng hồ thông minh, đo SpO2', 'watch9.jpg'),
('Samsung Galaxy Watch 5', 4000000, 5200000, 3, 2, 'Samsung', 'Theo dõi sức khỏe toàn diện', 'watch5.jpg'),
('Sạc dự phòng Anker', 600000, 950000, 3, NULL, 'Anker', 'Dung lượng 20000mAh, sạc nhanh', 'anker_powerbank.jpg'),
('Cáp sạc Type-C', 100000, 250000, 3, NULL, 'OEM', 'Dây cáp bền bỉ, truyền dữ liệu tốt', 'cap_typec.jpg'),
('Ốp lưng iPhone', 50000, 150000, 3, NULL, 'OEM', 'Ốp lưng silicon bảo vệ máy', 'oplung_iphone.jpg');
GO
-- 8. TonKho

INSERT INTO TonKho (SanPhamId, SoLuong)

VALUES

(1, 50),

(2, 30),

(3, 20);

GO
-- 9. GioHang

INSERT INTO GioHang (KhachHangId, SessionId, NgayTao)

VALUES

(1, 'sess001', GETDATE()),

(2, 'sess002', GETDATE());

GO
-- 10. ChiTietGioHang

INSERT INTO ChiTietGioHang (GioHangId, SanPhamId, Id, SoLuong, DonGia)

VALUES

(1, 1, 1, 2, 20000000),

(1, 2, 2, 1, 18000000),

(2, 3, 3, 1, 30000000);

GO
-- 11. HoaDonBan

INSERT INTO HoaDonBan (NgayBan, KhachHangId, NhanVienId, DiaChiNhanHang, TongTien, PhuongThucThanhToan, TrangThai)

VALUES

(GETDATE(), 1, 2, '123 Le Loi, TP. HCM', 58000000, 1, 1),

(GETDATE(), 2, 3, '456 Tran Hung Dao, TP. HCM', 30000000, 2, 0);

GO
-- 12. ChiTietHoaDonBan

INSERT INTO ChiTietHoaDonBan (HoaDonBanId, SanPhamId, Id, SoLuong, GiaBan, KhuyenMai)

VALUES

(1, 1, 1, 2, 20000000, 10.0),

(1, 2, 2, 1, 18000000, NULL),

(2, 3, 3, 1, 30000000, 200.0);

GO
-- 13. HoaDonNhap

INSERT INTO HoaDonNhap (NgayLap, NhaCungCapId, TongTien)

VALUES

(GETDATE(), 1, 50000000),

(GETDATE(), 2, 30000000);

GO
-- 14. ChiTietHoaDonNhap

INSERT INTO ChiTietHoaDonNhap (HoaDonNhapId, SanPhamId, SoLuong, GiaNhap)

VALUES

(1, 1, 50, 15000000),

(1, 2, 30, 12000000),

(2, 3, 20, 25000000);

GO
-- 15. PhieuBaoHanh

INSERT INTO PhieuBaoHanh (HoaDonBanId, SanPhamId, NgayLap, NgayHetHan, MoTa, TrangThai)

VALUES

(1, 1, GETDATE(), DATEADD(YEAR, 1, GETDATE()), 'Bảo hành chính hãng 1 năm', 1),

(2, 3, GETDATE(), DATEADD(YEAR, 1, GETDATE()), 'Bảo hành chính hãng 1 năm', 1);

GO 

