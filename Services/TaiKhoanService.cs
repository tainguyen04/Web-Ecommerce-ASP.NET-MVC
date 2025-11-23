using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.DTO;
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

        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _context.TaiKhoan.ToListAsync();
        }

        public async Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.TaiKhoan.FindAsync(id);
        }

        public async Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            try
            {
                
                var existingTaiKhoan = await _context.TaiKhoan.AnyAsync(tk => tk.TenDangNhap == taiKhoan.TenDangNhap);
                if (existingTaiKhoan) return false;
                taiKhoan.MatKhau = HashPassword(taiKhoan.MatKhau);
                if (taiKhoan.VaiTro == VaiTro.User)
                    taiKhoan.KhachHang = new KhachHang();
                else if(taiKhoan.VaiTro == VaiTro.Admin)
                    taiKhoan.NhanVien = new NhanVien();

                _context.TaiKhoan.Add(taiKhoan);
                return await _context.SaveChangesAsync() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> UpdateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            try
            {
                _context.TaiKhoan.Update(taiKhoan);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaiKhoanAsync(int? id)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if(tk == null) return false;
            _context.TaiKhoan.Remove(tk);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> LockTaiKhoanAsync(int id)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.TrangThai = TrangThaiTaiKhoan.Locked;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnlockTaiKhoanAsync(int id)
        {
            var tk = await _context.TaiKhoan.FindAsync(id);
            if (tk == null) return false;
            tk.TrangThai = TrangThaiTaiKhoan.Locked;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ResetMatKhauAsync(int id, string passWord)
        {
            var tk = await _context.TaiKhoan.FindAsync(id); 
            if (tk == null) return false;
            tk.MatKhau = HashPassword(passWord);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
