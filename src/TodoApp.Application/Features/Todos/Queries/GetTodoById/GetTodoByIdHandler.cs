using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Dto;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Features.Todos.Queries.GetTodoById;

public class GetTodoByIdQueryHandler(ITodoService todoService, ILogger<GetTodoByIdQueryHandler> logger): IRequestHandler<GetTodoByIdQuery, TodoDto?>
{
    public async Task<TodoDto?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogTrace("Get todo by id query called.");
        return await todoService.GetTodoByIdAsync(request.Id);
    }
}

