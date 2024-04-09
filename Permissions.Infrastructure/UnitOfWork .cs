using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure
{
  public class UnitOfWork : IDisposable
  {
    private readonly ApplicationDbContext _context;
    private bool _disposed;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
      _context = context;
      _repositories = new Dictionary<Type, object>();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
      if (_repositories.Keys.Contains(typeof(TEntity)))
      {
        return _repositories[typeof(TEntity)] as IRepository<TEntity>;
      }

      var repository = new Repository<TEntity>(_context);
      _repositories.Add(typeof(TEntity), repository);
      return repository;
    }

    public void SaveChanges()
    {
      _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      await _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
        _disposed = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }

}
