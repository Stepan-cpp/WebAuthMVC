using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAuthMVC.Models;

public class LoginViewModel
{
   [BindRequired]
   [StringLength(32, MinimumLength = 1)]
   [Required]
   public string Username { get; set; }
   
   [BindRequired]
   [Required]
   public string Password { get; set; }
   
   public string? Message { get; set; }
}