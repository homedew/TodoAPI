using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodoAPI.Dtos;
using TodoAPI.Entity;
using TodoAPI.Repository;

namespace TodoAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;
        public TodoService(ITodoRepository todoRepository, IMapper mapper)
        {
            _repository = todoRepository;
            _mapper = mapper;
        }
        public async Task AddTodoAsync(TodoDto todo)
        {
            var todoItem = _mapper.Map<TodoItem>(todo);
            await _repository.AddAsync(todoItem);
        }

        public async Task DeleteToDoAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<TodoDto>> GetAllTodosAsync()
        {
            var todos = await _repository.GetAllAsync();
            return _mapper.Map<List<TodoDto>>(todos);

        }

        public async Task<TodoDto?> GetTodoByIdAsync(int id)
        {
            var todos = await _repository.GetByIdAsync(id);
            
            return _mapper.Map<TodoDto>(todos);
        }

        public async Task UpdateTodoAsync(int id, TodoDto todo)
        {
            var todoItems = _mapper.Map<TodoItem>(todo);
            await _repository.UpdateAsync(todoItems);
        }
    }
}