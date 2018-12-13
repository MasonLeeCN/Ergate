using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Ergate
{
    [AttributeUsage(AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : TypeFilterAttribute
    {
        public JwtAuthorizeAttribute() : base(typeof(JwtAuthorizeFilter))
        {
        }
    }

    public class JwtAuthorizeFilter : IAuthorizationFilter
    {
        public IConfiguration configuration;

        public JwtAuthorizeFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new ExceptionResult(new Exception("未找到Authorization Header"), false);
                return;
            }

            string authHeader = context.HttpContext.Request.Headers["Authorization"];

            var authStrs = authHeader.Split(' ');
            if (authStrs.Count() < 3)
            {
                context.Result = new ExceptionResult(new Exception("Authorization值不符合规范"), false);
                return;
            }

            var authType = authStrs[0];
            var token = authStrs[2];

            if (authType.ToLower() == "bearer")
            {
                var jwtKey = configuration.GetSection("JWt:Key").Value;
                if (string.IsNullOrEmpty(jwtKey))
                {
                    context.Result = new ExceptionResult(new Exception("鉴权配置异常"), false);
                    return;
                }

                var keyBuff = Encoding.UTF8.GetBytes(jwtKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBuff),
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateTokenReplay = false
                };

                try
                {
                    SecurityToken validatedToken;
                    tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                    var jwtToken = validatedToken as JwtSecurityToken;

                    var ci = new ClaimsIdentity();
                    ci.AddClaims(jwtToken.Claims);

                    context.HttpContext.User.AddIdentity(ci);
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else if (authType.ToLower()=="rsa")
            {
                
            }
            else
            {
                context.Result = new ExceptionResult(new Exception("无法识别的Authorization类型"), false);
                return;
            }
            
        }
    }
}