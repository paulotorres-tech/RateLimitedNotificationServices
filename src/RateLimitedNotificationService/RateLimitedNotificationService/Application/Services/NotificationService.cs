using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Application.Services
{
    /// <summary>
    /// Service responsible for sending notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IRateLimiterService _rateLimiterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="rateLimiterService">The rate limiter to enforce rate limits on notification requests.</param>
        public NotificationService(IRateLimiterService rateLimiterService)
        {
            _rateLimiterService = rateLimiterService;
        }

        /// <summary>
        /// Sends a notification based on the provided request.
        /// </summary>
        /// <param name="request">The notification request containing the type, recipient, and message.</param>
        /// <returns>A structured message indicating the result of the send operation.</returns>
        public NotificationResponse Send(NotificationRequest request)
        {
            if (_rateLimiterService.IsRequestAllowed(request))
            {
                // Logic to send email notification
                Console.WriteLine($"Sending '{request.Type}' notification to {request.Recipient}: {request.Message}");
                return new NotificationResponse
                {
                    Success = true,
                    Message = $"Notification '{request.Type}' sent to {request.Recipient}"
                };
            }
            else
            {
                // Rate limit exceeded
                Console.WriteLine($"Rate limit exceeded for '{request.Type}' notification to {request.Recipient}");
                return new NotificationResponse
                {
                    Success = false,
                    Message = $"Rate limit exceeded for '{request.Type}' notification to {request.Recipient}"
                };
            }
        }
    }
}
