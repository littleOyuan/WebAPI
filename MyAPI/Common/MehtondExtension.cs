using System.Net;
using System.Net.Http;
using System.Web;

namespace MyAPI.Common
{
    public static class MehtondExtension
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (!request.Properties.ContainsKey("MS_HttpContext")) return null;

            string userHostAddress = ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress;

            return string.IsNullOrWhiteSpace(userHostAddress) ? null : IPAddress.Parse(userHostAddress).ToString();
        }
    }
}