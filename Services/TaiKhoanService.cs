using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace QLCHBanDienThoaiMoi.Services
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly ApplicationDbContext _context;
        private readonly IGioHangService _gioHangService;

        public TaiKhoanService(ApplicationDbContext context,IGioHangService gioHangService)
        {
            _context = context;
            _gioHangService = gioHangService;
        }

        // ============================
        // 🔐 Hàm mã hóa mật khẩu
        // ============================
        private string HashPassword(string password)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // ============================
        // 🔐 Đăng nhập
        // ============================
        public TaiKhoan? DangNhap(string username, string password)
        {
            string passHash = HashPassword(password);

            return _context.TaiKhoan
                .Include(x => x.KhachHang)
                .Include(x => x.NhanVien)
                .FirstOrDefault(x =>
                    x.TenDangNhap == username &&
                    x.MatKhau == passHash &&
                    x.TrangThai == TrangThaiTaiKhoan.Active);
        }

        // ============================
        // 📝 Đăng ký tài khoản khách hàng
        // ============================
        public async Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh,string sessionId)
        {
            if (await KiemTraTenDangNhap(tk.TenDangNhap))
                return false;

            tk.MatKhau = HashPassword(tk.MatKhau);
            tk.VaiTro = VaiTro.User;

            await _context.TaiKhoan.AddAsync(tk);
            await _context.SaveChangesAsync();

            // Gán tài khoản vào khách hàng
            kh.TaiKhoan = tk;
            await _context.KhachHang.AddAsync(kh);
            await _context.SaveChangesAsync();
            //Tạo giỏ hàng
            await _gioHangService.CreateGioHangAsync(null,kh.Id);
            if (string.IsNullOrEmpty(sessionId))
                await _gioHangService.MergeCartAsync(sessionId, kh.Id);
            return true;
        }

        // ============================
        // 🔍 Kiểm tra tên đăng nhập
        // ============================
        public async Task<bool> KiemTraTenDangNhap(string username)
        {
            return await _context.TaiKhoan
                .AnyAsync(x => x.TenDangNhap == username);
        }
    }
}
