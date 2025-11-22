using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Services
{
    public class ThongKeSanPhamService : IThongKeSanPhamService
    {
        private readonly ApplicationDbContext _context;

        public ThongKeSanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ThongKeSanPhamDto>> Top3SanPhamBanChay()
        {
            var top3 = await _context.ChiTietHoaDonBan
                .Include(ct => ct.SanPham)
                .Where(ct => ct.HoaDonBan.TrangThai == Models.TrangThaiHoaDon.HoanThanh)
                .GroupBy(ct => new { ct.SanPhamId, ct.SanPham.TenSanPham })
                .Select(g => new ThongKeSanPhamDto
                {
                    SanPhamId = g.Key.SanPhamId,
                    TenSanPham = g.Key.TenSanPham,
                    TongSoLuong = g.Sum(x => x.SoLuong)
                })
                .OrderByDescending(x => x.TongSoLuong)
                .Take(3)
                .ToListAsync();

            return top3;
        }
    }
}
