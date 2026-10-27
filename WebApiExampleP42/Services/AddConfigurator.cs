using WebApiExampleP42.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApiExampleP42.Services;

public static class AddConfigurator
{
    public static void AddProductsRoutes(this WebApplication app)
    {
        app.MapGet("/products", async (ProductService productService, HttpContext httpContext) =>
        {
            var logMessage = $"[{DateTime.Now}] {httpContext.Request.Method} {httpContext.Request.Path} ip:{httpContext.Connection.RemoteIpAddress}";
            Console.WriteLine(logMessage);


            var products = await productService.GetAllProductsAsync();
            return Results.Ok(products);
        });

        // Отримати один продукт за його ідентифікатором GET /products/{id}
        // /products/1   
        // /products/2
        // /products/3
        // /products/{id:int}
        //            id - назва параметра (наша)   

        app.MapGet("/products/{id:int}", async (int id, ProductService productService, HttpContext httpContext) =>
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
        app.MapPut("/products/{id:int}", async (int id, [FromBody] Product updatedProduct, ProductService productService, HttpContext httpContext) =>
        {
            var product = await productService.UpdateProductAsync(id, updatedProduct); // Зберігаємо продукти
            if (product == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        });
        // Видалити продукт за його ідентифікатором DELETE /products/{id}
        app.MapDelete("/products/{id:int}", async (int id, ProductService productService, HttpContext httpContext) =>
        {
            var result = await productService.DeleteProductAsync(id);
            if (!result)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        });
    }

}

/*
 Створити проєкт веб-апі

Для повідомлень

повдомлення
- ід
- від кого (адреса)
- текст повідомлення
- дата час


реалізувати методи отримання усіх повідомлень, отримання одного повідомлення, повідомленнь від одного відправника (параметр адреса)
видалення повідомленян по ід

стоврити http файл з тестами методів щоб продемонтрувати як вони працюють
  
 */