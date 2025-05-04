using MediatR;
using TodoApp.Application.Dto;
using TodoApp.Application.Helper;

namespace TodoApp.Application.Features.Todos.Queries.GetAllTodos;

public record GetAllTodosQuery(TodoQueryParameters QueryParams): IRequest<PagedResponse<TodoDto>>;