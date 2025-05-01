using Microsoft.AspNetCore.Builder;

namespace Codecaine.Common.AspNetCore.Middleware
{
    internal static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Configure the custom exception handler middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The configured application builder.</returns>
        internal static IApplicationBuilder UseCodecaineCommonExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
