using FluentValidation;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Repository;
using GTBack.Repository.UnitOfWorks;
using GTBack.Service.Configurations;
using GTBack.Service.Services;
using GTBack.Service.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GTBack.WebAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClaimValue(this HttpContext context, string claimType)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        public static long? GetUserId(this HttpContext context)
        {
            var userId = context.GetClaimValue("Id");
            return string.IsNullOrEmpty(userId) ? null : long.Parse(userId);
        }
    }
}
