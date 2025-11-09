using System.ComponentModel.DataAnnotations.Schema;

namespace QLCHBanDienThoaiMoi.Models
{
    public class ChiTietGioHang
    {
        public int Id { get; set; }
        public int GioHangId { get; set; }
        public GioHang GioHang { get; set; } = null!;
        public int SanPhamId { get; set; }
        public SanPham SanPham { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        [NotMapped]
        public decimal TongTien => SoLuong * DonGia;

    }
}
