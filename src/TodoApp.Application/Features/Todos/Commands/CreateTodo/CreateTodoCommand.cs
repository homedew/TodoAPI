using MediatR;
using TodoApp.Application.Dto;

namespace TodoApp.Application.Features.Todos.Commands.CreateTodo;

public record CreateTodoCommand(TodoDto Todo) : IRequest;