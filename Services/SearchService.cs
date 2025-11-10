using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SanPham>> TimKiemSanPhamAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<SanPham>();

            return await _context.SanPham
                .Include(sp => sp.DanhMucSanPham)
                .Include(sp => sp.KhuyenMai)
                .Where(sp => sp.TenSanPham.Contains(keyword))
                .ToListAsync();
        }
    }
}
