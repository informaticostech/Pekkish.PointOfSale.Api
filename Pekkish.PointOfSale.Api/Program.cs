using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.Api.Services;
using Pekkish.PointOfSale.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// SQL Server
builder.Services.AddDbContext<PointOfSaleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionPekkishPointOfSaleDB")));

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
// Services
builder.Services.AddTransient<IPointOfSaleService, PointOfSaleService>();
builder.Services.AddTransient<IWatiService, WatiService>();

//Wati Config
builder.Services.Configure<WatiConfig>(builder.Configuration.GetSection("WatiConfig"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
