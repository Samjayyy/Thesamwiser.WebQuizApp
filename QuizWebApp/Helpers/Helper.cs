using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace QuizWebApp.Helpers
{
    public static class Helper
    {
        public static IHtmlString ToLocalDateTimeString(this DateTime utc)
        {
            var timeZoneId = ConfigurationManager.AppSettings["TimeZone"];
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return new MvcHtmlString(TimeZoneInfo.ConvertTimeFromUtc(utc, timeZoneInfo).ToString("g"));
        }

        public static string AppUrl(this UrlHelper urlHelper)
        {
            var request = urlHelper.RequestContext.HttpContext.Request;
            return request.Url.GetLeftPart(UriPartial.Scheme | UriPartial.Authority);
        }
    }
}