using Microsoft.EntityFrameworkCore;
using AspNetCore.ReCaptcha;
using Application.Extensions;

var builder = WebApplication.CreateBuilder();

string connection = "Server=(localdb)\\mssqllocaldb;Database=application;Trusted_Connection=True;";

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptchaV3"));

var app = builder.Build();
app.MapRazorPages();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseSession();
app.UseExceptionMiddleware();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();