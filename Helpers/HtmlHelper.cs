using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace QLCHBanDienThoaiMoi.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent FormatPrice(this IHtmlHelper htmlHelper, decimal price)
        {
            var formatted = string.Format(new CultureInfo("vi-VN"), "{0:N0} ₫", price);
            return new HtmlString(formatted);
        }
    }
}
