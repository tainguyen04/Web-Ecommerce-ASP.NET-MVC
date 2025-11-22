using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeController : Controller
    {
        private readonly IThongKeService _thongKeService;
        private readonly ApplicationDbContext _context;

        public ThongKeController(IThongKeService thongKeService, ApplicationDbContext context)
        {
            _thongKeService = thongKeService;
            _context = context;
        }

        /// <summary>
        /// Hiển thị form chọn ngày bắt đầu, ngày kết thúc
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Thống kê doanh thu theo khoảng ngày
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Index(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                ViewBag.Error = "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.";
                return View();
            }

            // Tính doanh thu
            var doanhThu = await _thongKeService.DoanhThuTheoKhoang(tuNgay, denNgay);

            // Lấy danh sách hóa đơn trong khoảng thời gian
            var danhSachHoaDon = _context.HoaDonBan
                .Where(h => h.TrangThai == Models.TrangThaiHoaDon.HoanThanh
                         && h.NgayBan.Date >= tuNgay.Date
                         && h.NgayBan.Date <= denNgay.Date)
                .ToList();

            // Truyền dữ liệu ra view
            ViewBag.TuNgay = tuNgay.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay.ToString("yyyy-MM-dd");
            ViewBag.DoanhThu = doanhThu;
            ViewBag.DanhSach = danhSachHoaDon;

            return View();
        }
    }
}
