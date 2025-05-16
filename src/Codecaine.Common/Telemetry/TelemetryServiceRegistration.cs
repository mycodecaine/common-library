using Codecaine.Common.Telemetry.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;

namespace Codecaine.Common.Telemetry
{
    public static class TelemetryServiceRegistration
    {
        public static void AddTelemetryRegistration(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog(Logger.ConfigureLogger);

            builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation() // Captures HTTP requests
                    .AddHttpClientInstrumentation() // Captures outgoing HTTP requests
                    .AddSource("Codecaine.SportApi")
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Codecaine.SportApi"))
                    .AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri("http://localhost:4317"); // OpenTelemetry Collector OTLP gRPC port
                        otlpOptions.Protocol = OtlpExportProtocol.Grpc;
                    })

                    //.AddJaegerExporter(options =>
                    //{
                    //    options.AgentHost = "localhost"; // Jaeger host
                    //    options.AgentPort = 6831; // Jaeger port
                    //})
                    .AddConsoleExporter(); // Debugging traces in console
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation() // Collects HTTP metrics
                    .AddRuntimeInstrumentation() // Collects GC, CPU, etc.
                    .AddMeter("Codecaine.SportApi.Metrics")
                    .AddPrometheusExporter(); // Exposes metrics for Prometheus
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(loggingBuilder =>
            {
                loggingBuilder.IncludeFormattedMessage = true;
                loggingBuilder.IncludeScopes = true;
                loggingBuilder.ParseStateValues = true;
            });
        }

        public static IApplicationBuilder UseTelemetryRegistration(this WebApplication app)
        {
            // Enable Serilog request logging
            app.UseSerilogRequestLogging();

            app.UseMetricServer();

            app.MapPrometheusScrapingEndpoint();

            return app;
        }
    }
}
