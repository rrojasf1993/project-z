using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HandwritenRecognition.Data.Repository;

public class HandwritenRecognitionRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly OcrDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public HandwritenRecognitionRepository(OcrDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    
    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null) query = query.Where(filter);

        foreach (var property in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(property);

        if (orderBy != null)
          
            return orderBy(query).ToList();
        return query.ToList();
    }

    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}