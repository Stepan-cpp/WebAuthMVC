using System.Security.Cryptography;
using System.Text;
using WebAuthMVC.BLL.Abstractions;

namespace WebAuthMVC.BLL.Services;

public class Sha512UserVerificationService : IUserVerificationService
{
   public string GetHash(string password)
   {
      byte[] hashBytes = SHA512.HashData(Encoding.ASCII.GetBytes(password));
      StringBuilder hashHex = new StringBuilder();
      foreach (var b in hashBytes)
      {
         hashHex.Append(b.ToString("X"));
      }

      return hashHex.ToString();
   }

   public bool Verify(string hash, string password)
   {
      return IUserVerificationService.ConstTimeHashEquals(hash, GetHash(password));
   }
}