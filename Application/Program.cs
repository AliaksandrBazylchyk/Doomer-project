using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using Application.Data.Options;
using Application.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder();

string connection = "Server=(localdb)\\mssqllocaldb;Database=application;Trusted_Connection=True;";

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));


builder.Services.AddUserAuthentication(builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>());

builder.Services.AddScoped(typeof(AuthService));
builder.Services.AddScoped(typeof(TokenService));

// builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptchaV2"));

var app = builder.Build();

app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionMiddleware();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();