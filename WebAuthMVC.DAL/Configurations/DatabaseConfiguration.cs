using Microsoft.Extensions.DependencyInjection;
using WebAuthMVC.DAL.Abstractions;
using WebAuthMVC.DAL.Contexts;
using WebAuthMVC.DAL.Repositories;

namespace WebAuthMVC.Infrastructure.Configurations;

public static class DatabaseConfiguration
{
   public static void RegisterDalDependencies(this IServiceCollection services)
   {
      services.AddSingleton<ApplicationContext>();
      services.AddSingleton<IUnitOfWork, EfUnitOfWork>();
   }
}