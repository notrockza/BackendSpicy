using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BackendSpicy.Models;
using BackendSpicy.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.MyInstallerExtensions(builder);

// Add services to the container.

//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//------------------------ 
// builder.Configuration.GetConnectionString เอาไว้อ่านค่าใน appsettings
// Services มาจาก WebApplicationBuilder
//var connectionString = builder.Configuration.GetConnectionString("DatabaseContext");
//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseSqlServer(connectionString)
//);
//------------------------

//var connectionString = builder.Configuration.GetConnectionString("DatabaseContext");
//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseSqlServer(connectionString)
//    );




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); //อนุญาตเข้าหาไฟล์จากภายนอกได้
app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);
app.UseCors(CorsInstaller.MyAllowAnyOrigins);


// app.UseAuthentication(); ต้องอยู่ก่อนนะ
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

