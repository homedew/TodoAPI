using MediatR;
using TodoApp.Application.Dto;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodo;

public record UpdateTodoCommand(int Id, TodoDto Todo): IRequest<bool>;