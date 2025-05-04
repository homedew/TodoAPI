using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Dto;
using TodoApp.Application.Helper;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Features.Todos.Queries.GetAllTodos;

public class GetAllTodosQueryHandler(ITodoService todoService, ILogger<GetAllTodosQueryHandler> logger): IRequestHandler<GetAllTodosQuery, PagedResponse<TodoDto>>
{
    public async Task<PagedResponse<TodoDto>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var query = todoService.GetAllTodos();
        var allowedSortFields = new[] { "title", "createdDate", "status" };

        // Filtering
        if (!string.IsNullOrEmpty(request.QueryParams.Title))
        {
            query = query.Where(t => t.Title.Contains(request.QueryParams.Title));
        }

        if (!string.IsNullOrEmpty(request.QueryParams.Searching))
        {
            query = query.WhereContains(request.QueryParams.Searching, "Title", "Description", "CreatedBy");
        }
        
        var response = await query.ToPagedResponseAsync(request.QueryParams.Page,
            request.QueryParams.PageSize,
            request.QueryParams.SortBy,
            request.QueryParams.SortOrder,
            allowedSortFields, cancellationToken);
        
        return response;
    }
}