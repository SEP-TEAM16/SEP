using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using SEP.Common.DTO;
using SEP.Common.Enums;
using SEP.Bank2.Infrastructure;
using SEP.Bank2.Interfaces;
using SEP.Bank2.Services;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BankDatabase")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IBankService, BankService>();
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


var jss = new JavaScriptSerializer();

var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/auth");
httpRequest.Method = "POST";
httpRequest.ContentType = "application/json";

var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
var authKeys = new List<AuthKeyWithPortDTO>
{
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route"), int.Parse(appSettings.GetValue<string>("Info:Port")), true, appSettings.GetValue<string>("Info:RouteType"), 6),
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route1"), int.Parse(appSettings.GetValue<string>("Info:Port")), true, appSettings.GetValue<string>("Info:RouteType1"), 1),
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route2"), int.Parse(appSettings.GetValue<string>("Info:Port")), false, appSettings.GetValue<string>("Info:RouteType2"), 6)
};
streamWriter.Write(jss.Serialize(authKeys));
streamWriter.Close();
httpRequest.GetResponse();

app.Run();