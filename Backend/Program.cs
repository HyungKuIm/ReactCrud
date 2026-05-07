using Backend.Models;
using Microsoft.EntityFrameworkCore;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// services
builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("Default") 
    ?? throw new ArgumentException("Connection string is null");
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlite(connectionString));

var app = builder.Build();


app.UseCors(MyAllowSpecificOrigins);

// middlewares
app.MapControllers();

app.Run();
