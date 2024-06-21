using RateLimitedNotificationService.Core.Entities;

namespace RateLimitedNotificationService.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that sends notifications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification based on the provided request.
        /// </summary>
        /// <param name="request">The notification request containing the type, recipient, and message.</param>
        /// <returns>A structured message indicating the result of the send operation.</returns>
        NotificationResponse Send(NotificationRequest request);
    }
}
