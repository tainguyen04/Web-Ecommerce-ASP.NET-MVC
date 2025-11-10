using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class GioHang
    {
        public int Id { get; set; }
        public int? KhachHangId { get; set; }
        public KhachHang? KhachHang { get; set; } 
        public string? SessionId { get; set; }
        public DateTime NgayTao { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
    }
}
