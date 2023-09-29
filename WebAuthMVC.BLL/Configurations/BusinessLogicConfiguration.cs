using Microsoft.Extensions.DependencyInjection;
using WebAuthMVC.BLL.Abstractions;
using WebAuthMVC.BLL.Services;

namespace WebAuthMVC.BLL.Configurations;

public static class BusinessLogicConfiguration
{
   public static void RegisterBllDependencies(this IServiceCollection services)
   {
      services.AddSingleton<IUserVerificationService, BcryptUserVerificationService>();
      services.AddSingleton<IRegistrationService, EfRegistrationService>();
   }
}