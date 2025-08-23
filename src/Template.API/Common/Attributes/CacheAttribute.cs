using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Template.API.Common.Attributes
{
    /// <summary>
    /// Caches the response of an action for a specified duration
    /// </summary>
    public class CacheAttribute : ActionFilterAttribute
    {
        private readonly int _cacheDurationInSeconds;
        private readonly string _cacheKeyPrefix;

        public CacheAttribute(int cacheDurationInSeconds = 300, string cacheKeyPrefix = "api_cache")
        {
            _cacheDurationInSeconds = cacheDurationInSeconds;
            _cacheKeyPrefix = cacheKeyPrefix;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = GenerateCacheKey(context);
            var cacheService = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            if (cacheService != null && cacheService.TryGetValue(cacheKey, out var cachedResponse))
            {
                context.Result = new ObjectResult(cachedResponse)
                {
                    StatusCode = 200
                };
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 200)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDurationInSeconds),
                    Priority = CacheItemPriority.Normal
                };

                cacheService?.Set(cacheKey, objectResult.Value, cacheOptions);
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var controller = context.Controller.GetType().Name;
            var action = context.ActionDescriptor.DisplayName;
            var parameters = string.Join("_", context.ActionArguments.Values.Select(v => v?.ToString() ?? "null"));

            var keyString = $"{_cacheKeyPrefix}_{controller}_{action}_{parameters}";

            // Create hash for consistent key length
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
