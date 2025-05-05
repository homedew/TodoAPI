using TodoApp.Infrastructure.Repositories.Interface;

namespace TodoApp.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ITodoRepository TodoRepository { get; }
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}