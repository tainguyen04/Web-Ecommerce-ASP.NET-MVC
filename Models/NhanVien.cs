namespace QLCHBanDienThoaiMoi.Models
{
    public class NhanVien
    {
        public int Id { get; set; }
        public string? TenNhanVien { get; set; }
        public string? SoDienThoai { get; set; }
        public TaiKhoan? TaiKhoan { get; set; } 
        public ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();
    }
}
