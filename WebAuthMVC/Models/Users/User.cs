using System.ComponentModel.DataAnnotations;
using WebAuth;

namespace WebAuthMVC.Models.Users;

public class User
{
   [Key]
   public string Username { get; set; }
   public string PasswordHash { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   
   public User()
   {
      
   }

   public bool Verify(string password, IPasswordEncryptionService encryptor)
   {
      return encryptor.Verify(PasswordHash, password);
   }

   public bool VerifyHash(string hash)
   {
      return SecureCompare(hash, PasswordHash);
   }

   public static string HashPassword(string password, IPasswordEncryptionService encryptionService)
   {
      return encryptionService.GetHash(password);
   }

   public static bool SecureCompare(string str1, string str2)
   {
      if (str1 == null || str2 == null)
      {
         return false;
      }

      int diff = str1.Length ^ str2.Length;

      for (int i = 0; i < str1.Length && i < str2.Length; i++)
      {
         diff |= (str1[i] ^ str2[i]);
      }

      return diff == 0;
   }
}