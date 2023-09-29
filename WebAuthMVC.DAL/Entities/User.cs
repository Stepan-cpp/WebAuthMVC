using System.ComponentModel.DataAnnotations;

namespace WebAuthMVC.Infrastructure.Entities;

public class User
{
   [Key] public string Username { get; set; } = "";
   public string PasswordHash { get; set; } = "";
   public string FirstName { get; set; } = "";
   public string LastName { get; set; } = "";
   public bool IsAdmin { get; set; } = false;
}