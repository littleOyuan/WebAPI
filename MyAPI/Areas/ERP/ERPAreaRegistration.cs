using System.Web.Http;
using System.Web.Mvc;

namespace MyAPI.Areas.ERP
{
    public class ERPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ERP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            string areaName = AreaName;

            context.Routes.MapHttpRoute(
                areaName + "_area",
                string.Format("api/{0}", areaName + "/{controller}/{id}"),
                new { area = areaName, id = RouteParameter.Optional }
            );
        }
    }
}
