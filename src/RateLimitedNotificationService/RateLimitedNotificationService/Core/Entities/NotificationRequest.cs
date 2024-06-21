namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents a request to send a notification.
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// Gets or sets the type of the notification (e.g., status, news, marketing).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the recipient of the notification.
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string Message { get; set; }
    }
}
