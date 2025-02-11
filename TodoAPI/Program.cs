using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoAPI.Database;
using TodoAPI.Repository;
using TodoAPI.Services;
using TodoAPI.Validator;

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
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITodoService, TodoService>();
// Todo: think about if we have a lot of services, need to automicaly register

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=> {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    });
}

app.MapControllers();
app.UseHttpsRedirection();


app.Run();


