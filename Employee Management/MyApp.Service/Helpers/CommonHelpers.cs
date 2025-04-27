using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service.Helpers
{
    public static class CommonHelpers
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get the current username or return "System" as fallback.
        /// </summary>
        public static string GetCurrentUsername(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                Logger.Warn("HttpContext is null while getting current username.");
                return "System";
            }

            var username = httpContext.User?.Identity?.IsAuthenticated == true
                ? httpContext.User.Identity.Name
                : "System";

            Logger.Info($"Retrieved username: {username}");
            return username;
        }

        /// <summary>
        /// Get client IP address from request.
        /// </summary>
        public static string GetIpAddress(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                Logger.Warn("HttpContext is null while getting IP address.");
                return "Unknown";
            }

            var ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            Logger.Info($"Retrieved IP Address: {ipAddress}");
            return ipAddress;
        }
    }
}
