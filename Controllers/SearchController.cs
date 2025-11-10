using Microsoft.AspNetCore.Mvc;
using QLCHBanDienThoaiMoi.Services;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword)
        {
            var results = await _searchService.TimKiemSanPhamAsync(keyword);
            ViewBag.Keyword = keyword;
            return View(results);
        }
    }
}
