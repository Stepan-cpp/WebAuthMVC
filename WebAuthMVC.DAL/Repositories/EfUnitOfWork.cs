using System.Reflection.Metadata;
using WebAuthMVC.DAL.Abstractions;
using WebAuthMVC.DAL.Contexts;
using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.DAL.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
   private ApplicationContext Db { get; }
   private IRepository<User, string>? usersRepository;

   public IRepository<User, string> Users
   {
      get
      {
         if (usersRepository == null)
            usersRepository = new UserRepository(Db);

         return usersRepository;
      }
   }

   public EfUnitOfWork(ApplicationContext db)
   {
      Db = db;
   }

   public void Save()
   {
      Db.SaveChanges();
   }

   private bool disposed;

   public virtual void Dispose(bool disposing)
   {
      if (disposed)
         return;
      
      if (disposing)
         Db.Dispose();

      disposed = true;
   }

   public void Dispose()
   {
      Dispose(true);
      GC.SuppressFinalize(this);
   }
}