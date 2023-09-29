using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAuthMVC.Models;

public class PasswordChangeViewModel
{
   [StringLength(64, MinimumLength = 8)]
   [Required]
   [BindRequired]
   public string NewPassword { get; set; }
}