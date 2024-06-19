using RateLimitedNotificationService.Core.Entities;

namespace RateLimitedNotificationService.Core.Interfaces
{
    public interface IRateLimiterService
    {
        bool IsRequestAllowed(NotificationRequest request);
    }
}
