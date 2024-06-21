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
        /// <summary>
        /// Adds infrastructure services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The same IServiceCollection with the added services.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IRateLimiterService, RateLimiterService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
