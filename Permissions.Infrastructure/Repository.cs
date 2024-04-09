using Microsoft.EntityFrameworkCore;

namespace Permissions.Infrastructure
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
      _context = context;
      _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
      return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }

    public void Add(TEntity entity)
    {
      _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
      _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
      _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      return await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
