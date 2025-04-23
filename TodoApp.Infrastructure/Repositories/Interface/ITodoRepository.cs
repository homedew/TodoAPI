using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Entity;
namespace TodoApp.Infrastructure.Repositories.Interface
{
    public interface ITodoRepository
    {
        Task<TodoItem?> GetByIdAsync(int id);
        IQueryable<TodoItem> GetAll();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}