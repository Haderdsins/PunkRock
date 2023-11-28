using System.Reflection;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Serilog; 


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);
builder.Host.UseMetricsWebTracking().UseMetrics(options => 
{
    // Настройка endpoints для Prometheus метрик
    options.EndpointOptions = endpointsOptions =>
    {
        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        endpointsOptions.EnvironmentInfoEndpointEnabled = false;
    };
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.AddMetrics();//добавили метрики
builder.Services.AddSwaggerGen(options =>
{
    // Дополнительная информация для генерации документации.
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v9999",
        Title = "Store Punk-Rock.API",
        Description = "Магазин Панк-Рока",
        Contact = new OpenApiContact
        {
            Name = "Васкин Максим Вадимович",
            Email = "maxifly0202@mail.ru",
            Url = new Uri("https://t.me/Haderdsins"),
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();