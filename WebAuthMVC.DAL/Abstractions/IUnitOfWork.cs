using WebAuthMVC.Infrastructure.Entities;

namespace WebAuthMVC.DAL.Abstractions;

public interface IUnitOfWork : IDisposable
{
   IRepository<User, string> Users { get; }
   void Save();
}