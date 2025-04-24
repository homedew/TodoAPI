using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dto;
using TodoAPI.Helper;
using TodoApp.Application.Features.Todos.Commands.CreateTodo;
using TodoApp.Application.Features.Todos.Queries;
using TodoApp.Application.Features.Todos.Queries.GetTodoById;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controller
{
    [ApiController]
    //  [ApiVersion("1.0")]
    // [Route("api/v{version: apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class TodoController(
        ILogger<TodoController> logger,
        IMediator mediator
    ) : BaseController
    {
        [HttpGet("v{version:apiVersion}/gettodos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiVersion("1.0")]

        // Return Type Không Đúng: Các return type như Task<Action<Result<IEnumberable<TodoDTo>>>> có vẻ không đúng chuẩn ASP.NET Core, có thể gây lỗi khi Swagger cố gắng sinh tài liệu.
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos([FromQuery] TodoQueryParameters queryParams,
            CancellationToken cancellationToken = default)
        {
            // todo handle paging in service
            var query = todoService.GetAllTodos();
            var allowedSortFields = new[] { "title", "createdDate", "status" };

            // Filtering
            if (!string.IsNullOrEmpty(queryParams.Title))
            {
                query = query.Where(t => t.Title.Contains(queryParams.Title));
            }

            if (!string.IsNullOrEmpty(queryParams.Searching))
            {
                query = query.WhereContains(queryParams.Searching, "Title", "Description", "CreatedBy");
            }

            var response = await query.ToPagedResponseAsync(queryParams.Page,
                queryParams.PageSize,
                queryParams.SortBy,
                queryParams.SortOrder,
                allowedSortFields, cancellationToken);

            logger.LogInformation("Getting todos - Page: {Page}, PageSize: {PageSize}", queryParams.Page,
                queryParams.PageSize);

            return Ok(response);
        }

        [HttpGet("v{version:apiVersion}/getTodo/{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await mediator.Send(new GetTodoByIdQuery(id));
        
            if (todo is null)
            {
                return NotFound();
            }
        
            logger.LogInformation("Get todo by id: {Id}", id);
        
            return Ok(todo);
        }

        [HttpPost("v{version:apiVersion}/postTodo")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<TodoDto>> PostTodo(TodoDto todo)
        {
            try
            {
                await mediator.Send( new CreateTodoCommand(todo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

            logger.LogInformation("Post todo by id: {Id}", todo.Id);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPut("v{version:apiVersion}/{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> PutTodo(int id, TodoDto todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _todoService.UpdateTodoAsync(todo.Id, todo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _todoService.ExistsAsync(todo.Id))
                {
                    return NotFound();
                }

                throw;
            }

            _logger.LogInformation("Put todo by id: {Id}", todo.Id);

            return NoContent();
        }

        [HttpDelete("v{version:apiVersion}/{id}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                await _todoService.DeleteToDoAsync(id);
                _logger.LogInformation("Delete todo by id: {Id}", id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo item with id {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}