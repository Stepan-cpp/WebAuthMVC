using WebAuthMVC.BLL.BusinessModels;

namespace WebAuthMVC.BLL.Abstractions;

public interface IRegistrationService : IDisposable
{
   UserDto? Login(LoginModelDto user);
   UserDto? Login(IUserCredentials creds);
   
   void RegisterUser(RegisterModelDto user);
   void DeleteUser(IUserCredentials user);
   void ChangeUserPassword(IUserCredentials cred, string newPassword);
   bool IsUsernameInUse(string username);
}