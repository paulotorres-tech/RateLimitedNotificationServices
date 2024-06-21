using Microsoft.AspNetCore.Mvc;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Presentation.Controllers
{
    /// <summary>
    /// API controller for handling notification requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service to send notifications.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Sends a notification based on the provided request.
        /// </summary>
        /// <param name="request">The notification request containing the type, recipient, and message.</param>
        /// <returns>An appropriate HTTP response indicating the result of the send operation.</returns>
        [HttpPost]
        public IActionResult SendNotification([FromBody] NotificationRequest request)
        {
            var response = _notificationService.Send(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(429, response); // 429 Too Many Requests
            }
        }
    }
}
