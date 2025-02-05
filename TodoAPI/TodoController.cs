using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Database;
using TodoAPI.Entity;
using TodoAPI.Repository;

namespace TodoAPI.API
{

    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _logger;
        public TodoController(ITodoRepository repository, ILogger<TodoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodo(int id)
        {
            var todo = await _repository.GetByIdAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodo(TodoItem todo)
        {
            await _repository.AddAsync(todo);
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodo(int id, TodoItem todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(todo);
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
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}