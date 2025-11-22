using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Services;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThongKeSanPhamController : Controller
    {
        private readonly IThongKeSanPhamService _service;

        public ThongKeSanPhamController(IThongKeSanPhamService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var top3 = await _service.Top3SanPhamBanChay();
            return View(top3);
        }
    }
}
