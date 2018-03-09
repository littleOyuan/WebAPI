using JWT;
using JWT.Serializers;
using MyAPI.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MyAPI.Foundation
{
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        private string JsonWebToken { get; set; }

        private const string HeaderAuthorizationScheme = "Bearer";
        private const string IssAtClaims = "issAt";
        private const string ApplicationIdClaims = "applicationId";
        private const string ApplicationId = "Demo";

        private const string CommunicationKey = "Colipu-Demo-Api";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null || !actionContext.Request.Headers.Authorization.Scheme.Equals(HeaderAuthorizationScheme))
            {
                throw new Exception("请求头身份认证不符合规范");
            }

            JsonWebToken = actionContext.Request.Headers.Authorization.Parameter;

            if (actionContext.Request.Method != HttpMethod.Get)
            {
                string requestString = actionContext.Request.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrWhiteSpace(requestString))
                {
                    throw new Exception("请求体不能为空");
                }
            }

            VerifyJsonWebToken();
        }

        private void VerifyJsonWebToken()
        {
            JsonNetSerializer jsonNetSerializer = new JsonNetSerializer();
            UtcDateTimeProvider utcDateTimeProvider = new UtcDateTimeProvider();
            JwtBase64UrlEncoder jwtBase64UrlEncoder = new JwtBase64UrlEncoder();
            JwtValidator jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);

            JwtDecoder jwtDecoder = new JwtDecoder(jsonNetSerializer, jwtValidator, jwtBase64UrlEncoder);

            try
            {
                IDictionary<string, object> payloadClaims = jwtDecoder.DecodeToObject(JsonWebToken, CommunicationKey, true);

                if (!payloadClaims.ContainsKey(IssAtClaims) || !payloadClaims.ContainsKey(ApplicationIdClaims) ||
                    !payloadClaims[ApplicationIdClaims].ToString().Equals(ApplicationId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Jwt中Payload不符合规范");
                }

                IsRequestExpire((long)payloadClaims[IssAtClaims]);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("身份认证失败：{0}", e.Message));
            }
        }

        private void IsRequestExpire(long timeStamp)
        {
            long tokenExpiryTime = 1000;

            bool isExpired = DateTimeHepler.ConvertToTimeStamp(DateTime.Now) - timeStamp > tokenExpiryTime;

            if (isExpired) throw new Exception("凭据过期");

        }
    }
}