using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRateLimiterService _rateLimiter;

        public NotificationService(IRateLimiterService rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        public void Send(NotificationRequest request)
        {
            if (_rateLimiter.IsRequestAllowed(request))
            {
                // Logic to send email notification
                Console.WriteLine($"Sending '{request.Type}' notification to {request.Recipient}: {request.Message}");
            }
            else
            {
                Console.WriteLine($"Rate limit exceeded for '{request.Type}' notification to {request.Recipient}");
            }
        }
    }
}
