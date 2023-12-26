using System.Reflection;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Serilog;



var builder = WebApplication.CreateBuilder(args);
builder.Host.UseMetricsWebTracking().UseMetrics(options => 
{
    options.EndpointOptions = endpointsOptions =>
    {
        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        endpointsOptions.EnvironmentInfoEndpointEnabled = false;
    };
});





builder.Services.AddControllers();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.AddMetrics();
builder.Services.AddSwaggerGen(options =>
{
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

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
