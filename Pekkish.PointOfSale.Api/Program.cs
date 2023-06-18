using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Pekkish.PointOfSale.Api;
using Pekkish.PointOfSale.Api.Models.Wati;
using Pekkish.PointOfSale.Api.Services;
using Pekkish.PointOfSale.DAL.Context;

var builder = WebApplication.CreateBuilder(args);

// SQL Server
builder.Services.AddDbContext<PointOfSaleContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionPekkishPointOfSaleDB")));

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AppSetting Config
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<WatiConfig>(builder.Configuration.GetSection("WatiConfig"));

// Services
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddTransient<IPointOfSaleService, PointOfSaleService>();
builder.Services.AddTransient<IWatiService, WatiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
