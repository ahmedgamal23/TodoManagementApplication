using TodoManagementApplication.Application.Interface;
using TodoManagementApplication.Domain.Data;
using TodoManagementApplication.Domain.Models;

namespace TodoManagementApplication.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Todo> TodoRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            TodoRepository = new BaseRepository<Todo>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
