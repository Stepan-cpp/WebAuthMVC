namespace WebAuthMVC.BLL.Abstractions;

public interface IUserVerificationService
{
   public string GetHash(string password);
   public bool Verify(string hash, string password);

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
}