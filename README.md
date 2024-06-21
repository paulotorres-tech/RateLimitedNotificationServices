# Rate-Limited Notification Service

## Project Description

The Rate-Limited Notification Service is a robust .NET Core 8 web API designed to manage and control the frequency of email notifications sent to recipients. Leveraging clean architecture principles, this service ensures users are protected from excessive emails due to system errors or misuse by implementing efficient rate-limiting mechanisms.

## Key Features

- **Rate Limiting**: Enforces predefined rate limits for various notification types (e.g., status updates, daily news, marketing).
- **Flexible Configuration**: Easily configurable rate limit rules for different notification types.
- **In-Memory Caching**: Uses in-memory caching to track request counts and enforce limits effectively.
- **Clean Architecture**: Promotes separation of concerns with distinct layers for core business logic, application services, infrastructure, and presentation.

## Technical Highlights

- **Framework**: .NET Core 8
- **Languages**: C#
- **Architecture**: Clean architecture with separate layers for core, application, infrastructure, and presentation.
- **Dependencies**:
  - `Microsoft.Extensions.Caching.Memory` for efficient in-memory caching.

## Project Structure

- **Core**: Contains business entities and interfaces.
- **Application**: Implements business logic and use cases.
- **Infrastructure**: Handles data access and external service integrations.
- **Presentation**: Exposes API endpoints for interaction.

## How It Works

1. **Rate Limiting**: The service checks if a notification request exceeds the allowed limit based on its type and recipient.
2. **Notification Service**: If the request is within the limit, the notification is sent; otherwise, a rate limit exceeded message is logged.
3. **API**: Exposes endpoints to accept notification requests, process them, and enforce rate limits efficiently.

## Getting Started

### Prerequisites

- .NET Core 8 SDK
- Visual Studio or any C# IDE
- Postman or curl for testing

### Installation

1. **Clone the repository**:
   ```sh
   git clone https://github.com/yourusername/RateLimitedNotificationService.git
   cd RateLimitedNotificationService
   ```

2. **Build the project**:
   ```sh
   dotnet build
   ```

3. **Run the project**:
   ```sh
   dotnet run
   ```

### Testing the API

Use Postman or curl to send POST requests to the `/notification` endpoint with a JSON body. Example payload:

```json
{
    "type": "news",
    "recipient": "user@example.com",
    "message": "This is a news update."
}
```

Example curl command:

```sh
curl -X POST "https://localhost:{port}/notification" -H "Content-Type: application/json" -d '{"type":"news","recipient":"user@example.com","message":"This is a news update."}'
```

### Configuration

The rate limits for different notification types can be configured in the `appsettings.json` file:

```json
{
  "RateLimiting": {
    "Limits": {
      "Status": {
        "Limit": 2,
        "Period": "00:01:00" // 1 minute
      },
      "News": {
        "Limit": 1,
        "Period": "1.00:00:00" // 1 day
      },
      "Marketing": {
        "Limit": 3,
        "Period": "01:00:00" // 1 hour
      }
    }
  }
}
```

### Code Overview

#### Core Layer

**Entities/NotificationRequest.cs**
```csharp
namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents a request to send a notification.
    /// </summary>
    public class NotificationRequest
    {
        public string Type { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
    }
}
```

**Entities/NotificationResponse.cs**
```csharp
namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents the response of a notification send operation.
    /// </summary>
    public class NotificationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
```

**Entities/RateLimitSettings.cs**
```csharp
namespace RateLimitedNotificationService.Core.Entities
{
    /// <summary>
    /// Represents the rate limit settings for a specific notification type.
    /// </summary>
    public class RateLimitSettings
    {
        public int Limit { get; set; }
        public TimeSpan Period { get; set; }
    }

    /// <summary>
    /// Configuration class for rate limiting settings.
    /// </summary>
    public class RateLimitingConfig
    {
        public Dictionary<string, RateLimitSettings> Limits { get; set; }
    }
}
```

**Interfaces/INotificationService.cs**
```csharp
namespace RateLimitedNotificationService.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that sends notifications.
    /// </summary>
    public interface INotificationService
    {
        NotificationResponse Send(NotificationRequest request);
    }
}
```

**Interfaces/IRateLimiterService.cs**
```csharp
namespace RateLimitedNotificationService.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that enforces rate limiting on notification requests.
    /// </summary>
    public interface IRateLimiterService
    {
        bool IsRequestAllowed(NotificationRequest request);
    }
}
```

#### Application Layer

**Services/NotificationService.cs**
```csharp
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

        public NotificationService(IRateLimiterService rateLimiterService)
        {
            _rateLimiterService = rateLimiterService;
        }

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
```

**Services/RateLimiterService.cs**
```csharp
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Application.Services
{
    /// <summary>
    /// Service responsible for enforcing rate limits on notification requests.
    /// </summary>
    public class RateLimiterService : IRateLimiterService
    {
        private readonly IMemoryCache _cache;
        private readonly Dictionary<string, RateLimitSettings> _rateLimits;

        public RateLimiterService(IMemoryCache cache, RateLimitingConfig rateLimitingConfig)
        {
            _cache = cache;
            _rateLimits = rateLimitingConfig.Limits;
        }

        public bool IsRequestAllowed(NotificationRequest request)
        {
            // If there is no rate limit defined for the notification type, allow the request
            if (!_rateLimits.ContainsKey(request.Type))
                return true;

            var rateLimitSettings = _rateLimits[request.Type];
            var cacheKey = $"{request.Type}:{request.Recipient}";

            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = rateLimitSettings.Period;
                return 0;
            });

            if (requestCount >= rateLimitSettings.Limit)
                return false;

            _cache.Set(cacheKey, requestCount + 1);
            return true;
        }
    }
}
```

#### Infrastructure Layer

**Infrastructure/DependencyInjection.cs**
```csharp
using Microsoft.Extensions.DependencyInjection;
using RateLimitedNotificationService.Application.Services;
using RateLimitedNotificationService.Core.Interfaces;

namespace RateLimitedNotificationService.Infrastructure
{
    /// <summary>
    /// Extension method for setting up dependency injection for infrastructure services.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IRateLimiterService, RateLimiterService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
```

#### Presentation Layer

**Controllers/NotificationController.cs**
```csharp
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

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

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
```

#### Program.cs
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Load RateLimiting configuration
var rateLimitingConfig = new RateLimitingConfig();
builder.Configuration.GetSection("RateLimiting").Bind(rateLimitingConfig);
builder.Services.AddSingleton(rateLimitingConfig);

// Add services to the container.
builder.Services.AddInfrastructure();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rate-Limited Notification Service API",
        Version = "v1",
        Description = "API for sending rate-limited notifications"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rate-Limited Notification Service API v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

