using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using SEP.Common.Models;
using SEP.PayPal.Infrastructure;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PayPalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PayPalDatabase")));
builder.Services.AddScoped<IPayPalService, PayPalService>();
builder.Services.AddAutoMapper(typeof(Program));

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


JavaScriptSerializer jss = new JavaScriptSerializer();

HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/auth");
httpRequest.Method = "POST";
httpRequest.ContentType = "application/json";

var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
AuthKey authKey = new AuthKey(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route"));
streamWriter.Write(jss.Serialize(authKey));
streamWriter.Close();
httpRequest.GetResponse();

app.Run();
