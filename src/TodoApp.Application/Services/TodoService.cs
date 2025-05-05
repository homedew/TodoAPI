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
using TodoApp.Infrastructure.UnitOfWork;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly ILogger<TodoService> _logger;
        public TodoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TodoService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task AddTodoAsync(TodoDto todo)
        {
            var todoItem = _mapper.Map<TodoItem>(todo);
            await _unitOfWork.TodoRepository.AddAsync(todoItem);
        }

        public async Task DeleteToDoAsync(int id)
        {
            if (!await _unitOfWork.TodoRepository.ExistsAsync(id))
            {
                _logger.LogWarning("Todo item not found with ID {id}", id);
                throw new KeyNotFoundException($"Todo item with Id {id} not found");
            }

            await _unitOfWork.TodoRepository.DeleteAsync(id);

        }

        public IQueryable<TodoDto> GetAllTodos()
        {
            var todos =  _unitOfWork.TodoRepository.GetAll();
            var projected = _mapper.ProjectTo<TodoDto>(todos);
            _logger.LogInformation("Fetching all todo items from database.");
            return projected;

        }

        public async Task<TodoDto?> GetTodoByIdAsync(int id)
        {
            var todos = await _unitOfWork.TodoRepository.GetByIdAsync(id);

            return _mapper.Map<TodoDto>(todos);
        }

        public async Task UpdateTodoAsync(int id, TodoDto todo)
        {
            var todoItems = _mapper.Map<TodoItem>(todo);
            await _unitOfWork.TodoRepository.UpdateAsync(todoItems);
        }

         public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.TodoRepository.ExistsAsync(id);
        }
    }
}