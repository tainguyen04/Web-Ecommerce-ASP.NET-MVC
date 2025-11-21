namespace QLCHBanDienThoaiMoi.DTO
{
    public class ChiTietHoaDonNhapDTO
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; } = "";
        public int SoLuong { get; set; }
        public int GiaNhap { get; set; }
        public decimal ThanhTien => SoLuong * GiaNhap;
    }

    public class HoaDonNhapDTO
    {
        public int Id { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int NhaCungCapId { get; set; }
        public string NhaCungCapName { get; set; } = "";
        public int TongTien { get; set; }
        public List<ChiTietHoaDonNhapDTO> ChiTietSanPhams { get; set; } = new List<ChiTietHoaDonNhapDTO>();
    }
}
