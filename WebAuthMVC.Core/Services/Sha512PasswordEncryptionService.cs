using System.Security.Cryptography;
using System.Text;

namespace WebAuth.HashProviders;

public class Sha512PasswordEncryptionService : IPasswordEncryptionService
{
   
   
   public static bool SafeEquals(string a, string b)
   {
      if (a.Length != b.Length)
      {
         return false;
      }

      int result = 0;
      for (int i = 0; i < a.Length; i++)
      {
         result |= a[i] ^ b[i];
      }

      return result == 0;
   }

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
      return SafeEquals(hash, GetHash(password));
   }
}