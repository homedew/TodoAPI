using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace TodoApp.Infrastructure.Database
{
    public class TodoDbContextFactory: IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args) {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../TodoApp.API");
            var config = new ConfigurationBuilder()
                             .SetBasePath(basePath)
                            .AddJsonFile($"appsettings.{environment}.json", optional: false)
                            .Build();

             var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("Postgres"));

            return new TodoDbContext(optionsBuilder.Options);
        }
    }
}