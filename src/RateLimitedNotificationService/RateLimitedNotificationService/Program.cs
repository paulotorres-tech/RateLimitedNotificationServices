using RateLimitedNotificationService.Core.Entities;
using RateLimitedNotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();

// Load RateLimiting configuration
var rateLimitingConfig = new RateLimitingConfig();
builder.Configuration.GetSection("RateLimiting").Bind(rateLimitingConfig);
builder.Services.AddSingleton(rateLimitingConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
