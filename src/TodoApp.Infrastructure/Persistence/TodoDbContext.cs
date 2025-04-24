using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Entity;

namespace TodoApp.Infrastructure.Database
{
    public class TodoDbContext : DbContext
    {
        public Guid InstanceId { get; } = Guid.NewGuid();

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            Console.WriteLine($"TodoDb created: {InstanceId}");
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=DBName;Integrated Security=True");
        // }
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}

