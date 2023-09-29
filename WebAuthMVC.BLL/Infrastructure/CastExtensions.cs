using WebAuthMVC.BLL.BusinessModels;
using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.BLL.Infrastructure;

public static class CastExtensions
{
   public static User ToDalUser(this UserDto user)
   {
      return new User
      {
         Username = user.Username, 
         FirstName = user.FirstName, 
         LastName = user.LastName, 
         IsAdmin = false,
         PasswordHash = user.PasswordHash
      };
   }
   
   public static UserDto ToDtoUser(this User user)
   {
      return new UserDto
      {
         Username = user.Username, 
         FirstName = user.FirstName, 
         LastName = user.LastName, 
         IsAdmin = false,
         PasswordHash = user.PasswordHash
      };
   }
   
   public static User ToDalUser(this RegisterModelDto user)
   {
      return new User
      {
         Username = user.Username, 
         FirstName = user.FirstName ?? "", 
         LastName = user.LastName ?? "",
         IsAdmin = false,
      };
   }
}