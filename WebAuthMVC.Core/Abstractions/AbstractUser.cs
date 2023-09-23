namespace WebAuth;

public abstract class AbstractUser
{
   public abstract string Username { get; set; }
   public abstract string PasswordHash { get; set; }

   public bool VerifyPassword(string password, IPasswordEncryptionService encryptor)
   {
      return encryptor.Verify(PasswordHash, password);
   }
   
   public bool VerifyHash(string passwordHash, IPasswordEncryptionService encryptor)
   {
      return ConstTimeHashEquals(PasswordHash, passwordHash);
   }

   public static bool ConstTimeHashEquals(string hashA, string hashB)
   {
      if (hashA.Length != hashB.Length)
         throw new ArgumentException();

      bool equal = true;
      for (int i = 0; i < hashA.Length; i++)
         if (hashA[i] != hashB[i])
            equal = false;

      return equal;
   }

   public static string HashPassword(string password, IPasswordEncryptionService encryptor)
   {
      return encryptor.GetHash(password);
   }
}