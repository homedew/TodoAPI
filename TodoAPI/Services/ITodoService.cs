using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Dtos;

namespace TodoAPI.Services
{
    public interface ITodoService
    {
        IQueryable<TodoDto> GetAllTodos();
        Task<TodoDto?>  GetTodoByIdAsync(int id);
        Task AddTodoAsync(TodoDto todo);
        Task UpdateTodoAsync(int id, TodoDto todo);
        Task DeleteToDoAsync(int id);
    }
}