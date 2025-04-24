using MediatR;
using TodoApp.Application.Dto;

namespace TodoApp.Application.Features.Todos.Queries;

public record GetTodoByIdQuery(int Id) : IRequest<TodoDto?>
{
   
}