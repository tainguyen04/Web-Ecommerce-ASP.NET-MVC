namespace QLCHBanDienThoaiMoi.Models
{
    public class DanhMucSanPham
    {
        public int Id { get; set; }
        public string TenDanhMuc { get; set; } = null!;
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
