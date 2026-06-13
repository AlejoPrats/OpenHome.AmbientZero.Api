using Microsoft.Extensions.Primitives;

namespace AmbientZero.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetLanguage(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("i18nLang", out StringValues result);

            return string.IsNullOrEmpty(result) ? "en" : result!;
        }

        public static void SetFileDownloadHeader(this HttpContext httpContext, string filename)
        {
            httpContext.Response.ContentType = "application/octet-stream";
            httpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{filename}\"");
        }
    }
}
