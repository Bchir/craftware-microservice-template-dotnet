using Asp.Versioning;
using System.Text;

namespace Craftware.ApiVersioning
{
    internal static class SunsetPolicyExtensions
    {
        public static string GetHtmlSunsetWarning(this SunsetPolicy sunsetPolicy)
        {
            var text = new StringBuilder();
            text.Append("<strong>");

            if (sunsetPolicy.Date is DateTimeOffset when)
                text.Append("This API will be sunset on ")
                    .Append(when.Date.ToShortDateString())
                    .Append('.');

            if (sunsetPolicy.HasLinks)
            {
                text.AppendLine();

                foreach (var link in sunsetPolicy.Links)
                {
                    if (link.Type == "text/html")
                    {
                        text.AppendLine();

                        if (link.Title.HasValue)
                        {
                            text.Append(link.Title.Value).Append(": ");
                        }

                        text.Append(link.LinkTarget.OriginalString);
                    }
                }
            }
            text.Append("</strong>");

            return text.ToString();
        }
    }
}