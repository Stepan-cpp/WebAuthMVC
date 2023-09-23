using System.ComponentModel.DataAnnotations;
using WebAuth;

namespace WebAuthMVC.Infrastructure.Entities;

public class User : AbstractUser
{
   [Key]
   public override string Username { get; set; }
   public override string PasswordHash { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   
   public User()
   {
      
   }
}