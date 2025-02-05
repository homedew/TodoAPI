using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Database;
using TodoAPI.Entity;

namespace TodoAPI.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;
        public TodoRepository(TodoContext context)
        {
            _context = context;           
        }

        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            
            if(item is not null) {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
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