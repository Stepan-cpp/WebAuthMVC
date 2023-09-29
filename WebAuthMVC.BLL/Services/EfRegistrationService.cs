using System.ComponentModel.DataAnnotations;
using WebAuthMVC.BLL.Abstractions;
using WebAuthMVC.BLL.BusinessModels;
using WebAuthMVC.BLL.Infrastructure;
using WebAuthMVC.DAL.Abstractions;
using WebAuthMVC.Infrastructure.Entities;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using ValidationException = WebAuthMVC.BLL.Infrastructure.ValidationException;

namespace WebAuthMVC.BLL.Services;

public class EfRegistrationService : IRegistrationService
{
   private IUnitOfWork Database { get; set; }
   private IUserVerificationService VerificationService { get; set; }

   public EfRegistrationService(IUnitOfWork database, IUserVerificationService verificationService)
   {
      Database = database;
      VerificationService = verificationService;
   }

   public void Dispose()
   {
      Database.Dispose();
   }

   public UserDto? Login(LoginModelDto creds)
   {
      User? user = Database.Users.Get(creds.Username);
      if (user is null)
         return null;

      if (!VerificationService.Verify(user.PasswordHash, creds.Password))
         return null;

      return user.ToDtoUser();
   }
   
   public UserDto? Login(IUserCredentials creds)
   {
      User? user = Database.Users.Get(creds.Username);
      if (user is null)
         return null;

      if (!IUserVerificationService.ConstTimeHashEquals(creds.PasswordHash, user.PasswordHash))
         return null;

      return user.ToDtoUser();
   }

   public void RegisterUser(RegisterModelDto user)
   {
      if (Database.Users.Get(user.Username) is not null)
         throw new ValidationException("This username is already taken", nameof(UserDto.Username));
      
      var results = new List<ValidationResult>();
      Validator.TryValidateObject(user, new ValidationContext(user), results);
      if (results.Count > 0)
         throw new ValidationException(results[0].ErrorMessage ?? "", results[0].MemberNames.First());

      var dalUser = user.ToDalUser();
      dalUser.PasswordHash = VerificationService.GetHash(user.Password);
      Database.Users.Create(dalUser);
      Database.Save();
   }

   public void DeleteUser(IUserCredentials credentials)
   {
      if (Login(credentials) is not UserDto user)
         throw new ValidationException("Invalid user credentials", "");
      
      Database.Users.Delete(new User { Username = user.Username});
      Database.Save();
   }

   public void ChangeUserPassword(IUserCredentials cred, string newPassword)
   {
      if (Login(cred) is not UserDto user)
         throw new ValidationException("Invalid user credentials", "");

      Database.Users.Get(user.Username)!.PasswordHash = VerificationService.GetHash(newPassword);
      Database.Save();
   }

   public bool IsUsernameInUse(string username)
   {
      return Database.Users.Get(username) != null;
   }
}