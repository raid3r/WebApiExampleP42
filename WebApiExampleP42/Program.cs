using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;

var builder = WebApplication.CreateBuilder(args);

// Singleton - створюэться одир раз пыд час завантежння і усі подальші запити буде використовуватися
// дин і той же екземпляр класу ProductService
var dataFilePath = builder.Configuration["ProductsDataFile"];

// один раз створюється екземпляр JsonFileProductsProvider і використовується для всіх запитів
builder.Services.AddSingleton<IDataProvider<Product>>(new JsonFileProductsProvider(dataFilePath));
// Scoped - створюється один раз на кожен запит, і використовується для всіх об'єктів, які потребують його під час обробки цього запиту
builder.Services.AddScoped<ProductService>();
// Transient - створюється новий екземпляр ProductService для кожного запиту, і використовується тільки під час обробки цього запиту
//builder.Services.AddTransient<ProductService>();
builder.Services.AddControllers();

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

// Приклад middleware для логування запитів
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

app.MapControllers();

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