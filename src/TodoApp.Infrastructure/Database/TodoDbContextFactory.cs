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
        public TodoDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            // Add directory existence check
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../TodoApp.API");
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"Configuration directory not found: {basePath}");
            }

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile($"appsettings.{environment}.json", optional: false)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
                optionsBuilder.UseNpgsql(config.GetConnectionString("Postgres"));

                return new TodoDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initialize DbContext: {ex.Message}", ex);
            }
        }
    }
}