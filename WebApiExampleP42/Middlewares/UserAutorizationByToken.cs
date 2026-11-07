using WebApiExampleP42.Services;

namespace WebApiExampleP42.Middlewares;

public class UserAutorizationByToken
{
    public RequestDelegate _next;
    public UserAutorizationByToken(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService)
    {
        var header = context.Request.Headers["X-My-Authorization"].FirstOrDefault();
        var token = header?.ToString();

        //var exceptRoutes = new List<string> {
        //    "/swagger/index.html",
        //    "/openapi/v1.json",
        //    "/api/user/register", 
        //    "/api/user/login" 
        //};
        //if (exceptRoutes.Contains(context.Request.Path.Value?.ToLower())){
        //    await _next(context);
        //    return;
        //}

        if (string.IsNullOrEmpty(token))
        {
            //context.Response.StatusCode = 401; // Unauthorized
            //await context.Response.WriteAsync("Authorization token is missing.");
            await _next(context); // пропускаємо запит далі, бо можливо це маршрут який не потребує авторизації
            return;
        }

        var user = await userService.GetUserByTokenAsync(token);
        if (user == null)
        {
            //context.Response.StatusCode = 401; // Unauthorized
            //await context.Response.WriteAsync("Invalid authorization token.");
            await _next(context); // пропускаємо запит далі, бо можливо це маршрут який не потребує авторизації
            return;
        }

        // знайшли користувача, можна додати його до контексту, щоб контролери могли отримати доступ до нього
        context.Items["User"] = user;


        // запит - мідлвара 1 - мідлвара 2 - мідлвара 3 - контролер - метод контролера - відповідь

        await _next(context);
    }

}
