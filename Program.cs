using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;
using VolgaIT2023;
using VolgaIT2023.Middleware;
using VolgaIT2023.Models;
using VolgaIT2023.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "DESCRIPTION",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat= "Bearer + JWT",
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "jwt",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });

});
builder.Services.AddTransient<TokenCheckerMiddleware>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => {
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    throw new UnauthorizedException();
                },
                OnForbidden = async context =>
                {
                    context.Response.StatusCode = 403;
                    throw new ForbiddenException();
                }
                
            };
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT_KEY"))),
                ValidateIssuerSigningKey = true
            };
        
        });
builder.Services.AddAuthorization(opts =>
{

    opts.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAssertion(s => {
            string? user = s.User.FindFirstValue(ClaimTypes.Role);
            if (user == null) return false;
            if (user!="Admin") return false;
            return true;
        });
    });
});
var g = builder.Configuration.GetConnectionString("AppDB");
builder.Services.AddDbContext<DatabaseContext>(o => { o.EnableDetailedErrors().EnableSensitiveDataLogging(false).UseNpgsql(builder.Configuration.GetValue<string>("DATABASE_CONNECTION_STRING")); });

builder.Services.AddTransient<JWTService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccountAdminService>();
builder.Services.AddTransient<TransportAdminService>();
builder.Services.AddTransient<RentAdminService>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<TransportService>();
builder.Services.AddTransient<RentService>();
builder.Services.AddTransient<PaymentService>();
builder.Services.AddTransient<ErrorHandlerMiddleware>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = ctx => new ModelStateExceptionFilter();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseMiddleware<TokenCheckerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
