using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Features.Todos.Commands.DeleteTodo;

public class DeleteTodoCommandHandler(ITodoService todoService, ILogger<DeleteTodoCommandHandler> logger) : IRequestHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        if (!await todoService.ExistsAsync(request.Id)) return false;

        try
        {
            await todoService.DeleteToDoAsync(request.Id);
            //save change
            logger.LogInformation("DeleteTodoCommandHandler success");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"DeleteTodoCommandHandler fail{ex}");
            return false;
        }
    }
}