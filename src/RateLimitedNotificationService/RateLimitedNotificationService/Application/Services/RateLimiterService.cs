using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Application.Services
{
    /// <summary>
    /// Service responsible for enforcing rate limits on notification requests.
    /// </summary>
    public class RateLimiterService : IRateLimiterService
    {
        private readonly IMemoryCache _cache;
        private readonly Dictionary<string, RateLimitSettings> _rateLimits;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateLimiterService"/> class.
        /// </summary>
        /// <param name="cache">The in-memory cache for storing request counts.</param>
        /// <param name="rateLimitingConfig">The configuration for rate limiting settings.</param>
        public RateLimiterService(IMemoryCache cache, RateLimitingConfig rateLimitingConfig)
        {
            _cache = cache;
            _rateLimits = rateLimitingConfig.Limits;
        }

        /// <summary>
        /// Checks if the provided notification request is allowed based on the rate limiting rules.
        /// </summary>
        /// <param name="request">The notification request to be checked.</param>
        /// <returns>True if the request is allowed; otherwise, false.</returns>
        public bool IsRequestAllowed(NotificationRequest request)
        {
            // If there is no rate limit defined for the notification type, allow the request
            if (!_rateLimits.ContainsKey(request.Type))
                return true;

            // Get the rate limit settings for the notification type
            var rateLimitSettings = _rateLimits[request.Type];
            var cacheKey = $"{request.Type}:{request.Recipient}";

            // Retrieve the current request count from the cache or initialize it if not present
            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                // Set the cache expiration to match the rate limit period
                entry.AbsoluteExpirationRelativeToNow = rateLimitSettings.Period;
                return 0;
            });

            // Check if the request count exceeds the rate limit
            if (requestCount >= rateLimitSettings.Limit)
                return false;

            // Increment the request count and update the cache
            _cache.Set(cacheKey, requestCount + 1);
            return true;
        }
    }
}
