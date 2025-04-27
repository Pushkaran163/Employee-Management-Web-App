using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyApp.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent DisplayFormattedDate(this IHtmlHelper htmlHelper, DateTime date)
        {
            return new HtmlString(date.ToString("MMM dd, yyyy"));
        }
    }
}
