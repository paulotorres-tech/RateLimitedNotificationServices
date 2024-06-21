namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents the rate limit settings for a specific notification type.
    /// </summary>
    public class RateLimitSettings
    {
        /// <summary>
        /// Gets or sets the maximum number of notifications allowed within the specified period.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the time period within which the limit applies.
        /// </summary>
        public TimeSpan Period { get; set; }
    }

    /// <summary>
    /// Configuration class for rate limiting settings.
    /// </summary>
    public class RateLimitingConfig
    {
        /// <summary>
        /// Gets or sets the dictionary of rate limit settings for different notification types.
        /// </summary>
        public Dictionary<string, RateLimitSettings> Limits { get; set; }
    }
}
