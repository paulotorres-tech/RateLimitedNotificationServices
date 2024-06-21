namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents the response of a notification send operation.
    /// </summary>
    public class NotificationResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the notification was sent successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a message describing the result of the send operation.
        /// </summary>
        public string Message { get; set; }
    }
}
