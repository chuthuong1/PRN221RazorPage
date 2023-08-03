using Microsoft.Extensions.Options;
using System;
using WebApplication1.ChatHubSignarl; // si
using WebApplication1.Bussiness.IRepository;
using WebApplication1.Bussiness.Mapping;
using WebApplication1.Bussiness.Repository;
using WebApplication1.DataAccess.Models;
using WebApplication1.ChatHubSignarl;
using WebApplication1.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("Northwind");
builder.Services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IOrdersRepository, OrdersRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapHub<CartHub>("/CartHub");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();