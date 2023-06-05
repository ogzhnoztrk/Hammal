using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.DataAccess.Repository.IRepository
{
  public interface IRepository<T> where T : class
  {
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
    System.Threading.Tasks.Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    void Update(T entity);
    IQueryable<T> GetAll();
    /// <summary>
    /// Where sorgu yerine kullanılır. Örn:/ _repo.Find(x=>x.Id == xx.x)
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
  }
}
