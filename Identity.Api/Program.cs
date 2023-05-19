using Identity.Api.Context;
using Identity.Api.Extensions;
using Identity.Api.Midlewares;
using Identity.Api.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var loger = new LoggerConfiguration()
    .WriteTo.File("log.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(loger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"));
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseErrorHandlerMiddleware();

app.MapControllers();

app.Run();
