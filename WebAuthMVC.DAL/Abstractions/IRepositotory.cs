namespace WebAuthMVC.DAL.Abstractions;

public interface IRepository<TV, in TK> where TV : class
{
   IEnumerable<TV> GetAll();
   TV? Get(TK id);
   IEnumerable<TV> Find(Func<TV, bool> predicate);
   void Create(TV item);
   void Update(TV item);
   void DeleteByKey(TK id);
   void Delete(TV item);
}