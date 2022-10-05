using CardStorageService.Core.Crypto;
using CardStorageService.Core.Crypto.Impl;
using CardStorageService.DataBase;
using CardStorageService.DataBase.Entities;
using CardStorageService.DataBase.Repository;
using CardStorageService.DataBase.Repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Logging service

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.RequestHeaders.Add("Authorization");
    logging.RequestHeaders.Add("X-Real-IP");
    logging.RequestHeaders.Add("X-Forwarded-For");
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
}).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });

#endregion

#region Confugure EF DBContext Service

builder.Services.AddDbContext<CardStorageServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
});

#endregion

#region Add User Services

builder.Services.AddScoped<IRepository<Card, Guid>, CardRepository>();
builder.Services.AddScoped<IRepository<Client, int>, ClientRepository>();
builder.Services.AddScoped<CryptoFatotySHA512>();
builder.Services.AddScoped<ICrypto>(s => s.GetRequiredService<CryptoFatotySHA512>());
builder.Services.AddSingleton<IAuthenticateService, AuthenticateService>();

#endregion

#region Configure JWT Tokens

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme =
    JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme =
    JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new
    TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticateService.SecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

#endregion

#region Configure Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Custom swagger", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// For [Autorize] working
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
