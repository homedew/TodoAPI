using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodo;

public class UpdateTodoCommandHandler(ITodoService todoService, ILogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, bool>
{
    public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        if (await todoService.ExistsAsync(request.Id)) return false;
        try
        {
            await todoService.UpdateTodoAsync(request.Id, request.Todo);
            logger.LogInformation("UpdateTodoCommandHandler success");
            // unitofwork.SaveChange();
            return true;

        } catch(Exception ex) {
            logger.LogError ($"UpdateTodoCommandHandler fail: {ex}");
            return false;

        }
    }
}