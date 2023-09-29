using Microsoft.EntityFrameworkCore;
using WebAuthMVC.DAL.Abstractions;
using WebAuthMVC.DAL.Contexts;
using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.DAL.Repositories;

public class UserRepository : IRepository<User, string>
{
   private ApplicationContext db;

   public UserRepository(ApplicationContext appContext)
   {
      db = appContext;
   }
   
   public IEnumerable<User> GetAll()
   {
      return db.Users;
   }

   public User? Get(string id)
   {
      return db.Users.Find(id);
   }

   public IEnumerable<User> Find(Func<User, bool> predicate)
   {
      return db.Users.Where(predicate);
   }

   public void Create(User item)
   {
      db.Users.Add(item);
   }

   public void Update(User item)
   {
      db.Users.Update(item);
   }

   public void DeleteByKey(string id)
   {
      var user = db.Users.Find(id);
      if (user is null)
         throw new ArgumentException($"There's no such a user with name {id}");
      
      db.Users.Remove(user);
   }

   public void Delete(User item)
   {
      db.Users.Remove(item);
   }
}