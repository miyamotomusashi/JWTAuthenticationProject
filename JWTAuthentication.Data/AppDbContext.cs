using JWTAuthentication.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthentication.Data
{
  public class AppDbContext : IdentityDbContext<UserApp, IdentityRole, string>
  {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
      base.OnModelCreating(builder);

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserRefreshToken> UserREfreshTokens { get; set; }
  }
}
