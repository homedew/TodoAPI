using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApp.Infrastructure.Database;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Repositories.Interface;

namespace TodoApp.Infrastructure.UnitOfWork;

public class UnitOfWork: IUnitOfWork
{
    private readonly TodoDbContext _todoDbContext;
    private readonly IServiceProvider _serviceProvider;
    private ITodoRepository _todoRepository;

    public UnitOfWork(TodoDbContext todoDbContext, IServiceProvider serviceProvider)
    {
        _todoDbContext = todoDbContext;
        _serviceProvider = serviceProvider;
    }

    public ITodoRepository TodoRepository => _todoRepository ??= _serviceProvider.GetRequiredService<ITodoRepository>();
    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await _todoDbContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        _todoDbContext?.Dispose();
        GC.SuppressFinalize(this);
    }

}