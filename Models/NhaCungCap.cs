namespace QLCHBanDienThoaiMoi.Models
{
    public class NhaCungCap
    {
        public int Id { get; set; }
        public string? TenNCC { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
    }
}
