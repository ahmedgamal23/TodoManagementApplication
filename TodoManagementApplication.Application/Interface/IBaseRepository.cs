using System.Linq.Expressions;
using TodoManagementApplication.Application.Core;

namespace TodoManagementApplication.Application.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        /*public ValueTask<IEnumerable<T>> GetAllAsync(
            int pageNumber = 1, int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            Expression<Func<T, bool>>? filter = null
        );*/
        
        public ValueTask<BaseModel<T>> GetAllAsync(
            int pageNumber = 1, int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            Expression<Func<T, bool>>? filter = null
        );

        public ValueTask<T?> GetAsync(Expression<Func<T, bool>>? filter = null);

        public ValueTask<T> CreateAsync(T model);

        public ValueTask<bool> UpdateAsync(T model);

        public ValueTask<bool> DeleteAsync(string id);
    }
}
