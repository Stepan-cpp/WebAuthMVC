using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAuthMVC.Models;

public class RegisterModel : LoginModel
{
   
   [BindRequired]
   [StringLength(32, MinimumLength = 1)]
   [Required]
   [Remote(action: "VerifyUsername", controller: "Home")]
   public override string Username { get; set; }
   
   [BindRequired]
   [StringLength(64, MinimumLength = 8)]
   [Required]
   public override string Password { get; set; }
   
   [BindRequired]
   [StringLength(32, MinimumLength = 1)]
   [Required]
   public string FirstName { get; set; }
   
   public string? LastName { get; set; }
}