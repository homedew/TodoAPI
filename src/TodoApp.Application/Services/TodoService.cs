using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Dto;
using TodoApp.Infrastructure.Entity;
using TodoApp.Infrastructure.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoService> _logger;
        public TodoService(ITodoRepository todoRepository, IMapper mapper, ILogger<TodoService> logger)
        {
            _repository = todoRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task AddTodoAsync(TodoDto todo)
        {
            var todoItem = _mapper.Map<TodoItem>(todo);
            await _repository.AddAsync(todoItem);
        }

        public async Task DeleteToDoAsync(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                _logger.LogWarning("Todo item not found with ID {id}", id);
                throw new KeyNotFoundException($"Todo item with Id {id} not found");
            }

            await _repository.DeleteAsync(id);

        }

        public IQueryable<TodoDto> GetAllTodos()
        {
            var todos =  _repository.GetAll();
            var projected = _mapper.ProjectTo<TodoDto>(todos);
            _logger.LogInformation("Fetching all todo items from database.");
            return projected;

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

         public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
    }
}