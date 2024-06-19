using System;
using System.Collections.Generic;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Application.Services
{
    public class RateLimiterService : IRateLimiterService
    {
        private readonly IMemoryCache _cache;
        private readonly Dictionary<string, (int limit, TimeSpan period)> _rateLimits;

        public RateLimiterService(IMemoryCache cache)
        {
            _cache = cache;
            _rateLimits = new Dictionary<string, (int limit, TimeSpan period)>
            {
                { "status", (2, TimeSpan.FromMinutes(1)) },
                { "news", (1, TimeSpan.FromDays(1)) },
                { "marketing", (3, TimeSpan.FromHours(1)) }
            };
        }

        public bool IsRequestAllowed(NotificationRequest request)
        {
            if (!_rateLimits.ContainsKey(request.Type))
                return true;

            var (limit, period) = _rateLimits[request.Type];
            var cacheKey = $"{request.Type}:{request.Recipient}";
            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = period;
                return 0;
            });

            if (requestCount >= limit)
                return false;

            _cache.Set(cacheKey, requestCount + 1);
            return true;
        }
    }
}
