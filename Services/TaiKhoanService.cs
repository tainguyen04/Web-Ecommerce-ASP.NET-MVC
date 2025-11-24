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

		public TaiKhoanService(ApplicationDbContext context, IGioHangService gioHangService)
		{
			_context = context;
			_gioHangService = gioHangService;
		}

		// ============================
		// Mã hóa mật khẩu SHA256
		// ============================
		public static string HashPasswordSHA256(string password)
		{
			using var sha = SHA256.Create();
			var bytes = Encoding.UTF8.GetBytes(password);
			var hash = sha.ComputeHash(bytes);
			return string.Concat(Array.ConvertAll(hash, b => b.ToString("x2")));
		}

		// ============================
		// Đăng nhập
		// ============================
		public TaiKhoan? DangNhap(string username, string password)
		{
			string hashed = HashPasswordSHA256(password);
			return _context.TaiKhoan
				.FirstOrDefault(t => t.TenDangNhap == username && t.MatKhau == hashed);
		}

		// ============================
		// Đăng ký tài khoản khách hàng
		// ============================
		public async Task<bool> DangKyAsync(TaiKhoan tk, KhachHang kh, string? sessionId)
		{
			if (await KiemTraTenDangNhap(tk.TenDangNhap))
				return false;

			tk.MatKhau = HashPasswordSHA256(tk.MatKhau);
			tk.VaiTro = VaiTro.KhachHang;

			await _context.TaiKhoan.AddAsync(tk);
			await _context.SaveChangesAsync();   // ← Lưu để có Id

			// ĐÃ SỬA: Gán navigation property thay vì TaiKhoanId
			kh.TaiKhoan = tk;
			// hoặc nếu bạn có TaiKhoanId thì: kh.TaiKhoanId = tk.Id;

			await _context.KhachHang.AddAsync(kh);
			await _context.SaveChangesAsync();

			// Tạo giỏ hàng
			await _gioHangService.CreateGioHangAsync(null, kh.Id);

			if (!string.IsNullOrEmpty(sessionId))
				await _gioHangService.MergeCartAsync(sessionId, kh.Id);

			return true;
		}

		// ============================
		// Kiểm tra tên đăng nhập tồn tại
		// ============================
		public async Task<bool> KiemTraTenDangNhap(string username)
		{
			return await _context.TaiKhoan.AnyAsync(x => x.TenDangNhap == username);
		}

		public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
		{
			return await _context.TaiKhoan
				.Include(t => t.KhachHang)
				.Include(t => t.NhanVien)
				.ToListAsync();
		}

		public async Task<TaiKhoan?> GetTaiKhoanByIdAsync(int? id)
		{
			if (id == null) return null;
			return await _context.TaiKhoan
				.Include(t => t.KhachHang)
				.Include(t => t.NhanVien)
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		// ============================
		// Tạo tài khoản mới (dùng trong Admin)
		// ============================
		public async Task<bool> CreateTaiKhoanAsync(TaiKhoan taiKhoan)
		{
			try
			{
				if (await _context.TaiKhoan.AnyAsync(t => t.TenDangNhap == taiKhoan.TenDangNhap))
					return false;

				taiKhoan.MatKhau = HashPasswordSHA256(taiKhoan.MatKhau);

				if (taiKhoan.VaiTro == VaiTro.KhachHang)
				{
					taiKhoan.KhachHang = new KhachHang();
				}
				else // NhanVien hoặc Admin
				{
					taiKhoan.NhanVien = new NhanVien();
				}

				_context.TaiKhoan.Add(taiKhoan);
				return await _context.SaveChangesAsync() > 0;
			}
			catch
			{
				return false;
			}
		}

		// ============================
		// Cập nhật vai trò tài khoản (Admin dùng)
		// ============================
		public async Task<bool> UpdateTaiKhoanAsync(int id, VaiTro newVaiTro)
		{
			try
			{
				var tk = await GetTaiKhoanByIdAsync(id);
				if (tk == null) return false;

				var oldVaiTro = tk.VaiTro;
				if (oldVaiTro == newVaiTro) return true;

				tk.VaiTro = newVaiTro;

				// Chuyển từ Khách hàng → Nhân viên/Admin
				if (oldVaiTro == VaiTro.KhachHang && (newVaiTro == VaiTro.NhanVien || newVaiTro == VaiTro.Admin))
				{
					if (tk.KhachHang != null)
					{
						_context.KhachHang.Remove(tk.KhachHang);
						tk.KhachHang = null;
					}
					if (tk.NhanVien == null)
						tk.NhanVien = new NhanVien();
				}
				// Chuyển từ Nhân viên/Admin → Khách hàng
				else if ((oldVaiTro == VaiTro.NhanVien || oldVaiTro == VaiTro.Admin) && newVaiTro == VaiTro.KhachHang)
				{
					if (tk.NhanVien != null)
					{
						_context.NhanVien.Remove(tk.NhanVien);
						tk.NhanVien = null;
					}
					if (tk.KhachHang == null)
						tk.KhachHang = new KhachHang();
				}

				_context.TaiKhoan.Update(tk);
				return await _context.SaveChangesAsync() > 0;
			}
			catch
			{
				return false;
			}
		}

		// ============================
		// Xóa tài khoản
		// ============================
		public async Task<bool> DeleteTaiKhoanAsync(int? id)
		{
			var tk = await GetTaiKhoanByIdAsync(id);
			if (tk == null) return false;

			if (tk.NhanVien != null) _context.NhanVien.Remove(tk.NhanVien);
			if (tk.KhachHang != null) _context.KhachHang.Remove(tk.KhachHang);

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
			tk.TrangThai = TrangThaiTaiKhoan.Active;
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> ResetMatKhauAsync(int id, string newPassword)
		{
			var tk = await _context.TaiKhoan.FindAsync(id);
			if (tk == null) return false;
			tk.MatKhau = HashPasswordSHA256(newPassword);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}