using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pratice.Middleware
{
    public class JwtHendler
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtHendler(RequestDelegate next,IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token != null)
            {
                getUserDataFromToken(context, token);
            }
            await _next(context);
        }

        private void getUserDataFromToken(HttpContext context, string token)
        {
            try
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                tokenhandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Thisismysecretkey")),
                    ClockSkew = TimeSpan .Zero,
                    ValidateAudience = false ,
                    ValidateIssuer = false ,
                }, out SecurityToken validatedToken
                   );
                var jwtToken = (JwtSecurityToken)validatedToken;
                int UserId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                context.Items["UserId"] = UserId;
                int RoleId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value);
                context.Items["RoleId"] = RoleId;
            }
            catch (Exception e)
            {
            }   
        }
    }
}
