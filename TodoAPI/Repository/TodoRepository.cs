using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TodoAPI.Database;
using TodoAPI.Entity;

namespace TodoAPI.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoRepository> _logger;
        private readonly IMemoryCache _cache;
        public TodoRepository(TodoContext context, ILogger<TodoRepository> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _context.TodoItems.Remove(new TodoItem { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TodoItems.AnyAsync(e => e.Id == id);
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}