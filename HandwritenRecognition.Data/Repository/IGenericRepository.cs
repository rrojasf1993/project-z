<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

=======
>>>>>>> 0fdab17697fc8c270d7acb38f3e90a7aa88b71af
namespace HandwritenRecognition.Data.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);

    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
    Task SaveChangesAsync();
}