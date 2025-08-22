using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Application.Common;

namespace Template.Application.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<PaginatedResult<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
