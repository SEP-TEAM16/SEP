using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Persistence.Repositories;
using SEP.WebShop.Web.Authorization;
using SEP.WebShop.Web.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200",
                                              "https://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings")["SecurityKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped((provider) =>
{
    var connection = new SqlConnection(builder.Configuration.GetConnectionString("ConnectionString"));
    connection.Open();
    return connection;
});

builder.Services.AddScoped((serviceProvider) =>
{
    var connection = serviceProvider.GetService<SqlConnection>();
    return connection.BeginTransaction(IsolationLevel.ReadUncommitted);
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IWebShopUserRepository, WebShopUserRepository>();
builder.Services.AddScoped<ISubscriptionOptionRepository, SubscriptionOptionRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<WebShopUserService>();
builder.Services.AddScoped<SubscriptionOptionService>();
builder.Services.AddScoped<SubscriptionService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
