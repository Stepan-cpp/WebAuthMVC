using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.BusinessModels;

public class LoginModelDto
{
   public string Password { get; set; } = "";
   public string Username { get; set; } = "";
}