using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Services
{

    public interface IHttpService
    {
        string? GetBaseUrl();
        bool TryGetCookieValue(string? cookieKey, out string? data);
        Task SetCookie(string? key, string? value, int expireDays = 30);
        Task RemoveCookie(string? key);
        Task<string?> GetRequestIpAddress();
    }
}
