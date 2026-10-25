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



// Отримати усі продукти GET /products
app.MapGet("/products", async (ProductService productService) =>
{
    var products = await productService.GetAllProductsAsync();
    return Results.Ok(products);
});

// Отримати один продукт за його ідентифікатором GET /products/{id}
// /products/1   
// /products/2
// /products/3
// /products/{id:int}
//            id - назва параметра (наша)   

app.MapGet("/products/{id:int}",  async (int id, ProductService productService) =>
{
    var product = await productService.GetProductByIdAsync(id);
    if (product == null)
    {
        return Results.NotFound(); // 404 Not Found
    }
    return Results.Ok(product);
});
// Додати новий продукт POST /products
app.MapPost("/products", async (ProductService productService, [FromBody] Product newProduct) =>
{
    var product = await productService.AddProductAsync(newProduct); // Зберігаємо продукти
    return Results.Ok(product);
});

// Оновити існуючий продукт PUT /products/{id}
app.MapPut("/products/{id:int}", async (int id, [FromBody] Product updatedProduct, ProductService productService) =>
{
    var product = await productService.UpdateProductAsync(id, updatedProduct); // Зберігаємо продукти
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
});
// Видалити продукт за його ідентифікатором DELETE /products/{id}
app.MapDelete("/products/{id:int}", async (int id, ProductService productService) =>
{
    var result = await productService.DeleteProductAsync(id);
    if (!result)
    {
        return Results.NotFound();
    }
    return Results.Ok();
});

app.Run();





/*
Встановити пакети NuGet:
 
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore.SwaggerUI
  
 */