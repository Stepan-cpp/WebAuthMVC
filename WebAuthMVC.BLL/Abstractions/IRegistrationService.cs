using WebAuthMVC.BLL.BusinessModels;

namespace WebAuthMVC.BLL.Abstractions;

public interface IRegistrationService : IDisposable
{
   UserDTO? Login(LoginModelDTO user);
   UserDTO? Login(IUserCredentials creds);
   
   void RegisterUser(RegisterModelDTO user);
   void DeleteUser(IUserCredentials user);
   void ChangeUserPassword(IUserCredentials cred, string newPassword);
   bool IsUsernameInUse(string username);
}