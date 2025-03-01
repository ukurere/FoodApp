using Microsoft.EntityFrameworkCore;
using FoodApp.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FoodAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(FoodAppContext)),
        sqlOptions =>
            sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name)));

var app = builder.Build();
app.Run();
