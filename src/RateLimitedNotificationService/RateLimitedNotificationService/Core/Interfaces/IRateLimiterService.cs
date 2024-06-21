using RateLimitedNotificationService.Core.Entities;

namespace RateLimitedNotificationService.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that enforces rate limiting on notification requests.
    /// </summary>
    public interface IRateLimiterService
    {
        /// <summary>
        /// Checks if the provided notification request is allowed based on the rate limiting rules.
        /// </summary>
        /// <param name="request">The notification request to be checked.</param>
        /// <returns>True if the request is allowed; otherwise, false.</returns>
        bool IsRequestAllowed(NotificationRequest request);
    }
}
