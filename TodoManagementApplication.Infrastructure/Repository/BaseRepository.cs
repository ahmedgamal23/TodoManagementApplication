using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoManagementApplication.Application.Core;
using TodoManagementApplication.Application.Interface;
using TodoManagementApplication.Domain.Data;
using TodoManagementApplication.Domain.Models;

namespace TodoManagementApplication.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet { get; set; }

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /*public async ValueTask<IEnumerable<T>> GetAllAsync(
            int pageNumber = 1, int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (include != null)
                query = include(query);

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }*/
        
        public async ValueTask<BaseModel<T>> GetAllAsync(
            int pageNumber = 1, int pageSize = 10,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
            Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (include != null)
                query = include(query);

            int total = (int)Math.Ceiling((double)query.Count() / pageSize);
            var result = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new BaseModel<T>
            {
                DataList = result,
                PageSize = pageSize,
                PageNumber = pageNumber,
                totalPages = total
            };
        }

        public async ValueTask<T?> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async ValueTask<T> CreateAsync(T model)
        {
            await _dbSet.AddAsync(model);
            return model;
        }

        public ValueTask<bool> UpdateAsync(T model)
        {
            var result = _dbSet.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            return new ValueTask<bool>(true);
        }

        public async ValueTask<bool> DeleteAsync(string id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop != null)
            {
                prop.SetValue(entity, true);
                _dbSet.Attach(entity);
                _context.Entry(entity).Property(prop.Name).IsModified = true;
                return true;
            }
            return false;
        }
    }
}
