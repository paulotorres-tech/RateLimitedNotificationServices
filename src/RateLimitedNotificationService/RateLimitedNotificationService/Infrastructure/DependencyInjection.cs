using Microsoft.Extensions.DependencyInjection;
using RateLimitedNotificationService.Application.Services;
using RateLimitedNotificationService.Core.Interfaces;
using System.Threading.RateLimiting;

namespace RateLimitedNotificationService.Infrastructure
{
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
