using Microsoft.AspNetCore.Mvc;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public IActionResult SendNotification([FromBody] NotificationRequest request)
        {
            _notificationService.Send(request);
            return Ok();
        }
    }
}
