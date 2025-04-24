using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Dto;
using TodoApp.Application.Interfaces;
using TodoApp.Infrastructure.Entity;

namespace TodoApp.Application.Features.Todos.Commands.CreateTodo;

public class CreateTodoCommandHandler(ITodoService todoService, ILogger<CreateTodoCommandHandler> logger)
    : IRequestHandler<CreateTodoCommand>
{

    public async Task Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        logger.LogTrace("Create todo handler called.");
        await todoService.AddTodoAsync(request.Todo);
    }
}
