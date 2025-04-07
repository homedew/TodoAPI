using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Entity;

namespace TodoAPI.Repository
{
    public interface ITodoRepository
    {
        Task<TodoItem?> GetByIdAsync(int id);
        IQueryable<TodoItem>  GetAllAsync();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}