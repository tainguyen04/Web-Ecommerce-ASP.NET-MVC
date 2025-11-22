using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly ApplicationDbContext _context;

        public ThongKeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> DoanhThuTheoKhoang(DateTime tuNgay, DateTime denNgay)
        {
            // Lọc hóa đơn đã hoàn thành trong khoảng thời gian
            return await _context.HoaDonBan
                .Where(h => h.TrangThai == TrangThaiHoaDon.HoanThanh
                         && h.NgayBan.Date >= tuNgay.Date
                         && h.NgayBan.Date <= denNgay.Date)
                .SumAsync(h => (int?)h.TongTien) ?? 0;
        }
    }
}
