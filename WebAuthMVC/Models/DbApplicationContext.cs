using Microsoft.EntityFrameworkCore;
using WebAuthMVC.Models.Users;

namespace WebAuthMVC.Models;

public sealed class DbApplicationContext : DbContext
{
   public DbSet<User> Users => Set<User>();

   public DbApplicationContext() => Database.EnsureCreated();
   
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      optionsBuilder.UseSqlite("Data Source=users.db");
   }
}