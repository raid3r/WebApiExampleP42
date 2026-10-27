using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;

var builder = WebApplication.CreateBuilder(args);

// Singleton - створюэться одир раз пыд час завантежння і усі подальші запити буде використовуватися
// дин і той же екземпляр класу ProductService
var dataFilePath = builder.Configuration["ProductsDataFile"];
builder.Services.AddSingleton(new ProductService(dataFilePath));

builder.Services.AddOpenApi();

// TODO налашутвання роботи з кодуванням JSON

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.Use(async (/*HttpContext*/ context, /*Func<Task>*/ next) =>
{
    var logMessage = $"[{DateTime.Now}] {context.Request.Method} {context.Request.Path} ip:{context.Connection.RemoteIpAddress}";
    Console.WriteLine(logMessage);

    // Приклад валідації запиту (можна додати власну логіку) і повернення помилки 400 Bad Request, якщо запит не валідний
    //var valid = true; // TODO implement validation logic
    //if (!valid)
    //{
    //    context.Response.StatusCode = 400; // Bad Request
    //    await context.Response.WriteAsync("Invalid request");
    //    return;
    //}

    await next(context);
});

app.AddProductsRoutes();

app.Run();
















/*
Встановити пакети NuGet:
 
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore.SwaggerUI
  
 */

/*
Middleware, контролери і маршрути


поток вводу - запит - > Middleware - > обробник - > відповідь

поток вводу - запит - > Middleware - > контролер - > маршрут - > метод контролера - > відповідь


HttpContext - об'єкт, який містить інформацію про HTTP-запит і HTTP-відповідь. Він надає доступ до таких властивостей, як Request (запит), Response (відповідь), User (користувач), Session (сесія) та інші.


 */ 