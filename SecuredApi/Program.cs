using Microsoft.EntityFrameworkCore;
using SecuredApi.Data.Contextes;
using SecuredApi.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=root;Password=root", b => b.MigrationsAssembly("SecuredApi")));

builder.Services.AddIdentityCore<UserEntity>().AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
