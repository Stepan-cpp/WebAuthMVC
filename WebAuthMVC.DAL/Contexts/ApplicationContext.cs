using Microsoft.EntityFrameworkCore;
using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.DAL.Contexts;

public sealed class ApplicationContext : DbContext
{
   public DbSet<User> Users => Set<User>();

   public ApplicationContext() => Database.EnsureCreated();
   
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      optionsBuilder.UseSqlite(@"Data Source=D:\RiderProjects\WebAuthMVC\WebAuthMVC.DAL\users.db");
   }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
   }
}