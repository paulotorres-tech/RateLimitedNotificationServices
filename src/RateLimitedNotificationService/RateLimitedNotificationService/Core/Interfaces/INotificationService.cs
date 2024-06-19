using RateLimitedNotificationService.Core.Entities;

namespace RateLimitedNotificationService.Core.Interfaces
{
    public interface INotificationService
    {
        void Send(NotificationRequest request);
    }
}
