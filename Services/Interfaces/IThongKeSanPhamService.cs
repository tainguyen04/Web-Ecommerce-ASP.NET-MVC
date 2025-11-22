using System.Collections.Generic;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Services
{
    public interface IThongKeSanPhamService
    {
        /// <summary>
        /// Lấy Top 3 sản phẩm bán chạy nhất theo tổng số lượng
        /// </summary>
        Task<List<ThongKeSanPhamDto>> Top3SanPhamBanChay();
    }

    public class ThongKeSanPhamDto
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; } = null!;
        public int TongSoLuong { get; set; }
    }
}
