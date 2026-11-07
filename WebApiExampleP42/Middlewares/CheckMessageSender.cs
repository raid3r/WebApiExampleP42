using WebApiExampleP42.Models;
using WebApiExampleP42.Services;
using WebApiExampleP42.Models.DTO;


namespace WebApiExampleP42.Middlewares;

public class CheckMessageSender
{
    public RequestDelegate _next;
    public CheckMessageSender(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/api/message") || !(context.Request.Method == "POST"))
        {
            await _next(context);
            return;
        }

        //буферизуємо тіло запиту, щоб його можна було прочитати кілька разів
        context.Request.EnableBuffering();
        var request = await context.Request.ReadFromJsonAsync<AddMessageRequest>();
        // повертаємо позицію потоку на початок, щоб контролер міг прочитати тіло запиту
        context.Request.Body.Position = 0;


        if (request != null)
        {
            if (!context.Items.ContainsKey("User"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User not found in context. Authorization middleware might be missing.");
                return;
            }

            var user = context.Items["User"] as User;

            if (user.Email != request.From)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("You are not authorized to send messages from this email.");
                return;
            }
        }
        await _next(context);
    }

}
