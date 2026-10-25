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

app.AddProductsRoutes();

app.Run();
















/*
Встановити пакети NuGet:
 
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore.SwaggerUI
  
 */