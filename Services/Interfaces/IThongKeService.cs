using System;
using System.Threading.Tasks;

namespace QLCHBanDienThoaiMoi.Services
{
    public interface IThongKeService
    {
        /// <summary>
        /// Tính tổng doanh thu trong khoảng từ tuNgay đến denNgay
        /// </summary>
        /// <param name="tuNgay">Ngày bắt đầu</param>
        /// <param name="denNgay">Ngày kết thúc</param>
        /// <returns>Tổng doanh thu</returns>
        Task<int> DoanhThuTheoKhoang(DateTime tuNgay, DateTime denNgay);
    }
}
