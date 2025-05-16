using Codecaine.Common.Abstractions;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using System.Diagnostics;

namespace Codecaine.Common.AspNetCore.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationIdGenerator correlationIdGenerator)
        {
            _next = next;
            _correlationIdGenerator = correlationIdGenerator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var activity = Activity.Current ?? new Activity("Incoming Request").Start();

            var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var existing)
                ? existing.ToString()
                : _correlationIdGenerator.Set().ToString();

            context.Response.Headers[CorrelationIdHeader] = correlationId;

            activity.SetTag("correlation_id", correlationId);
            

            await _next(context);

        }
    }
}
