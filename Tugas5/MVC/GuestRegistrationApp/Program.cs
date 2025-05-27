using GuestRegistrationApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Konfigurasi Dapper
builder.Services.AddScoped<IDbConnection>(_ => 
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DatabaseContext>();

var app = builder.Build();
app.Urls.Add("http://localhost:5000");
app.Urls.Add("https://localhost:5001");

// ... middleware lainnya ...

app.Run();
