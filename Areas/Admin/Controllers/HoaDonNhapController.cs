using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.DTO;
using System.Linq;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonNhapController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonNhapController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Index
        public IActionResult Index()
        {
            var model = _context.HoaDonNhap
            .Include(h => h.NhaCungCap)
            .ToList() // Lấy về danh sách trước khi mapping DTO
            .Select(h => new HoaDonNhapDTO
            {
                Id = h.Id,
                NgayLap = h.NgayLap,
                NhaCungCapId = h.NhaCungCapId,
                NhaCungCapName = h.NhaCungCap != null ? h.NhaCungCap.TenNCC : "",
                TongTien = h.TongTien
            })
            .ToList();


            return View(model);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.NCCList = new SelectList(_context.NhaCungCap, "Id", "TenNCC");
            ViewBag.SanPhamList = _context.SanPham.ToList();
            var model = new HoaDonNhapDTO();
            return View(model);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoaDonNhapDTO model)
        {
            if (model.ChiTietSanPhams == null)
                model.ChiTietSanPhams = new List<ChiTietHoaDonNhapDTO>();

            if (ModelState.IsValid)
            {
                var hoaDon = new HoaDonNhap
                {
                    NgayLap = model.NgayLap,
                    NhaCungCapId = model.NhaCungCapId,
                    TongTien = model.ChiTietSanPhams.Sum(c => c.SoLuong * c.GiaNhap),
                    ChiTietHoaDonNhaps = model.ChiTietSanPhams.Select(c => new ChiTietHoaDonNhap
                    {
                        SanPhamId = c.SanPhamId,
                        SoLuong = c.SoLuong,
                        GiaNhap = c.GiaNhap
                    }).ToList()
                };

                _context.HoaDonNhap.Add(hoaDon);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.NCCList = new SelectList(_context.NhaCungCap, "Id", "TenNCC", model.NhaCungCapId);
            ViewBag.SanPhamList = _context.SanPham.ToList();
            return View(model);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var hoaDon = _context.HoaDonNhap
                .Include(h => h.ChiTietHoaDonNhaps)
                .ThenInclude(c => c.SanPham)
                .Include(h => h.NhaCungCap)
                .FirstOrDefault(h => h.Id == id);

            if (hoaDon == null) return NotFound();

            var model = new HoaDonNhapDTO
            {
                Id = hoaDon.Id,
                NgayLap = hoaDon.NgayLap,
                NhaCungCapId = hoaDon.NhaCungCapId,
                NhaCungCapName = hoaDon.NhaCungCap?.TenNCC ?? "",
                TongTien = hoaDon.TongTien,
                ChiTietSanPhams = hoaDon.ChiTietHoaDonNhaps.Select(c => new ChiTietHoaDonNhapDTO
                {
                    SanPhamId = c.SanPhamId,
                    TenSanPham = c.SanPham?.TenSanPham ?? "",
                    SoLuong = c.SoLuong,
                    GiaNhap = c.GiaNhap
                }).ToList()
            };

            ViewBag.NCCList = new SelectList(_context.NhaCungCap, "Id", "TenNCC", model.NhaCungCapId);
            ViewBag.SanPhamList = _context.SanPham.ToList();
            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoaDonNhapDTO model)
        {
            if (model.ChiTietSanPhams == null)
                model.ChiTietSanPhams = new List<ChiTietHoaDonNhapDTO>();

            if (ModelState.IsValid)
            {
                var hoaDon = _context.HoaDonNhap
                    .Include(h => h.ChiTietHoaDonNhaps)
                    .FirstOrDefault(h => h.Id == model.Id);

                if (hoaDon == null) return NotFound();

                hoaDon.NgayLap = model.NgayLap;
                hoaDon.NhaCungCapId = model.NhaCungCapId;
                hoaDon.TongTien = model.ChiTietSanPhams.Sum(c => c.SoLuong * c.GiaNhap);

                _context.ChiTietHoaDonNhap.RemoveRange(hoaDon.ChiTietHoaDonNhaps);

                hoaDon.ChiTietHoaDonNhaps = model.ChiTietSanPhams.Select(c => new ChiTietHoaDonNhap
                {
                    SanPhamId = c.SanPhamId,
                    SoLuong = c.SoLuong,
                    GiaNhap = c.GiaNhap
                }).ToList();

                _context.Update(hoaDon);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.NCCList = new SelectList(_context.NhaCungCap, "Id", "TenNCC", model.NhaCungCapId);
            ViewBag.SanPhamList = _context.SanPham.ToList();
            return View(model);
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var hoaDon = _context.HoaDonNhap
                .Include(h => h.NhaCungCap)
                .Include(h => h.ChiTietHoaDonNhaps)
                .FirstOrDefault(h => h.Id == id);

            if (hoaDon == null) return NotFound();

            var model = new HoaDonNhapDTO
            {
                Id = hoaDon.Id,
                NgayLap = hoaDon.NgayLap,
                NhaCungCapId = hoaDon.NhaCungCapId,
                NhaCungCapName = hoaDon.NhaCungCap?.TenNCC ?? "",
                TongTien = hoaDon.TongTien
            };

            return View(model);
        }

        // POST: DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hoaDon = _context.HoaDonNhap
                .Include(h => h.ChiTietHoaDonNhaps)
                .FirstOrDefault(h => h.Id == id);

            if (hoaDon == null) return NotFound();

            if (hoaDon.ChiTietHoaDonNhaps != null && hoaDon.ChiTietHoaDonNhaps.Count > 0)
            {
                _context.ChiTietHoaDonNhap.RemoveRange(hoaDon.ChiTietHoaDonNhaps);
            }

            _context.HoaDonNhap.Remove(hoaDon);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
