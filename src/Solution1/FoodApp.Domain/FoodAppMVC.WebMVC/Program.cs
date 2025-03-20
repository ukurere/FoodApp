using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using FoodApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


Console.WriteLine($"🔍 Trying to load config: {builder.Environment.ContentRootPath}");

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("FoodAppContext");

/*
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing!");
    throw new Exception("Connection string is not set in configuration.");
}
Console.WriteLine($"Connection string: {connectionString}");
*/

builder.Services.AddDbContext<FoodAppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dishes}/{action=Index}/{id?}")
    .WithStaticAssets();

Console.WriteLine("🚀 Applying migrations...");
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoodAppContext>();
    dbContext.Database.Migrate();
}
Console.WriteLine("✅ Migrations applied successfully.");

app.Run();
