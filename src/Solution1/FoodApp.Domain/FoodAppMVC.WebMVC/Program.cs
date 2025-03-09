using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using FoodApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ✅ Логування шляху до конфігурації
Console.WriteLine($"🔍 Trying to load config: {builder.Environment.ContentRootPath}");

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("FoodAppContext");

// ✅ Переконайся, що connectionString не null
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("❌ Connection string is missing!");
    throw new Exception("Connection string is not set in configuration.");
}
Console.WriteLine($"🔗 Connection string: {connectionString}");

// ✅ Прив'язка до конкретного URL (порт можна змінити)
builder.WebHost.UseUrls("http://localhost:5255");

builder.Services.AddDbContext<FoodAppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Включаємо Swagger тільки в Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Редирект HTTP -> HTTPS
app.UseHttpsRedirection();

// ✅ Використання статичних файлів
app.UseStaticFiles();

// ✅ Налаштування маршрутизації
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dish}/{action=Index}/{id?}");
});

// ✅ Автоматичне застосування міграцій
Console.WriteLine("🚀 Applying migrations...");
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoodAppContext>();
    dbContext.Database.Migrate();
}
Console.WriteLine("✅ Migrations applied successfully.");

// ✅ Запуск сервера
app.Run();
