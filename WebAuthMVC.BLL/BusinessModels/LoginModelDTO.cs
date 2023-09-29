using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.BusinessModels;

public class LoginModelDTO
{
   public string Password { get; set; }
   public string Username { get; set; }
}