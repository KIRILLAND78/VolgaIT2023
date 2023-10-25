using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace VolgaIT2023.Middleware
{
    public class ErrorHandlerMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            try
            {
                await next(context);

                var ModelState = context.Features.Get<ModelStateFeature>()?.ModelState;
                if (ModelState!=null && !ModelState.IsValid)
                {
                    var errors = ModelState.Values.Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();
                    context.Response.Headers.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new ApiResponse(errors, "Error"));
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Headers.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ApiResponse(new { Message = ex.Message, Trace = (ex.StackTrace) }, "Error"));
            }
        }
    }
}
