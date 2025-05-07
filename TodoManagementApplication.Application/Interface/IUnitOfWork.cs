using TodoManagementApplication.Domain.Models;

namespace TodoManagementApplication.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<Todo> TodoRepository { get; }

        public ValueTask<int> SaveChangesAsync();
    }
}
