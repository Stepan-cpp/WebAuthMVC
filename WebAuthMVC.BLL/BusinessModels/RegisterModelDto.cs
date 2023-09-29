using System.ComponentModel.DataAnnotations;
using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.BusinessModels;

public class RegisterModelDto
{
   [StringLength(64, MinimumLength = 8)] public string Password { get; set; } = "";
   
   [StringLength(64, MinimumLength = 1)]
   public string Username { get; set; } = "";
   
   [StringLength(64, MinimumLength = 0)]
   public string? FirstName { get; set; }
   
   [StringLength(64, MinimumLength = 0)]
   public string? LastName { get; set; }
}