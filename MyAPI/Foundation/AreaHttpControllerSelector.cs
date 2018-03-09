using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace MyAPI.Foundation
{
    public class AreaHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;

        /// <summary>  
        /// Lazy 当前程序集中包含的所有IHttpController反射集合，TKey为小写的Controller  
        /// </summary>  
        private readonly Lazy<ILookup<string, Type>> _apiControllerTypes;

        private ILookup<string, Type> ApiControllerTypes
        {
            get { return _apiControllerTypes.Value; }
        }

        public AreaHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
            _apiControllerTypes = new Lazy<ILookup<string, Type>>(GetApiControllerTypes);
        }

        /// <summary>  
        /// 获取当前程序集中 IHttpController反射集合  
        /// </summary>  
        /// <returns></returns>  
        private ILookup<string, Type> GetApiControllerTypes()
        {
            IAssembliesResolver assembliesResolver = _configuration.Services.GetAssembliesResolver();

            return _configuration.Services.GetHttpControllerTypeResolver()
                .GetControllerTypes(assembliesResolver)
                .ToLookup(type => type.Name.ToLower().Substring(0, type.Name.Length - ControllerSuffix.Length), type => type);
        }

        /// <summary>  
        /// Selects a System.Web.Http.Controllers.HttpControllerDescriptor for the given System.Net.Http.HttpRequestMessage.  
        /// </summary>  
        /// <param name="request"></param>  
        /// <returns></returns>  
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            string areaName = null;
            string controllerName = GetControllerName(request);
            HttpControllerDescriptor httpControllerDescriptor = null;

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("未找到与请求匹配的路由：'{0}'", request.RequestUri)));
            }

            Type[] apiControllerTypes = ApiControllerTypes[controllerName.ToLower()] as Type[] ?? ApiControllerTypes[controllerName.ToLower()].ToArray();

            if (apiControllerTypes.Any())
            {
                var endString = string.Format(".{0}{1}", controllerName, ControllerSuffix);

                IDictionary<string, object> routeDatas = request.GetRouteData().Values;

                if (routeDatas.Count > 1)
                {
                    if (!routeDatas.ContainsKey("area"))
                    {
                        throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("找到多个与请求匹配的类型：'{0}'", request.RequestUri)));
                    }
                    areaName = routeDatas["area"].ToString();
                }

                Type type = string.IsNullOrWhiteSpace(areaName)
                    ? apiControllerTypes.FirstOrDefault(t => t.FullName != null && t.FullName.EndsWith(endString, StringComparison.CurrentCultureIgnoreCase))
                    : apiControllerTypes.FirstOrDefault(t => t.FullName != null && t.FullName.Contains(areaName) && t.FullName.EndsWith(endString, StringComparison.CurrentCultureIgnoreCase));

                if (type != null)
                {
                    httpControllerDescriptor = new HttpControllerDescriptor(_configuration, controllerName, type);
                }
            }

            if (httpControllerDescriptor == null)
            {
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("未找到与请求匹配的路由：'{0}'", request.RequestUri)));
            }
            return httpControllerDescriptor;
        }
    }
}