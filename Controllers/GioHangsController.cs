using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLCHBanDienThoaiMoi.Data;
using QLCHBanDienThoaiMoi.Models;
using QLCHBanDienThoaiMoi.Services;

namespace QLCHBanDienThoaiMoi.Controllers
{
    public class GioHangsController : Controller
    {
        private readonly GioHangService _gioHangService;
        public GioHangsController(GioHangService gioHangService)
        {
            _gioHangService = gioHangService;
        }
        public string EnsureSessionIdExists()
        {
            var sesionId = HttpContext.Session.GetString("CartSessionId")?.Trim();
            if (string.IsNullOrEmpty(sesionId))
            {
                sesionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sesionId);
            }
            return sesionId;
        }
        public int GetKhachHangId()
        {
            var khachHangId = HttpContext.Session.GetInt32("KhachHangId");
            if (!khachHangId.HasValue)
            {
                throw new Exception("KhachHangId không tồn tại trong session");
            }
            return khachHangId.Value;
        }
        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            var sessionId = EnsureSessionIdExists();
            int? khachHangId = GetKhachHangId(); // Lấy từ người dùng đăng nhập nếu có
            Console.WriteLine($"SessionId: {sessionId}, KhachHangId: {khachHangId}");
            var gioHang = await _gioHangService.GetGioHangAsync(sessionId, khachHangId);
            return View(gioHang);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Thêm vào giỏ hàng khi người dùng chọn sản phẩm
        public async Task<IActionResult> AddToCart(int sanPhamId, int soLuong)
        {
            string sessionId = EnsureSessionIdExists();
            await _gioHangService.AddToCardAsync(sessionId, sanPhamId, null, soLuong);
            TempData["ThongBao"] = "Đã thêm vào giỏ hàng";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        

        

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string sessionId,int khachHangId,int sanPhamId)
        {
            sessionId = EnsureSessionIdExists();
            khachHangId = GetKhachHangId();
            var gioHang = await _gioHangService.DeletedSanPhamAsync(sessionId,khachHangId,sanPhamId);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
