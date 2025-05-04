using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApp.Application.Features.Todos.Commands.CreateTodo;
using TodoApp.Infrastructure.Database;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Repositories.Interface;
using TodoApp.Application.Mapping;
using TodoApp.Application.Services;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Validators;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c=> {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Todo API",
        Version = "v1",
        Description= "API for managing todo items",
        Contact = new OpenApiContact {Name = "DrCray", Email ="crayer@gmail.com"}
    });
});
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
// builder.Services.AddDbContext<TodoDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
// builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(TodoMappingProfile).Assembly);
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddApiVersioning(options => {

    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new QueryStringApiVersionReader("api-version")
    );
// integrate with Swagger

}).AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Todo: think about if we have a lot of services, need to automicaly register

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Tạo một Swagger endpoint cho mỗi phiên bản API
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.MapControllers();
app.UseHttpsRedirection();


app.Run();


