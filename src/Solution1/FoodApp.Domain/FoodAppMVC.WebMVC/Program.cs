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
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoodAppContext>();
    dbContext.Database.Migrate();
}

app.Run();
