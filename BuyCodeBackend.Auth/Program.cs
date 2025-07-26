using BuyCodeBackend.Auth.Domain.Models;
using BuyCodeBackend.Auth.Infrastructure.DBContexts;
using BuyCodeBackend.Auth.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuthContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDbConnectionString")));

builder.Services
    .AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AuthContext>();

#region Seeder

builder.Services.AddScoped<RolePermissionSeeder>();

#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var rolePermissionSeeder = scope.ServiceProvider.GetRequiredService<RolePermissionSeeder>();
    try
    {
        await rolePermissionSeeder.SeedAsync();
    }
    catch (Exception e)
    {
        app.Logger.LogCritical($"{e.Message}\n Stopping application");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

