using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecuredApi.Data.Entities;

namespace SecuredApi.Data.Contextes
{
    public class AppDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=root;Password=root");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
