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
		public static string HashPasswordSHA256(string password)
		{
			using (var sha = SHA256.Create())
			{
				var bytes = Encoding.UTF8.GetBytes(password);
				var hash = sha.ComputeHash(bytes);

				// Convert to hex
				StringBuilder sb = new StringBuilder();
				foreach (var b in hash)
					sb.Append(b.ToString("x2"));

				return sb.ToString();
			}
		}

		// ============================
		// 🔐 Đăng nhập
		// ============================
		public TaiKhoan? DangNhap(string username, string password)
		{
			string hashed = HashPasswordSHA256(password);

			return _context.TaiKhoan
				.FirstOrDefault(t => t.TenDangNhap == username
								  && t.MatKhau == hashed);
		}


		// ============================
		// 📝 Đăng ký tài khoản khách hàng
		// ============================
		public async Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh,string sessionId)
        {
            if (await KiemTraTenDangNhap(tk.TenDangNhap))
                return false;

            tk.MatKhau = HashPasswordSHA256(tk.MatKhau);
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
            return await _context.TaiKhoan
                                .Include(kh => kh.KhachHang)
                                .Include(nv => nv.NhanVien)
                                .ToListAsync();
        }

        public async Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.TaiKhoan
                                .Include(kh => kh.KhachHang)
                                .Include (nv => nv.NhanVien)
                                .FirstOrDefaultAsync(tk => tk.Id == id);
        }

        public async Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan)
        {
            try
            {
                
                var existingTaiKhoan = await _context.TaiKhoan.AnyAsync(tk => tk.TenDangNhap == taiKhoan.TenDangNhap);
                if (existingTaiKhoan) return false;
                taiKhoan.MatKhau = HashPasswordSHA256(taiKhoan.MatKhau);
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

        public async Task<bool> UpdateTaiKhoanAsync(int id, VaiTro newVaiTro)
        {
            try
            {
                var existing = await GetTaiKhoanByIdAsync(id);
                if (existing == null) return false;
                var oldVaitro = existing.VaiTro;
                if(oldVaitro == newVaiTro) return true;
          
                existing.VaiTro = newVaiTro;
                if(oldVaitro == VaiTro.Admin && newVaiTro == VaiTro.User)
                {
                    if(existing.NhanVien != null)
                    {
                        _context.NhanVien.Remove(existing.NhanVien);
                        existing.NhanVien = null;
                    }
                    if(existing.KhachHang == null)
                        existing.KhachHang = new KhachHang();
                }else if(oldVaitro == VaiTro.User && newVaiTro == VaiTro.Admin)
                {
                    if (existing.KhachHang != null)
                    {
                        _context.KhachHang.Remove(existing.KhachHang);
                        existing.KhachHang = null;
                    }
                    if (existing.NhanVien == null)
                        existing.NhanVien = new NhanVien();
                }
                _context.TaiKhoan.Update(existing);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaiKhoanAsync(int? id)
        {
            var taiKhoan = await GetTaiKhoanByIdAsync(id);
            if(taiKhoan == null) return false;
            if(taiKhoan.NhanVien != null)
                _context.NhanVien.Remove(taiKhoan.NhanVien);
            else if(taiKhoan.KhachHang != null)
                _context.KhachHang.Remove(taiKhoan.KhachHang);

            _context.TaiKhoan.Remove(taiKhoan);
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
            tk.TrangThai = TrangThaiTaiKhoan.Active;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ResetMatKhauAsync(int id, string passWord)
        {
            var tk = await _context.TaiKhoan.FindAsync(id); 
            if (tk == null) return false;
            tk.MatKhau = HashPasswordSHA256(passWord);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
