using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MyAPI.Base;

namespace MyAPI.Foundation
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var sb = new StringBuilder();

                foreach (var modelState in actionContext.ModelState)
                {
                    if (modelState.Value.Errors.Count == 0)
                    {
                        continue;
                    }

                    string errorMsg = string.Join("", modelState.Value.Errors.Select(x => x.Exception == null ? x.ErrorMessage : x.Exception.Message));

                    sb.AppendFormat("参数 {0}：{1}", modelState.Key, errorMsg);
                }

                string exceptioMessage = sb.ToString();

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent<ResponseBase>(new ResponseBase(false, exceptioMessage), new JsonMediaTypeFormatter(), "application/json")
                };
            }
        }
    }
}