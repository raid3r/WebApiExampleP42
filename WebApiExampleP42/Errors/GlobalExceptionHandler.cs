using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;

namespace WebApiExampleP42.Errors;

public class GlobalExceptionHandler(
    //ApiDbContext dbContext
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService,
    IWebHostEnvironment environment
    ) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
        )
    {
        /*
         * Що тут робимо:
         * - логування помилки
         * - відправка повідомлення про помилку в зовнішню систему моніторингу
         * - нотифікація розробників про критичну помилку
         * - дозбираємо додаткову інформацію про помилку (наприклад, стек трейс + користувач + дані які були в запиті)
         * - формуємо відповідь для клієнта (наприклад, у форматі JSON)
         */

        // логування помилки в базу даних
        // TODO

        // логування помилки в налаштований логгер (наприклад, Serilog, NLog, etc.)
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; // Internal Server Error




        // формуємо власну відповідь у форматі JSON з повідомленням про помилку
        await httpContext.Response.WriteAsJsonAsync(new
        {
            error = "An unexpected error occurred.",
            message = exception.Message,
            stackTrace = environment.IsDevelopment() ? exception.StackTrace : null // не показуємо стек трейс у продакшн середовищі
        });

        // Використовуємо ProblemDetails для формування відповіді про помилку
        //await problemDetailsService.TryWriteAsync(
        //    new ProblemDetailsContext
        //    {
        //        HttpContext = httpContext,
        //        ProblemDetails = new ProblemDetails
        //        {
        //            Status = StatusCodes.Status500InternalServerError,
        //            Title = "An unexpected error occurred.",
        //            Detail = exception.Message,
        //            Type = "https://example.com/probs/unexpected-error"
        //        }
        //    }
        //    );



        return true; // якщо обробка помилки була успішною, false - якщо ні
    }

}
