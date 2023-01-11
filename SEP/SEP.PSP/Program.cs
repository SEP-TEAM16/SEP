using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using SEP.Common.DTO;
using SEP.Common.Enums;
using SEP.PSP.Infrastructure;
using SEP.PSP.Interfaces;
using SEP.PSP.Services;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<PSPDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PSPDatabase")), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IPSPService, PSPService>();
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
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route1"), int.Parse(appSettings.GetValue<string>("Info:Port")), false, appSettings.GetValue<string>("Info:Route1Type"), 4),
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route2"), int.Parse(appSettings.GetValue<string>("Info:Port")), false, appSettings.GetValue<string>("Info:Route2Type"), 4),
    new AuthKeyWithPortDTO(appSettings.GetValue<string>("Info:Key"), appSettings.GetValue<string>("Info:Route3"), int.Parse(appSettings.GetValue<string>("Info:Port")), false, appSettings.GetValue<string>("Info:Route3Type"), 4)
};
streamWriter.Write(jss.Serialize(authKeys));
streamWriter.Close();
httpRequest.GetResponse();

var buildServiceProvider = builder.Services.BuildServiceProvider();
var someSingletonService = buildServiceProvider.GetRequiredService<IPSPService>();

app.Run();
