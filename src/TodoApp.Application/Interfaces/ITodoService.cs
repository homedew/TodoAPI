using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Application.Dto;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoService
    {
        IQueryable<TodoDto> GetAllTodos();
        Task<TodoDto?>  GetTodoByIdAsync(int id);
        Task AddTodoAsync(TodoDto todo);
        Task UpdateTodoAsync(int id, TodoDto todo);
        Task DeleteToDoAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}