using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAuthMVC.Models;

public class LoginModel
{
   [BindRequired]
   [StringLength(32, MinimumLength = 1)]
   [Required]
   public virtual string Username { get; set; }
   
   [Required]
   public virtual string Password { get; set; }
   
   public string? Message { get; set; }
}