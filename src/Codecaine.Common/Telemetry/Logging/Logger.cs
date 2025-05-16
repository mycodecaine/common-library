using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Grafana.Loki;

namespace Codecaine.Common.Telemetry.Logging
{
    public static  class Logger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (context, loggerConfiguration) =>
        {
            var env = context.HostingEnvironment;
            // make use this later Environment.GetEnvironmentVariable("GRAFANA_LOKI_URL");
            var grafanalokiUrl = "http://localhost:3100";
            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console()
                .WriteTo.GrafanaLoki(grafanalokiUrl,
                    [
                        new() { Key = "job", Value = "Codecaine.API" } // Set a custom job name
                        ]
                );// Send logs to Loki

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override("Codecaine.API.Presentation", LogEventLevel.Information);
            }


        };
    }
}
