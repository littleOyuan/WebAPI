using System.Web.Http;
using MyAPI.Foundation;

namespace MyAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new CustomizedExceptionFilterAttribute());

            config.Routes.MapHttpRoute(
                "DefaultAreaActionApi",
                "api/{area}/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                "DefaultAreaApi",
                "api/{area}/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}
