using Microsoft.AspNetCore.Diagnostics;
using Randomizer.Common;
using System.Net;
using System.Text.Json;

namespace Randomizer.Api.Extensions;
public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    logger.LogError($"Something went wrong: {contextFeature.Error.Message}");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var error = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = ErrorMessages.IntervalServerError
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                }
            });
        });
    }
}
