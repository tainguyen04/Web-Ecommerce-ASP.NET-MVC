using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services.Interfaces;
using QLCHBanDienThoaiMoi.Helpers;

namespace QLCHBanDienThoaiMoi.Controllers
{
	public class AccountController : Controller
	{
		private readonly ITaiKhoanService _taiKhoanService;
		private readonly SessionHelper _sessionHelper;

		public AccountController(ITaiKhoanService taiKhoanService, SessionHelper sessionHelper)
		{
			_taiKhoanService = taiKhoanService;
			_sessionHelper = sessionHelper;
		}

		// GET: Login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		// POST: Login
		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			var user = _taiKhoanService.DangNhap(username, password);
			if (user == null)
			{
				ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
				return View();
			}

			// CHỖ NÀY ĐÃ SỬA: Chỉ để 1 Claim Role thôi, dùng tên chuỗi
			string roleName = user.VaiTro switch
			{
				VaiTro.KhachHang => "KhachHang",
				VaiTro.NhanVien => "NhanVien",
				VaiTro.Admin => "Admin",
				_ => "KhachHang"
			};

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.TenDangNhap),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim("UserId", user.Id.ToString()),
				new Claim("KhachHangId", user.KhachHang?.Id.ToString() ?? ""),
				new Claim(ClaimTypes.Role, roleName)   // ← Chỉ để dòng này thôi
            };

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
				});

			TempData["ThongBao"] = "Đăng nhập thành công!";
			return RedirectToAction("Index", "Home");
		}

		// Helper (có thể xóa nếu không dùng)
		private string GetRoleName(int vaiTro) => vaiTro switch
		{
			0 => "KhachHang",
			1 => "NhanVien",
			2 => "Admin",
			_ => "KhachHang"
		};

		// GET: Register
		[HttpGet]
		public IActionResult Register() => View();

		// POST: Register
		[HttpPost]
		public async Task<IActionResult> Register(
			string username,
			string password,
			string confirmPassword,
			string tenKH,
			string diachi,
			string sdt,
			string email)
		{
			var sessionId = _sessionHelper.EnsureSessionIdExists();

			if (password != confirmPassword)
			{
				ViewBag.Error = "Mật khẩu xác nhận không khớp!";
				return View();
			}

			if (await _taiKhoanService.KiemTraTenDangNhap(username))
			{
				ViewBag.Error = "Tên đăng nhập đã tồn tại!";
				return View();
			}

			var tk = new TaiKhoan
			{
				TenDangNhap = username,
				MatKhau = password,
				VaiTro = VaiTro.KhachHang   // ← ĐÃ SỬA: từ VaiTro.User → VaiTro.KhachHang
			};

			var kh = new KhachHang
			{
				TenKhachHang = tenKH,
				DiaChi = diachi,
				Email = email,
				SoDienThoai = sdt
			};

			bool success = await _taiKhoanService.DangKyAsync(tk, kh, sessionId);
			if (!success)
			{
				ViewBag.Error = "Không thể đăng ký. Vui lòng thử lại!";
				return View();
			}

			TempData["ThongBao"] = "Đăng ký thành công!";
			return RedirectToAction("Login");
		}

		// Đăng xuất
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}
	}
}