using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Database {
    public class TodoContext : DbContext {
        public TodoContext() { }
        public TodoContext(DbContextOptions<TodoContext> options) : base (options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=DBName;Integrated Security=True");
        // }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}