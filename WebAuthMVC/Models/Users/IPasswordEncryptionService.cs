namespace WebAuth;

public interface IPasswordEncryptionService
{
   public string GetHash(string password);
   public bool Verify(string hash, string password);
}