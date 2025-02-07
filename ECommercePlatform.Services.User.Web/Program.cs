using ECommercePlatform.Services.User.Application;
using ECommercePlatform.Services.User.Infrastructure;
using ECommercePlatform.Services.User.Web;
using ECommercePlatform.Services.User.Web.Api;
using ECommercePlatform.Services.User.Web.Dto;
using ECommercePlatform.Shared.ServiceDefaults.Configuration;
using ECommercePlatform.Shared.ServiceDefaults.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddLogging();

builder.Services.Configure<ServiceConfiguration>(builder.Configuration.GetSection(nameof(ServiceConfiguration)));
builder.AddServiceDefaults(builder.Configuration.Get<ServiceConfiguration>() ??
                           throw new ArgumentException($"{nameof(ServiceConfiguration)} is not provided"));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseServiceDefaults();

app.MapApiEndpoints();

app.Run();
