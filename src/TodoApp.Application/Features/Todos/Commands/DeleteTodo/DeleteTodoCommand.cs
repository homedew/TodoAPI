using MediatR;

namespace TodoApp.Application.Features.Todos.Commands.DeleteTodo;

public record DeleteTodoCommand(int Id): IRequest<bool>;