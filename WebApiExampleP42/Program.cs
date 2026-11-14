using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;
using WebApiExampleP42.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Дістаємо рядок підключення до бази даних з конфігурації
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Налаштовуємо контекст бази даних для використання SQLite
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString)
    );

// Додаємо репозиторії для моделей
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IRepository<Message>, MessageRepository>();



builder.Services.AddAutoMapper(c =>
{
    // ключ
    c.LicenseKey = "FREE-LIMITED-KEY";
}, typeof(Program));

// Додаємо сервіси для роботи з моделями
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MessageService>();


builder.Services.AddControllers();

builder.Services.AddOpenApi();

// TODO налашутвання роботи з кодуванням JSON

var app = builder.Build();

app.UseStaticFiles(); // дозволяє обслуговувати статичні файли з wwwroot

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "My API V1");
        c.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
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

app.UseMiddleware<UserAutorizationByToken>(); // додає до Items["User"] користувача, якщо токен валідний, або повертає 401 Unauthorized, якщо токен відсутній або не валідний
app.UseMiddleware<CheckMessageSender>(); // дістає з Items["User"] користувача і перевіряє чи співпадає його email з From у тілі запиту на відправку повідомлення, якщо ні - повертає 403 Forbidden



app.MapControllers();

app.Run();



/*
 
Створити в проєкті з повідомленнями
модель відправник
з можливістю
зареєструватися (пошта, пароль) - отримати токен на відправку 
авторизуватися (пошта, пароль) - отримати токен на відправку 

метод додавання повідомлення повинен тепер 
працювати лише тоді коли у полі від кого вказано емейл який було введено при реєстрації
а в заголовку є токен отриманий через метод авторизації чи реєстрації
перевірку повинна робити мідлвара (яка перевіряє тільки один маршрут - відправки)

  
 */


/*
 Додати AutoMapper пакет до проєкту
додати профілі перетворень моделей в DTO і навпаки
використати цей функціонал в своєму прожкті
 */








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


---

по цьому завданню зробити реалізацію з використанням контролерів
а сервіс роботи з повідомленнями розділити на окремі сервіси
1) сервіс роботи з моделями
2) сервіс збереження та завантадження з файлу
підключати їх через ін'єкцію залежностей

Додаткове завдання
створити middleware який дивиться на параметр з конфігурації Maintenance true/false (параметр в appsettings.json)
якщо true - запит на будьякий роут повертає відповідь в форматі html з повідомленням що сервер на облуговуванні
якщо false - все працює як завжди

 */


/*
 
 SOLID 
  
 Шар архітектури

    Валідація
    Контролер
        DTO моделі на вхід і вихід
        Розібрати запит на моделі, передати в сервіс
    Сервіс
        Бізнесова логіка
    Репозиторій
       Моделі
    DbContext
       база даних
    

  
  
  
  
 */

