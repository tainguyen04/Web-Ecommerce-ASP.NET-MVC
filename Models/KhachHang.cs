using QLCHBanDienThoaiMoi.Models;

public class KhachHang
{
	public int Id { get; set; }
	public string TenKhachHang { get; set; } = null!;
	public string? DiaChi { get; set; }
	public string? SoDienThoai { get; set; }
	public string? Email { get; set; }

	// Navigation
	public TaiKhoan? TaiKhoan { get; set; }

	// Thêm dòng này nếu muốn có FK rõ ràng
	public int? TaiKhoanId { get; set; }   // ← Thêm dòng này
}