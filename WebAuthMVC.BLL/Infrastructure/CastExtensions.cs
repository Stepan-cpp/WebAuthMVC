using WebAuthMVC.BLL.BusinessModels;
using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.BLL.Infrastructure;

public static class CastExtensions
{
   public static User ToDalUser(this UserDTO user)
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
   
   public static UserDTO ToDtoUser(this User user)
   {
      return new UserDTO
      {
         Username = user.Username, 
         FirstName = user.FirstName, 
         LastName = user.LastName, 
         IsAdmin = false,
         PasswordHash = user.PasswordHash
      };
   }
   
   public static User ToDalUser(this RegisterModelDTO user)
   {
      return new User
      {
         Username = user.Username, 
         FirstName = user.FirstName, 
         LastName = user.LastName,
         IsAdmin = false,
      };
   }
}