using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dto;
using TodoApp.Application.Features.Todos.Commands.CreateTodo;
using TodoApp.Application.Features.Todos.Queries;
using TodoApp.Application.Features.Todos.Queries.GetTodoById;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Features.Todos.Commands.UpdateTodo;
using TodoApp.Application.Features.Todos.Commands.DeleteTodo;
using TodoApp.Application.Features.Todos.Queries.GetAllTodos;
using TodoApp.Application.Helper;

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
            var response = await mediator.Send(new GetAllTodosQuery(queryParams));
            logger.LogInformation("Getting todos - Page: {Page}, PageSize: {PageSize}", queryParams.Page,
                queryParams.PageSize);

            return Ok(response);
        }

        [HttpGet("v{version:apiVersion}/getTodo/{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await mediator.Send(new GetTodoByIdQuery(id));

            if (todo is null) return NotFound();

            logger.LogInformation("Get todo by id: {Id}", id);

            return Ok(todo);
        }

        [HttpPost("v{version:apiVersion}/postTodo")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<TodoDto>> PostTodo(TodoDto todo)
        {
            try
            {
                await mediator.Send(new CreateTodoCommand(todo));
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
            var isUpdateSuccess = await mediator.Send(new UpdateTodoCommand(todo.Id, todo));
            if (!isUpdateSuccess) return StatusCode(500,  "Internal Server Error");

            logger.LogInformation("Put todo by id: {Id}", todo.Id);

            return Ok();
        }

        [HttpDelete("v{version:apiVersion}/{id}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var isDeleteSucess = await mediator.Send(new DeleteTodoCommand(id));
            if(!isDeleteSucess) return StatusCode(500, "Internal Server Error");;

            return Ok();
        }
    }
}