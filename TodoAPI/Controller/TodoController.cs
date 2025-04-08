using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Dtos;
using TodoAPI.Entity;
using TodoAPI.Helper;
using TodoAPI.Repository;
using TodoAPI.Services;

namespace TodoAPI.API
{

    [ApiController]
    //  [ApiVersion("1.0")]
    // [Route("api/v{version: apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class TodoController : BaseController
    {
        private readonly ITodoRepository _repository;
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;
        private readonly IMapper _mapper;
        public TodoController(ITodoRepository repository, ILogger<TodoController> logger, IMapper mapper, ITodoService todoService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _todoService = todoService;
        }


        [HttpGet("v{version:apiVersion}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiVersion("1.0")]

// Return Type Không Đúng: Các return type như Task<Action<Result<IEnumberable<TodoDTo>>>> có vẻ không đúng chuẩn ASP.NET Core, có thể gây lỗi khi Swagger cố gắng sinh tài liệu.
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            // todo handle paging in service
            var todos =  _todoService.GetAllTodos();
            var totalCount = await todos.CountAsync();

            var paginatedTodos = todos.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); 
            var data = _mapper.Map<List<TodoDto>>(paginatedTodos);

            var response = new PagedResponse<TodoDto> {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = data
            };

            return Ok(response);
        }

        [HttpGet("v{version:apiVersion}/getTodo/{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<TodoDto>> GetTodo(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TodoDto>(todo));
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> PostTodo(TodoDto todo)
        {
            try {
                 await _todoService.AddTodoAsync(todo);
            } catch(Exception ex) {
                return StatusCode(500,  "Internal Server Error");
            }
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
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
                if (!await _repository.ExistsAsync(todo.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                await _todoService.DeleteToDoAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new {message = ex.Message});
            }      
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo item with id {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}